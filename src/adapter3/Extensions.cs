using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.Shared.VSCodeDebugProtocol.Messages;
using Neo;
using EpicChain.BlockchainToolkit.Models;
using EpicChain.Cryptography.ECC;
using EpicChain.SmartContract;
using Newtonsoft.Json.Linq;

namespace EpicChainTraceVisualizer.Neo3
{
    using StackItem = EpicChain.VM.Types.StackItem;
    using StackItemType = EpicChain.VM.Types.StackItemType;
    using ByteString = EpicChain.VM.Types.ByteString;

    internal static class Extensions
    {
        public static bool StartsWith<T>(this ReadOnlyMemory<T> @this, ReadOnlySpan<T> value)
            where T : IEquatable<T>
        {
            return @this.Length >= value.Length
                && @this[..value.Length].Span.SequenceEqual(value);
        }

        public static bool TryGetMethod(this DebugInfo? debugInfo, int instructionPointer, [MaybeNullWhen(false)] out DebugInfo.Method method)
        {
            if (debugInfo is not null)
            {
                foreach (var m in debugInfo.Methods)
                {
                    if (m.Range.Start <= instructionPointer
                        && instructionPointer <= m.Range.End)
                    {
                        method = m;
                        return true;
                    }
                }
            }
            method = default;
            return false;
        }

        public static bool TryGetDocumentPath(this DebugInfo.SequencePoint @this, DebugInfo? debugInfo, out string path)
        {
            if (debugInfo is not null
                && @this.Document < debugInfo.Documents.Count)
            {
                path = debugInfo.Documents[@this.Document];
                return true;
            }

            path = "";
            return false;
        }

        public static bool PathEquals(this DebugInfo.SequencePoint @this, DebugInfo? debugInfo, string path)
        {
            return @this.TryGetDocumentPath(debugInfo, out var _path)
                && string.Equals(path, _path, StringComparison.OrdinalIgnoreCase);
        }

        public static bool TryGetCurrentSequencePoint(this DebugInfo.Method? method, int instructionPointer, out DebugInfo.SequencePoint sequencePoint)
        {
            if (method.HasValue)
            {
                var sequencePoints = method.Value.SequencePoints;
                if (sequencePoints.Count > 0)
                {
                    for (int i = sequencePoints.Count - 1; i >= 0; i--)
                    {
                        if (instructionPointer >= sequencePoints[i].Address)
                        {
                            sequencePoint = sequencePoints[i];
                            return true;
                        }
                    }

                    sequencePoint = sequencePoints[0];
                    return true;
                }
            }
            sequencePoint = default;
            return false;
        }
        public static bool TryFind<T>(this IEnumerable<T> @this, Predicate<T> predicate, [MaybeNullWhen(false)] out T value)
        {
            if (@this is IReadOnlyList<T> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (predicate(list[i]))
                    {
                        value = list[i];
                        return true;
                    }
                }
            }
            else
            {
                foreach (var v in @this)
                {
                    if (predicate(v))
                    {
                        value = v;
                        return true;
                    }
                }
            }
            value = default;
            return false;
        }

        //https://stackoverflow.com/a/1646913
        public static int GetSequenceHashCode(this ReadOnlySpan<byte> span)
        {
            unchecked
            {
                int hash = 17;
                for (int i = 0; i < span.Length; i++)
                {
                    hash = (hash * 31) + span[i];
                }
                return hash;
            }
        }

        public static int GetSequenceHashCode(this byte[] array)
        {
            return GetSequenceHashCode(array.AsSpan());
        }

        public static JToken ToJson(this StackItem item)
        {
            return item switch
            {
                EpicChain.VM.Types.Boolean _ => item.GetBoolean(),
                EpicChain.VM.Types.Buffer buffer => buffer.GetSpan().ToHexString(),
                EpicChain.VM.Types.ByteString byteString => byteString.GetSpan().ToHexString(),
                EpicChain.VM.Types.Integer @int => @int.GetInteger().ToString(),
                // EpicChain.VM.Types.InteropInterface _ => MakeVariable("InteropInterface"),
                EpicChain.VM.Types.Map map => MapToJson(map),
                EpicChain.VM.Types.Null _ => new JValue((object?)null),
                // EpicChain.VM.Types.Pointer _ => MakeVariable("Pointer"),
                EpicChain.VM.Types.Array array => new JArray(array.Select(i => i.ToJson())),
                _ => throw new NotSupportedException(),
            };

            static JObject MapToJson(EpicChain.VM.Types.Map map)
            {
                var json = new JObject();
                foreach (var (key, value) in map)
                {
                    json.Add(PrimitiveTypeToString(key), value.ToJson());
                }
                return json;
            }

            static string PrimitiveTypeToString(EpicChain.VM.Types.PrimitiveType item)
            {
                try
                {
                    return item.GetString() ?? throw new Exception();
                }
                catch
                {
                    return Convert.ToHexString(item.GetSpan());
                }
            }
        }

