# EpicChain Graphical Wizard

[![](https://github.com/epicchainlabs/epicchain-graphical-wizard/actions/workflows/build-vscode.yml/badge.svg)](https://github.com/epicchainlabs/epicchain-graphical-wizard/actions)

---

**Important Note:** The EpicChain Graphical Wizard **DOES NOT** use the same versioning scheme as the core EpicChain platform. For instance, Debugger v3.4 corresponds to EpicChain v3.3. The reasoning for this versioning strategy is detailed in our [Versioning Strategy documentation](https://github.com/epicchainlabs/epicchain-graphical-wizard#versioning-strategy).

---

## Introduction

The **EpicChain Graphical Wizard** is an essential tool designed specifically for EpicChain developers, allowing them to effectively debug their smart contracts within popular integrated development environments (IDEs) such as **Visual Studio** and **Visual Studio Code**. By leveraging these platforms, developers can streamline their workflow and enhance their productivity when working with smart contracts.

### Purpose and Compatibility

This graphical wizard is built with a primary focus on compatibility with the **core EpicChain project**. This ensures that the debugging process mirrors the execution of contracts in a production setting, thereby minimizing discrepancies between the development and production environments. The consistency in behavior is crucial for developers to trust that their contracts will perform as expected once deployed on the EpicChain network.

### Smart Contract Development

EpicChain supports the creation of smart contracts using a variety of programming languages, catering to a broad range of developers. However, it is important to note that for the debugger to function correctly, the smart contract compiler must emit additional information. This information is vital for the debugger, as it enables the mapping of **EpicChain Virtual Machine** instructions back to the original source code. This feature is supported by several compilers specifically designed for EpicChain smart contracts, providing developers with flexibility in their development choices.

## Versioning Strategy

As of **March 2022**, the EpicChain Graphical Wizard project has adopted the [recommended versioning guidelines by Visual Studio Code](https://code.visualstudio.com/api/working-with-extensions/publishing-extension#prerelease-extensions). This approach allows the Visual Studio Code Marketplace to provide both production and pre-release versions of the extension, giving developers the freedom to choose which version to install. Furthermore, the marketplace will automatically keep the extension updated, ensuring users always have access to the latest features and fixes.

Going forward, the versioning strategy for this extension will have even minor versions for production releases and odd minor versions for preview releases. For instance, the initial production release under this new versioning system will be v3.2, while the first pre-release will be v3.3.

**Note:** This project employs **NerdBank Git Versioning** to manage release version numbers. Consequently, the patch versions of public releases may not follow a sequential order.

## Installation

To install the EpicChain Graphical Wizard, a compatible [.NET runtime](https://dotnet.microsoft.com/download/dotnet) must be installed on your system. The required version of .NET Core will depend on the specific version of the EpicChain Graphical Wizard you are using. The following table outlines the necessary .NET Core versions based on the Wizard version:

| EpicChain Graphical Wizard Version | .NET Core Version |
|------------------------------------|-------------------|
| v3.1 and later                     | [v6.0](https://dotnet.microsoft.com/download/dotnet/6.0) (for EpicChain contracts) <br /> [v3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) (for EpicChain Legacy Contracts) |
| v3.0                               | [v5.0](https://dotnet.microsoft.com/download/dotnet/5.0) (for EpicChain contracts) <br /> [v3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) (for EpicChain Legacy Contracts) |
| v2.0 (unsupported)                 | [v5.0](https://dotnet.microsoft.com/download/dotnet/5.0) (for EpicChain contracts) <br /> [v3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) (for EpicChain Legacy Contracts) |
| v1.0                               | [v3.1](https://dotnet.microsoft.com/download/dotnet/3.1) |
| v0.9 (unsupported)                 | [v3.0](https://dotnet.microsoft.com/download/dotnet/3.0) |
| v0.5 (unsupported)                 | [v2.2](https://dotnet.microsoft.com/download/dotnet/2.2) |

### Visual Studio Code Installation

The EpicChain Graphical Wizard can be easily installed for Visual Studio Code via the [Visual Studio Code Marketplace](https://marketplace.visualstudio.com/vscode). Ensure that the appropriate [.NET runtime](https://dotnet.microsoft.com/download/dotnet-core) is installed prior to setup.

### Installation on Ubuntu

For users operating on Ubuntu, the checkpoint functionality requires the installation of specific dependencies. These can be installed using the following command:

```shell
sudo apt install libsnappy-dev libc6-dev -y
```

### Installation on macOS

Users on macOS will need to install **RocksDB** using [Homebrew](https://brew.sh/). This can be accomplished with the following command:

```shell
brew install rocksdb
```

### Installing Preview Releases

The EpicChain Graphical Wizard has a public [build server](https://dev.azure.com/ngdenterprise/Build/_build?definitionId=4&_a=summary), where developers can access preview builds of the debugger. To install these builds, navigate to the desired build, click the “Artifacts” button in the upper right corner, and download the corresponding **VSIX-package** artifact. This artifact is a zip file containing the debugger VSIX file, which can be installed manually. For further instructions on installing VSIX extensions in Visual Studio Code, refer to the [official Visual Studio Code documentation](https://code.visualstudio.com/docs/editor/extension-gallery#_install-from-a-vsix).

### Visual Studio Installation

Currently, the EpicChain Graphical Wizard for Visual Studio is in preview mode. To install, download a recent release of **epicchain-graphical-wizard-{version}.vsix** from the [GitHub release page](https://github.com/epicchainlabs/epicchain-graphical-wizard/releases) and double-click the downloaded file.

The EpicChain Graphical Wizard for Visual Studio is compatible with Visual Studio 2019 (Community, Professional, or Enterprise editions) but has not been tested with preview releases of Visual Studio 2022. Additionally, the Wizard requires [.NET v5.0](https://dotnet.microsoft.com/download/dotnet/5.0) to debug EpicChain contracts. It is important to note that debugging of EpicChain Legacy contracts is not supported within this version.

For comprehensive guidance on utilizing the EpicChain Graphical Wizard for Visual Studio, please refer to the [additional documentation](docs/visual-studio.md).

## A Message from the Engineer

Thank you for taking the time to explore the **EpicChain Graphical Wizard**! Your feedback is invaluable, and I am keen to hear your thoughts on this product.

If you appreciate the debugger, please share your feedback on [EpicChain Twitter](https://twitter.com/epicchainlabs), reach out via [Email](mailto:devs@epic-chain.org), or join us on the [EpicChain Discord server](https://discord.com/invite/tzxDUxnYT8).

Conversely, if there are aspects of the debugger that you find unsatisfactory, I encourage you to file issues in our [GitHub repository](https://github.com/epicchainlabs/epicchain-graphical-wizard/issues). While you can contact me through Twitter, Discord, or email, GitHub issues are our primary channel for tracking bugs and feature requests. Your input is crucial, so do not hesitate to voice your suggestions for improvements or new features.

Software development is typically a collaborative effort, but the EpicChain Graphical Wizard has primarily been a solo endeavor. I am excited about the prospect of future contributions from other developers; however, my perspective has shaped the design of this tool. Although I find the debugger intuitive, I recognize that your experience may differ. The Wizard was created for the broader EpicChain developer community, and I genuinely want to make it more accessible, intuitive, and effective for all users. Your insights are essential to achieving this goal.

Thank you once again for considering the **EpicChain Graphical Wizard**. I look forward to your feedback and suggestions for enhancement.

\- Harry Pierson (aka [DevHawk](http://devhawk.net)), Chief Architect at ngd enterprise
