use anchor_lang::prelude::*;

#[account]
#[derive(InitSpace)]
pub struct Config {
    pub pubkey_initializer: Pubkey,
    pub rating_initializer: u64,
    pub pubkey_taker: Option<Pubkey>,
    pub rating_taker:Option<u64>,
    pub config_bump: u8,
    pub vault_bump: u8,
    pub winner: Option<Pubkey>,
    pub has_been_taken: bool,
    pub amount_of_bet_in_sol: u64,
}