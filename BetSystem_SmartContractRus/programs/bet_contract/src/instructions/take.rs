use anchor_lang::{prelude::*, system_program::{transfer, Transfer}};
use crate::state::Config;
use crate::errors::BetContractError;
use crate::instructions::initialize::LAMPORTS_PER_SOL;



#[derive(Accounts)]
pub struct Take<'info> {
    /// CHECK: This is not dangerous because we don't read or write from this account
    pub initializer: AccountInfo<'info>,
    #[account(mut)]
    pub taker: Signer<'info>,
    #[account(
        seeds = [b"config", initializer.key().as_ref(), b"bet_raceonlife"],
        bump = config.config_bump,
    )]
    pub config: Account<'info, Config>,
    #[account(
        mut,
        seeds = [
                    b"vault", 
                    initializer.key().as_ref(), 
                    config.key().as_ref(),
                    b"bet_raceonlife"
                ],
        bump = config.vault_bump,
    )]
    pub vault: SystemAccount<'info>,
    pub system_program: Program<'info, System>,
}   

impl<'info> Take<'info> {
    pub fn take_bet(&mut self, rating: u64) -> Result<()> {

        require!(!self.config.has_been_taken, BetContractError::BetAlreadyTaken);
        require!(self.taker.get_lamports() >= self.config.amount_of_bet_in_sol * LAMPORTS_PER_SOL, BetContractError::NotEnoughFunds);

        self.config.has_been_taken = true;
        self.config.pubkey_taker = Some(self.taker.key());
        self.config.rating_taker = Some(rating);
        
        let cpi_program = self.system_program.to_account_info();
        let cpi_accounts = Transfer {
            from: self.initializer.to_account_info(),
            to: self.vault.to_account_info(),
        };

        let cpi_ctx = CpiContext::new(cpi_program, cpi_accounts);

        transfer(cpi_ctx, self.config.amount_of_bet_in_sol * LAMPORTS_PER_SOL)
    }
}