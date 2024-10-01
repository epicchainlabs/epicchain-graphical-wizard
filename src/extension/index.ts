import * as vscode from "vscode";

import ActiveConnection from "./activeConnection";
import AutoComplete from "./autoComplete";
import BlockchainIdentifier from "./blockchainIdentifier";
import BlockchainMonitorPool from "./blockchainMonitor/blockchainMonitorPool";
import BlockchainsTreeDataProvider from "./vscodeProviders/blockchainsTreeDataProvider";
import CheckpointDetector from "./fileDetectors/checkpointDetector";
import {
  CommandArguments,
  sanitizeCommandArguments,
} from "./commands/commandArguments";
import ContractDetector from "./fileDetectors/contractDetector";
import ContractsTreeDataProvider from "./vscodeProviders/contractsTreeDataProvider";
import Log from "./util/log";
import NeoCommands from "./commands/neoCommands";
import EpicChainExpress from "./EpicChainExpress/EpicChainExpress";
import EpicChainExpressCommands from "./commands/EpicChainExpressCommands";
import EpicChainExpressDetector from "./fileDetectors/EpicChainExpressDetector";
import EpicChainExpressInstanceManager from "./EpicChainExpress/EpicChainExpressInstanceManager";
import EpicChainInvokeFileEditorProvider from "./vscodeProviders/EpicChainInvokeFileEditorProvider";
import QuickStartViewProvider from "./vscodeProviders/quickStartViewProvider";
import ServerListDetector from "./fileDetectors/serverListDetector";
import Templates from "./templates/templates";
import TrackerCommands from "./commands/trackerCommands";
import WalletDetector from "./fileDetectors/walletDetector";
import WalletsTreeDataProvider from "./vscodeProviders/walletsTreeDataProvider";

const LOG_PREFIX = "index";

function registerCommand(
  context: vscode.ExtensionContext,
  commandId: string,
  handler: (commandArguments: CommandArguments) => Promise<void>
) {
  context.subscriptions.push(
    vscode.commands.registerCommand(
      commandId,
      async (context?: BlockchainIdentifier | vscode.Uri | any) => {
        let commandArguments: CommandArguments = {};
        if (context && !!(context as vscode.Uri).fsPath) {
          // Activation was by right-click on an item in the VS Code file explorer
          commandArguments.path = (context as vscode.Uri).fsPath;
        } else if (context && !!(context as BlockchainIdentifier).name) {
          // Activation was by right-click on an item in the Blockchain explorer
          commandArguments.blockchainIdentifier =
            context as BlockchainIdentifier;
        } else if (context) {
          // Activation by command URI containing query string parameters
          commandArguments = await sanitizeCommandArguments(context);
        }
        await handler(commandArguments);
      }
    )
  );
}

