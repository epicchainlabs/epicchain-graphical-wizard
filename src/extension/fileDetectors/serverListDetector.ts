import * as fs from "fs";
import * as neonCore from "@cityofzion/neon-core";
import * as vscode from "vscode";

import BlockchainIdentifier from "../blockchainIdentifier";
import DetectorBase from "./detectorBase";
import IoHelpers from "../util/ioHelpers";
import JSONC from "../util/JSONC";
import Log from "../util/log";
import posixPath from "../util/posixPath";

const LOG_PREFIX = "ServerListDetector";

const SEARCH_PATTERN = "**/epicchain-servers.json";

const DEFAULT_FILE = {
  "neo-rpc-uris": ["http://localhost:80111"],
  "neo-blockchain-names": {
    "0x0000000000000000000000000000000000000000000000000000000000000000":
      "My Private Blockchain",
  },
};

const UNKNOWN_BLOCKCHAIN =
  "0x0000000000000000000000000000000000000000000000000000000000000000";

// These are the genesis block hashes of some well-known blockchains.
// We identify which blockchain an RPC URL corresponds to by requesting
// block 0 and then comparing the hash to the list below (and any other
// names the user has supplied through epicchain-servers.json file(s) in the
// current workspace):
const WELL_KNOWN_BLOCKCHAINS: { [genesisHash: string]: string } = {
  "0x1f4d1defa46faa5e7b9b8d3f79a06bec777d7c26c4aa5f6f5899a291daa87c15":
    "EpicChain Graphical Wizard MainNet",
  "0x9d3276785e7306daf59a3f3b9e31912c095598bbfb8a4476b821b0e59be4c57a":
    "EpicChain Graphical Wizard TestNet",
};

// These are the RPC URLs made available to users who do not have their
// own epicchain-servers.json file(s) in their workspace:
const SEED_URLS: { [url: string]: boolean } = {
  //
  // TODO: Add alternative TestNet endpoints.
  //
  // V3 MainNet:
  "http://mainnet1-seed.epic-chain.org:10111": true,
  "http://mainnet2-seed.epic-chain.org:10111": true,
  "http://mainnet3-seed.epic-chain.org:10111": true,
  "http://mainnet4-seed.epic-chain.org:10111": true,
  "http://mainnet5-seed.epic-chain.org:10111": true,
  // V3 TestNet:
  "http://testnet1-seed.epic-chain.org:20111": true,
  "http://testnet2-seed.epic-chain.org:20111": true,
  "http://testnet3-seed.epic-chain.org:20111": true,
  "http://testnet4-seed.epic-chain.org:20111": true,
  "http://testnet5-seed.epic-chain.org:20111": true,
};

export default class ServerListDetector extends DetectorBase {
  private blockchainsSnapshot: BlockchainIdentifier[] = [];

  get blockchains(): BlockchainIdentifier[] {
    return [...this.blockchainsSnapshot];
  }

  constructor(private readonly extensionPath: string) {
    super(SEARCH_PATTERN);
  }

  async customize() {
    if (this.files.length === 1) {
      await vscode.window.showTextDocument(vscode.Uri.file(this.files[0]));
    } else if (this.files.length > 0) {
      const fileToEdit = await IoHelpers.multipleChoiceFiles(
        "There are multiple blockchain configurations in the current workspace, which would you like to edit?",
        ...this.files
      );
      await vscode.window.showTextDocument(vscode.Uri.file(fileToEdit));
    } else {
      const workspaceFolders = vscode.workspace.workspaceFolders?.map((_) =>
        posixPath(_.uri.fsPath)
      );
      if (!workspaceFolders?.length) {
        vscode.window.showErrorMessage(
          "Blockchain configuration is stored in a file in your active workspace. Please open a folder in VS Code before proceeding."
        );
      } else {
        let workspaceFolder = workspaceFolders[0];
        if (workspaceFolders.length > 0) {
          workspaceFolder = await IoHelpers.multipleChoiceFiles(
            "Please chose a location to store blockchain configuration.",
            ...workspaceFolders
          );
        }
        const fileToEdit = posixPath(workspaceFolder, "epicchain-servers.json");
        if (!fs.existsSync(fileToEdit)) {
          fs.writeFileSync(fileToEdit, JSONC.stringify(DEFAULT_FILE));
        }
        await vscode.window.showTextDocument(vscode.Uri.file(fileToEdit));
      }
    }
  }

