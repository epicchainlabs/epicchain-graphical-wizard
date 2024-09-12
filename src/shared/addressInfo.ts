type AddressInfo = {
  address: string;
  epicchainBalance: number;
  epicpulseBalance: number;
  allBalances: { [assetHash: string]: number };
};

export default AddressInfo;
