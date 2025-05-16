import * as anchor from "@coral-xyz/anchor";
import { Program } from "@coral-xyz/anchor";
import { BetContract } from "../target/types/bet_contract";
import { BN } from "bn.js";
import wallet from "../test-keypair.json";
import wallet2 from "../test-keypair2.json";

import {
  Keypair,
  Connection,
  Commitment,
} from "@solana/web3.js";

// const INITIALIZER_KEYPAIR = anchor.web3.Keypair.fromSecretKey(wallet);
const INITIALIZER_KEYPAIR = Keypair.fromSecretKey(new Uint8Array(wallet));
// const keypair = Keypair.fromSecretKey(new Uint8Array(INITIALIZER_KEYPAIR.secretKey));
const TAKER_KEYPAIR = Keypair.fromSecretKey(new Uint8Array(wallet2));

const initial_wallet = anchor.Wallet.local();

const commitment: Commitment = "confirmed";
const connection = new Connection("http://127.0.0.1:8899", commitment);

const provider = anchor.AnchorProvider.env();


describe("bet_contract", () => {
  // Configure the client to use the local cluster.
  anchor.setProvider(anchor.AnchorProvider.env());
  const program = anchor.workspace.BetContract as Program<BetContract>;

  it("Is initialized!", async () => {
    console.log(INITIALIZER_KEYPAIR.publicKey);
    
    // await connection.requestAirdrop(INITIALIZER_KEYPAIR.publicKey, 100_000_000_000);
    const tx = await program.methods.initialize(new BN(1), new BN(1)).accountsPartial(
      {
        initializer: INITIALIZER_KEYPAIR.publicKey,
      }
    ).signers([INITIALIZER_KEYPAIR])
    .rpc();
    // 
    console.log("Your transaction signature", tx);
  });

  it("the bet is taken!", async () => {
    
    const tx = await program.methods.takeBet(new BN(1)).accountsPartial(
      {
        initializer: INITIALIZER_KEYPAIR.publicKey,
        taker: TAKER_KEYPAIR.publicKey,
      }
    ).signers([TAKER_KEYPAIR])
    .rpc();
    // 
    console.log("Your transaction signature", tx);
  });
});