  async processFiles() {
    const blockchainNames = { ...WELL_KNOWN_BLOCKCHAINS };
    const rpcUrls: { [url: string]: boolean } = { ...SEED_URLS };
    for (const file of this.files) {
      try {
        const contents = JSONC.parse(
          (await fs.promises.readFile(file)).toString()
        );
        const blockchainNamesThisFile = contents["neo-blockchain-names"];
        if (blockchainNamesThisFile) {
          for (let gensisBlockHash of Object.getOwnPropertyNames(
            blockchainNamesThisFile
          )) {
            gensisBlockHash = gensisBlockHash.toLowerCase().trim();
            const name = blockchainNamesThisFile[gensisBlockHash]?.trim();
            if (!blockchainNames[gensisBlockHash] && name) {
              blockchainNames[gensisBlockHash] = name;
            }
          }
        }
        const rpcUrlsThisFile = contents["neo-rpc-uris"];
        if (rpcUrlsThisFile && Array.isArray(rpcUrlsThisFile)) {
          for (const url of rpcUrlsThisFile) {
            try {
              const parsedUrl = vscode.Uri.parse(url, true);
              if (parsedUrl.scheme === "http" || parsedUrl.scheme === "https") {
                rpcUrls[url] = true;
              } else {
                Log.log(
                  LOG_PREFIX,
                  "Ignoring malformed URL (bad scheme)",
                  url,
                  file
                );
              }
            } catch (e) {
              Log.log(
                LOG_PREFIX,
                "Ignoring malformed URL (parse error)",
                url,
                file
              );
            }
          }
        }
      } catch (e) {
        Log.log(
          LOG_PREFIX,
          "Error parsing EpicChain Express config",
          file,
          e.message
        );
      }
    }
    const uniqueUrls = Object.getOwnPropertyNames(rpcUrls);
    const genesisBlockHashes = await Promise.all(
      uniqueUrls.map((_) => this.tryGetGenesisBlockHash(_))
    );
    const urlsByBlockchain: { [genesisHash: string]: string[] } = {};
    for (let i = 0; i < uniqueUrls.length; i++) {
      urlsByBlockchain[genesisBlockHashes[i]] =
        urlsByBlockchain[genesisBlockHashes[i]] || [];
      urlsByBlockchain[genesisBlockHashes[i]].push(uniqueUrls[i]);
    }
    const newBlockchainsSnapshot: BlockchainIdentifier[] = [];
    for (const genesisHash of Object.getOwnPropertyNames(urlsByBlockchain)) {
      const name = blockchainNames[genesisHash] || "Unknown Blockchain";
      const urls = urlsByBlockchain[genesisHash];
      const isWellKnown = !!WELL_KNOWN_BLOCKCHAINS[genesisHash];
      newBlockchainsSnapshot.push(
        BlockchainIdentifier.fromNameAndUrls(
          this.extensionPath,
          name,
          urls,
          isWellKnown
        )
      );
    }
    this.blockchainsSnapshot = newBlockchainsSnapshot;
  }

  private async tryGetGenesisBlockHash(rpcUrl: string) {
    try {
      const rpcClient = new neonCore.rpc.RPCClient(rpcUrl);
      const genesisBlock = await rpcClient.getBlock(0, true);
      return genesisBlock.hash;
    } catch (e) {
      Log.log(
        LOG_PREFIX,
        "Could not get genesis blockhash from",
        rpcUrl,
        e.message
      );
      return UNKNOWN_BLOCKCHAIN;
    }
  }
}