export async function activate(context: vscode.ExtensionContext) {
  Log.log(LOG_PREFIX, "Activating extension...");
  const blockchainMonitorPool = new BlockchainMonitorPool();
  const walletDetector = new WalletDetector();
  const EpicChainExpress = new EpicChainExpress(context);
  const serverListDetector = new ServerListDetector(context.extensionPath);
  const EpicChainExpressDetector = new EpicChainExpressDetector(
    context.extensionPath
  );
  const blockchainsTreeDataProvider = await BlockchainsTreeDataProvider.create(
    EpicChainExpressDetector,
    serverListDetector
  );
  const activeConnection = new ActiveConnection(
    blockchainsTreeDataProvider,
    blockchainMonitorPool
  );
  const contractDetector = new ContractDetector(activeConnection);
  const EpicChainExpressInstanceManager = new EpicChainExpressInstanceManager(
    EpicChainExpress,
    activeConnection
  );
  const autoComplete = new AutoComplete(
    context,
    EpicChainExpress,
    activeConnection,
    contractDetector,
    walletDetector,
    EpicChainExpressDetector
  );
  const walletsTreeDataProvider = new WalletsTreeDataProvider(
    context.extensionPath,
    activeConnection,
    walletDetector,
    autoComplete
  );
  const contractsTreeDataProvider = new ContractsTreeDataProvider(
    context.extensionPath,
    autoComplete,
    contractDetector
  );
  const EpicChainInvokeFileEditorProvider =
    new EpicChainInvokeFileEditorProvider(
      context,
      activeConnection,
      EpicChainExpress,
      autoComplete
    );
  const checkpointDetector = new CheckpointDetector();

  context.subscriptions.push(activeConnection);
  context.subscriptions.push(autoComplete);
  context.subscriptions.push(checkpointDetector);
  context.subscriptions.push(contractDetector);
  context.subscriptions.push(EpicChainExpressDetector);
  context.subscriptions.push(EpicChainExpressInstanceManager);
  context.subscriptions.push(serverListDetector);
  context.subscriptions.push(walletDetector);

  context.subscriptions.push(
    vscode.window.registerTreeDataProvider(
      "epicchain-graphical-wizard.views.blockchains",
      blockchainsTreeDataProvider
    )
  );

  context.subscriptions.push(
    vscode.window.registerTreeDataProvider(
      "epicchain-graphical-wizard.views.contracts",
      contractsTreeDataProvider
    )
  );

  context.subscriptions.push(
    vscode.window.registerTreeDataProvider(
      "epicchain-graphical-wizard.views.wallets",
      walletsTreeDataProvider
    )
  );

  context.subscriptions.push(
    vscode.window.registerCustomEditorProvider(
      "epicchain-graphical-wizard.epicchain.neo-invoke-json",
      EpicChainInvokeFileEditorProvider
    )
  );

  context.subscriptions.push(
    vscode.window.registerWebviewViewProvider(
      "epicchain-graphical-wizard.views.quickStart",
      new QuickStartViewProvider(
        context,
        blockchainsTreeDataProvider,
        EpicChainExpressInstanceManager,
        contractDetector,
        activeConnection,
        walletDetector
      )
    )
  );

  registerCommand(context, "epicchain-graphical-wizard.express.create", () =>
    EpicChainExpressCommands.create(
      context,
      EpicChainExpress,
      EpicChainExpressInstanceManager,
      autoComplete,
      blockchainMonitorPool,
      blockchainsTreeDataProvider
    )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.epicchain.newContract",
    () => Templates.newContract(context)
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.epicchain.walletCreate",
    () => NeoCommands.createWallet()
  );

  registerCommand(context, "epicchain-graphical-wizard.connect", () =>
    activeConnection.connect()
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.customizeServerList",
    () => serverListDetector.customize()
  );

  registerCommand(context, "epicchain-graphical-wizard.disconnect", () =>
    activeConnection.disconnect()
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.contractDeploy",
    (commandArguments) =>
      EpicChainExpressCommands.contractDeploy(
        EpicChainExpress,
        contractDetector,
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.customCommand",
    (commandArguments) =>
      EpicChainExpressCommands.customCommand(
        EpicChainExpress,
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.createCheckpoint",
    (commandArguments) =>
      EpicChainExpressCommands.createCheckpoint(
        EpicChainExpress,
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.exploreStorage",
    (commandArguments) =>
      EpicChainExpressCommands.exploreStorage(
        context,
        autoComplete,
        blockchainMonitorPool,
        blockchainsTreeDataProvider,
        EpicChainExpress,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.reset",
    (commandArguments) =>
      EpicChainExpressCommands.reset(
        EpicChainExpress,
        EpicChainExpressInstanceManager,
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.restoreCheckpoint",
    (commandArguments) =>
      EpicChainExpressCommands.restoreCheckpoint(
        EpicChainExpress,
        blockchainsTreeDataProvider,
        checkpointDetector,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.run",
    (commandArguments) =>
      EpicChainExpressInstanceManager.run(
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.runAdvanced",
    (commandArguments) =>
      EpicChainExpressInstanceManager.runAdvanced(
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.stop",
    (commandArguments) =>
      EpicChainExpressInstanceManager.stop(
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.transfer",
    (commandArguments) =>
      EpicChainExpressCommands.transfer(
        EpicChainExpress,
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.express.walletCreate",
    (commandArguments) =>
      EpicChainExpressCommands.walletCreate(
        EpicChainExpress,
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.epicchain.contractDeploy",
    (commandArguments) =>
      NeoCommands.contractDeploy(
        contractDetector,
        walletDetector,
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.epicchain.invokeContract",
    (commandArguments) =>
      NeoCommands.invokeContract(
        activeConnection,
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.tracker.openTracker",
    (commandArguments) =>
      TrackerCommands.openTracker(
        context,
        autoComplete,
        blockchainMonitorPool,
        blockchainsTreeDataProvider,
        commandArguments
      )
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.tracker.openContract",
    (commandArguments) =>
      TrackerCommands.openContract(context, autoComplete, commandArguments)
  );

  registerCommand(
    context,
    "epicchain-graphical-wizard.tracker.openWallet",
    (commandArguments) =>
      TrackerCommands.openWallet(
        context,
        autoComplete,
        commandArguments,
        activeConnection
      )
  );
}

export function deactivate() {
  Log.log(LOG_PREFIX, "Deactivating extension...");
  Log.close();
}
