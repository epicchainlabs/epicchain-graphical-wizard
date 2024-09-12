import * as vscode from "vscode";

import ActiveConnection from "../activeConnection";
import BlockchainsTreeDataProvider from "./blockchainsTreeDataProvider";
import ContractDetector from "../fileDetectors/contractDetector";
import EpicChainExpressInstanceManager from "../neoExpress/EpicChainExpressInstanceManager";
import QuickStartPanelController from "../panelControllers/quickStartPanelController";
import WalletDetector from "../fileDetectors/walletDetector";

export default class QuickStartViewProvider
  implements vscode.WebviewViewProvider
{
  constructor(
    private readonly context: vscode.ExtensionContext,
    private readonly blockchainsTreeDataProvider: BlockchainsTreeDataProvider,
    private readonly EpicChainExpressInstanceManager: EpicChainExpressInstanceManager,
    private readonly contractDetector: ContractDetector,
    private readonly activeConnection: ActiveConnection,
    private readonly walletDetector: WalletDetector
  ) {}

  resolveWebviewView(webviewView: vscode.WebviewView) {
    webviewView.webview.options = { enableScripts: true };
    new QuickStartPanelController(
      this.context,
      webviewView,
      this.blockchainsTreeDataProvider,
      this.EpicChainExpressInstanceManager,
      this.contractDetector,
      this.activeConnection,
      this.walletDetector
    );
  }
}