        // public static string? TryConvert(this StackItem item, CastOperation typeHint = CastOperation.None)
        // {
        //     try
        //     {
        //         return typeHint switch
        //         {
        //             CastOperation.Boolean => item.GetBoolean().ToString(),
        //             CastOperation.Integer => item.IsNull
        //                 ? BigInteger.Zero.ToString()
        //                 : ((EpicChain.VM.Types.Integer)item.ConvertTo(StackItemType.Integer)).GetInteger().ToString(),
        //             CastOperation.String => item.GetString(),
        //             CastOperation.HexString => ToHexString(item),
        //             CastOperation.ByteArray => ToHexString(item),
        //             // CastOperation.Address => ToAddress(item, 0x35),
        //             _ => null
        //         };
        //     }
        //     catch
        //     {
        //         return null;
        //     }

        //     // static string? ToAddress(StackItem item, byte version = 0x35)
        //     // {
        //     //     var span = item.GetSpan();
        //     //     if (span.Length == UInt160.Length)
        //     //     {
        //     //         var uint160 = new UInt160(span);
        //     //         return EpicChain.Wallets.Helper.ToAddress(uint160, version);
        //     //     }

        //     //     return null;
        //     // }

        //     static string ToHexString(StackItem item) => item.IsNull
        //         ? "<null>"
        //         : ((ByteString)item.ConvertTo(StackItemType.ByteString)).GetSpan().ToHexString();
        // }

        // public static T TryConvert<T>(this StackItem item) where T : StackItem
        // {
        //     switch (typeof(T))
        //     {
        //         case typeof(EpicChain.VM.Types.Integer):
        //             break;

        //     }

        // }

        public static Variable ToVariable(this StackItem item, IVariableManager manager, string name, ContractParameterType parameterType)
        {
            try
            {
                Variable? variable = parameterType switch
                {
                    ContractParameterType.Boolean => NewVariable(item.GetBoolean()),
                    ContractParameterType.ByteArray => ConvertByteArray(),
                    ContractParameterType.Hash160 => NewVariable(new UInt160(item.GetSpan())),
                    ContractParameterType.Hash256 => NewVariable(new UInt256(item.GetSpan())),
                    ContractParameterType.Integer => NewVariable(item.GetInteger()),
                    ContractParameterType.PublicKey => NewVariable(ECPoint.DecodePoint(item.GetSpan(), ECCurve.Secp256r1)),
                    ContractParameterType.Signature => ConvertByteArray(),
                    ContractParameterType.String => NewVariable(item.GetString()),
                    _ => null
                };

                if (variable != null) return variable;
            }
            catch { }

            return item.ToVariable(manager, name);

            Variable? NewVariable(object? obj) => obj == null ? null : new Variable { Name = name, Value = obj.ToString(), Type = parameterType.ToString() };

            Variable? ConvertByteArray()
            {
                if (item.IsNull) return new Variable { Name = name, Value = "<null>", Type = parameterType.ToString() };
                if (item is EpicChain.VM.Types.Buffer buffer) return ByteArrayContainer.Create(manager, buffer, name);
                if (item is EpicChain.VM.Types.ByteString byteString) return ByteArrayContainer.Create(manager, byteString, name);
                if (item is EpicChain.VM.Types.PrimitiveType)
                {
                    byteString = (ByteString)item.ConvertTo(StackItemType.ByteString);
                    return ByteArrayContainer.Create(manager, (ReadOnlyMemory<byte>)byteString, name);
                }
                return null;
            }
        }

        public static Variable ToVariable(this StackItem item, IVariableManager manager, string name)
        {
            return item switch
            {
                EpicChain.VM.Types.Array array => EpicChainArrayContainer.Create(manager, array, name),
                EpicChain.VM.Types.Boolean _ => new Variable { Name = name, Value = $"{item.GetBoolean()}", Type = "Boolean" },
                EpicChain.VM.Types.Buffer buffer => ByteArrayContainer.Create(manager, buffer, name),
                EpicChain.VM.Types.ByteString byteString => ByteArrayContainer.Create(manager, byteString, name),
                EpicChain.VM.Types.Integer @int => new Variable { Name = name, Value = $"{@int.GetInteger()}", Type = "Boolean" },
                EpicChain.VM.Types.InteropInterface _ => new Variable { Name = name, Value = "InteropInterface" },
                EpicChain.VM.Types.Map map => EpicChainMapContainer.Create(manager, map, name),
                EpicChain.VM.Types.Null _ => new Variable { Name = name, Value = "<null>", Type = "Null" },
                EpicChain.VM.Types.Pointer _ => new Variable { Name = name, Value = "Pointer" },
                _ => throw new NotSupportedException(),
            };
        }

        public static BigDecimal AsBigDecimal(this long value, byte? decimals = null)
        {
            decimals ??= EpicChain.SmartContract.Native.NativeContract.GAS.Decimals;
            return new BigDecimal((System.Numerics.BigInteger)value, decimals.Value);
        }
    }
}
