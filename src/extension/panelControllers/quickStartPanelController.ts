import * as vscode from "vscode";

import ActiveConnection from "../activeConnection";
import BlockchainsTreeDataProvider from "../vscodeProviders/blockchainsTreeDataProvider";
import ContractDetector from "../fileDetectors/contractDetector";
import EpicChainExpressInstanceManager from "../EpicChainExpress/EpicChainExpressInstanceManager";
import PanelControllerBase from "./panelControllerBase";
import QuickStartViewRequest from "../../shared/messages/quickStartViewRequest";
import QuickStartViewState from "../../shared/viewState/quickStartViewState";
import WalletDetector from "../fileDetectors/walletDetector";

const LOG_PREFIX = "QuickStartPanelController";

export default class QuickStartPanelController extends PanelControllerBase<
  QuickStartViewState,
  QuickStartViewRequest
> {
  constructor(
    context: vscode.ExtensionContext,
    panel: vscode.WebviewView,
    private readonly blockchainsTreeDataProvider: BlockchainsTreeDataProvider,
    private readonly EpicChainExpressInstanceManager: EpicChainExpressInstanceManager,
    private readonly contractDetector: ContractDetector,
    private readonly activeConnection: ActiveConnection,
    private readonly walletDetector: WalletDetector
  ) {
    super(
      {
        view: "quickStart",
        panelTitle: "",
        connectionName: null,
        hasContracts: false,
        hasDeployedContract: false,
        hasEpicChainExpressInstance: false,
        hasWallets: false,
        neoDeploymentRequired: false,
        EpicChainExpressDeploymentRequired: false,
        EpicChainExpressIsRunning: false,
        workspaceIsOpen: false,
      },
      context,
      panel
    );
    vscode.workspace.onDidChangeWorkspaceFolders(() => this.refresh());
    this.blockchainsTreeDataProvider.onDidChangeTreeData(() => this.refresh());
    this.EpicChainExpressInstanceManager.onChange(() => this.refresh());
    this.contractDetector.onChange(() => this.refresh());
    this.activeConnection.onChange(() => this.refresh());
    this.walletDetector.onChange(() => this.refresh());
    this.refresh();
  }

  onClose() {}

  private async refresh() {
    const connectionName =
      this.activeConnection.connection?.blockchainIdentifier.friendlyName ||
      null;

    let neoDeploymentRequired = false;
    let EpicChainExpressDeploymentRequired = false;
    const deploymentRequired =
      Object.values(this.contractDetector.contracts).filter(
        (_) => _.deploymentRequired
      ).length > 0;
    if (deploymentRequired) {
      if (
        this.activeConnection.connection?.blockchainIdentifier
          .blockchainType === "express"
      ) {
        EpicChainExpressDeploymentRequired = true;
      } else {
        neoDeploymentRequired = true;
      }
    }

    const hasContracts =
      Object.keys(this.contractDetector.contracts).length > 0;

    const hasDeployedContract =
      Object.values(this.contractDetector.contracts).filter((_) => _.deployed)
        .length > 0;

    const hasEpicChainExpressInstance =
      this.blockchainsTreeDataProvider
        .getChildren()
        .filter((_) => _.blockchainType === "express").length > 0;

    const hasWallets = this.walletDetector.wallets.length > 0;

    const EpicChainExpressIsRunning =
      this.EpicChainExpressInstanceManager.runningInstance?.blockchainType ===
      "express";

    const workspaceIsOpen = !!vscode.workspace.workspaceFolders?.length;

    await this.updateViewState({
      connectionName,
      hasContracts,
      hasDeployedContract,
      hasEpicChainExpressInstance,
      hasWallets,
      neoDeploymentRequired,
      EpicChainExpressDeploymentRequired,
      EpicChainExpressIsRunning,
      workspaceIsOpen,
    });
  }

  protected async onRequest(request: QuickStartViewRequest) {
    if (request.command) {
      await vscode.commands.executeCommand(request.command);
    }
  }
}
