# EpicChain Graphical Wizard Change Log

This change log documents all notable changes to the EpicChain Graphical Wizard. It is structured according to the guidelines outlined in [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) and adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

The version numbers for this project are managed automatically using [NerdBank.GitVersioning](https://github.com/AArnott/Nerdbank.GitVersioning). This tool streamlines the versioning process by setting the Semantic Versioning Patch value based on the Git height of the commit that generates each build.

## [Unreleased] 

### Added

- **Extension Download Feature**: A significant enhancement has been implemented, allowing the debugger to automatically download the debug adapter from GitHub if it is not available locally on the user's machine. This change addresses the common issue of missing dependencies, ensuring that users can seamlessly access the necessary tools without additional manual steps. (See pull request [#178](https://github.com/epicchainlabs/EpicChain Graphical Wizard/pull/178) for more details.)

### Changes

- **Debugger Update**: The debugger has undergone a comprehensive update tailored specifically for EpicChain. This update includes improvements in functionality and performance, ensuring that developers can debug their smart contracts more effectively.
- **Removal of Legacy Debugger**: The legacy debugger for EpicChain has been removed from the VSIX package. This decision simplifies the user experience by eliminating outdated components and focusing on the latest and most effective debugging tools. This update is reflected in issue [#157](https://github.com/epicchainlabs/epicchain Graphical Wizard/issues/157).

### Enhanced

- **Code Integration**: The BreakpointManager and DisassemblyManager have been integrated into the DebugSession. This integration streamlines the code structure and improves overall performance by consolidating related functionalities within the same module. (Refer to pull request [#161](https://github.com/epicchainlabs/EpicChain Graphical Wizard/issues/161) for more information.)
- **Expression Evaluator Improvements**: The ExpressionEvaluator has been enhanced to handle arrays and maps more efficiently. This improvement allows for more complex data structures to be evaluated during debugging sessions, facilitating a deeper understanding of the smart contracts in question.

### Improvements

- **Graphical Wizard Support**: The EpicChain Graphical Wizard has been updated to support version 2. This enhancement aligns the wizard with the latest features and capabilities, ensuring that users can effectively navigate and utilize the tools available to them. For more details on the current version 2 proposal, visit the documentation [here](https://github.com/devhawk/proposals/blob/devhawk/cd2l/nep-19.md).
- **Startup Enhancements**: Upon startup, all known debug information will now be associated with the deployed contract hash, enhancing the accuracy and usability of the debugger. This improvement is particularly beneficial for developers as it provides immediate context when debugging.
- **Script Loading Optimization**: All scripts will be loaded into the Disassembly Manager upon startup. This optimization ensures that all necessary scripts are readily available, improving the responsiveness and efficiency of the debugging process.

### Fixed

- **Null Reference Exception**: A critical bug that caused a null reference exception when displaying the Disassembly view has been resolved. This fix enhances the stability of the debugger, ensuring that users can access the disassembly features without encountering unexpected errors. (This fix is associated with commit e5d22e179593a7c94598ef9d1bdd633dd0e3de50.)

### Added Features

- **Storage Schema Support**: The debugger now includes comprehensive support for storage schemas. This feature allows developers to work with predefined structures for storing data on the blockchain, enhancing the organization and accessibility of stored information. For detailed information, please see the [overview](/docs/storage-schema-overview.md).
- **Storage Key Display**: Users can now view the full hex-encoded storage key for non-schematized storage. Additionally, there is an option to switch back to the hashed key view for non-schematized storage, providing flexibility in how storage keys are displayed and managed.

### Engineering Updates

- **Version Numbering Adoption**: The project has adopted recommendations from Visual Studio Code for managing version numbers. This change enables the distribution of pre-release versions of the debugger, providing users with early access to new features and improvements.
- **GitHub Workflows**: New GitHub workflows have been implemented to streamline push and pull request processes. These workflows facilitate better collaboration among developers and improve the overall development workflow.
- **Marketplace Publishing**: The release workflow for GitHub has been updated to publish the extension directly to the Visual Studio Code marketplace. This change simplifies the process of making new versions available to users, enhancing accessibility and usability.
- **Removal of Azure DevOps Workflow**: The Azure DevOps workflow has been removed as this repository no longer utilizes Azure DevOps for build and release services. This decision reflects a shift towards a more streamlined development environment focused on GitHub.

### General Updates

- **Nep17 Debug Experience**: The debugging experience for Nep17 tokens has been significantly improved, providing users with better insights and control while working with these token standards. (Refer to issue #84 for details.)
- **Script Hash Identification Change**: The debugger has been adapted to accommodate changes in script hash identification, ensuring compatibility with the latest standards and practices in the EpicChain ecosystem. (See issue #82 for more information.)
- **Oracle Response Debugging**: New capabilities have been introduced for debugging oracle responses, allowing developers to more effectively analyze and troubleshoot interactions with external data sources. (Refer to issue #80 for additional context.)
- **Development Mode Detection**: The extension now uses extension mode detection to identify when it is running in Development Mode. This enhancement improves the debugging experience by tailoring features and settings specifically for development scenarios. (See issue #79 for further details.)

---

This detailed changelog provides clarity and context for each modification made to the EpicChain Graphical Wizard, helping users understand the impact and significance of the changes. Let me know if you need further adjustments!