# Change Log

Welcome to the comprehensive Change Log for the EpicChain Graphical Wizard extension! This document is meticulously curated to keep you updated on all the significant updates, enhancements, and modifications made to the extension. It is designed to provide you with a clear understanding of new features, changes, and fixes that elevate the functionality and usability of the EpicChain Graphical Wizard. For more detailed guidance on structuring change logs, you can refer to [Keep a Changelog](http://keepachangelog.com/).

## Unreleased

### Added

- **Python Smart Contract Template**: We are excited to introduce a new Python smart contract template! This feature is specifically designed to streamline the development process for Python developers by providing a pre-configured template. It supports the creation and deployment of smart contracts within the EpicChain ecosystem and includes essential scaffolding and examples to facilitate a smoother development workflow.

### Changed

- **EpicChain Express Update to Version 3.1.38**: EpicChain Express has been upgraded to version 3.1.38. This update brings various enhancements and bug fixes aimed at improving performance, stability, and compatibility with the latest EpicChain features. We highly encourage users to upgrade to this version to benefit from the latest improvements.

### Changed

- **EpicChain Express Update to Version 3.0.21**: We have updated EpicChain Express to version 3.0.21. This update introduces important changes and optimizations that enhance the platform’s performance and integration with the EpicChain ecosystem.

### Removed

- **Contract Deployment Support**: Support for deploying contracts to the EpicChain Graphical Wizard MainNet has been removed. This decision helps streamline the extension’s functionality and focus on enhancing other aspects of the deployment process.

### Changed

- **EpicChain Express Update to Version 3.0.13**: EpicChain Express has been updated to version 3.0.13, incorporating crucial fixes and enhancements. These updates are essential for maintaining compatibility and improving overall user experience.
- **Configuration Update**: Adjustments have been made to the WELL_KNOWN_BLOCKCHAINS and SEED_URLS settings to support RC4/Final TestNet nodes. These changes ensure seamless connectivity and interaction with the latest TestNet configurations (#116).

### Added

- **Genesis Hash and Seed URLs**: We have added the genesis hash and known seed URLs for the EpicChain Graphical Wizard MainNet. These additions are crucial for setting up and connecting to the MainNet, providing users with the necessary information to initialize and connect to the network effectively.

### Changed

- **EpicChain Express Update to Version 3.0.5**: Updated EpicChain Express to version 3.0.5, which includes several improvements and optimizations designed to enhance the platform’s performance and reliability.

### Changed

- **Release Support and Major Version Bump**: Support for the EpicChain Graphical Wizard release has been incorporated, and the major version number has been bumped to 3. This change ensures consistency with the release version of the EpicChain Graphical Wizard, aligning it with the project’s broader versioning strategy.

### Changed

- **Invoke File Editor Enhancements**: The invoke file editor now supports editing invocation files that utilize arrays or objects as contract parameters. This enhancement improves flexibility and functionality, allowing users to manage more complex contract parameter configurations.
- **Extension Logs Visibility**: Extension logs are now visible in the VS Code Output Panel, offering better visibility into the extension’s operations and making debugging and monitoring more efficient.
- **Deployment Issue Fix**: An issue preventing contract deployment to the EpicChain Graphical Wizard TestNet has been resolved, ensuring successful contract deployments in the TestNet environment.
- **Menu Item Removal**: The "Invoke Contract" menu item has been removed for non-Express blockchain networks. This feature was incomplete, and its removal simplifies the extension’s interface.

### Changed

- **C# Smart Contract Template**: The C# smart contract template has been updated to include example unit tests. This addition provides users with practical examples of how to test their smart contracts, enhancing the development and quality assurance process.
- **Deployment Fix**: An issue preventing contracts from being deployed to TestNet has been fixed, resulting in smoother deployment operations.
- **EpicChain Express Update**: Updated to the latest RC3 build, incorporating recent fixes and improvements.
- **Workspace Support**: Limited support has been enabled within untrusted VS Code workspaces, allowing users to work with the extension in less secure environments.

### Changed

- **C# Sample Contract**: The C# sample contract now includes scaffolding for a ContractUpdate method, providing users with a template for implementing contract updates and streamlining the development process.
- **Hash Display Issue**: An issue where hashes were displayed in an unintuitive byte order in some contexts has been resolved, ensuring that hash values are presented in a more understandable format.
- **EpicChain Express Update**: Updated to the latest RC3 build to enhance compatibility and performance.
- **Asset Transfers**: Users can now transfer assets to arbitrary wallets, offering greater flexibility in managing assets within the EpicChain ecosystem.

### Removed

- **ms-dotnettools.vscode-dotnet-sdk Extension**: The usage of the ms-dotnettools.vscode-dotnet-sdk extension has been removed. This change simplifies the extension’s dependencies and aligns with updated best practices.

### Added

- **Smart Contract Panel**: A new panel has been introduced to display a list of all known smart contracts, allowing users to quickly access contract metadata. This feature enhances usability by providing easy access to contract information.
- **Wallet Panel**: A new panel has been added to show a list of all known wallets, facilitating quick access to balance information and wallet details.

### Changed

- **Java Smart Contract**: The process of creating a Java smart contract now automatically targets the latest version of EpicChain as specified in Maven Central. This change ensures that Java contracts remain compatible with the latest platform updates.
- **Dotnet Path Acquisition**: The extension now utilizes the ms-dotnettools.vscode-dotnet-sdk extension to acquire a path to dotnet, simplifying configuration and setup by eliminating the need for a global installation.
- **EpicChain Express Update**: Upgraded to the latest RC2 build, incorporating recent improvements and optimizations.
- **Dependency Updates**: Outdated npm package dependencies have been updated to the latest versions, including TypeScript 4, Node 14, React 17, and webpack 5. These updates ensure that the extension remains compatible with current technologies and standards.

### Added

- **Custom Block Time Option**: A new feature allows users to start EpicChain Express with a custom block time. This flexibility lets users configure the block time according to their specific needs and preferences.
- **Application Logs**: Application logs are now visible in the transaction details view, offering users more detailed information about transactions and system operations.
- **VS Code Command URIs**: Commands can now be invoked via VS Code Command URIs with arguments, enhancing the flexibility and automation of command execution.
- **Checkpoint Creation/Restoration**: Support for creating and restoring checkpoints has been added, enabling users to save and recover specific states of their blockchain environment.
- **UI Support for Stopping EpicChain Express**: User interface support for stopping EpicChain Express has been introduced, making it easier to manage and control the application.

### Changed

- **Contract Deployment Improvements**: Significant enhancements have been made to the contract deployment process, including the ability to right-click on nef files for deployment and improved messaging when debug info files are missing. These improvements aim to streamline the deployment workflow and provide clearer feedback.
- **Performance Improvements**: Various performance enhancements have been implemented to optimize the extension’s operation and overall user experience.

### Initial Release

This marks the initial release of the EpicChain Graphical Wizard extension. It provides the foundational features and functionality required for users to begin working with the EpicChain platform, laying the groundwork for future updates and enhancements.

We are committed to continuously improving the EpicChain Graphical Wizard extension and appreciate your feedback. Stay tuned for more updates and enhancements!
