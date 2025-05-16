use anchor_lang::prelude::*;
use crate::{errors::BetContractError, state::Config};

#[derive(Accounts)]
pub struct Winner<'info> {
    #[account(mut)]
    pub initializer: Signer<'info>,
    /// Don't check
    #[account(mut)]
    pub taker: AccountInfo<'info>,
    // /// Don't check
    // #[account(mut)]
    // pub winner: AccountInfo<'info>,

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
    pub system_program: Program<'info, System>
}

impl <'info> Winner<'info> {
    pub fn winner(&mut self, winner: Pubkey) -> Result<()> {
        require!(self.config.has_been_taken, BetContractError::BetAlreadyTaken);
        require!(self.config.pubkey_initializer == self.initializer.key(), BetContractError::NotTheCorrectsWinner);
        require!(self.config.pubkey_taker.unwrap() == self.taker.key(), BetContractError::NotTheCorrectsWinner);
        require!(winner == self.config.pubkey_taker.unwrap() || winner == self.config.pubkey_initializer, BetContractError::NotTheCorrectsWinner);

        if winner == self.config.pubkey_taker.unwrap() {
            self.config.winner = Some(self.taker.key());
        } else {
            self.config.winner = Some(self.initializer.key());
        }

        let cpi_program = self.system_program.to_account_info();
        let cpi_accounts = Transfer {
            from: self.vault.to_account_info(),
            to: self.config.winner.unwrap().to_account_info(),
        }

        Ok(())
    }
}