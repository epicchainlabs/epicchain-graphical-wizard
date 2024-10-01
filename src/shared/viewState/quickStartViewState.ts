type QuickStartViewState = {
  view: "quickStart";
  panelTitle: "";
  connectionName: string | null;
  hasContracts: boolean;
  hasDeployedContract: boolean;
  hasEpicChainExpressInstance: boolean;
  hasWallets: boolean;
  neoDeploymentRequired: boolean;
  EpicChainExpressDeploymentRequired: boolean;
  EpicChainExpressIsRunning: boolean;
  workspaceIsOpen: boolean;
};

export default QuickStartViewState;
