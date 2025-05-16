use anchor_lang::{prelude::*, system_program::{transfer, Transfer}};
use crate::state::config::Config;
use crate::errors::BetContractError;

pub const LAMPORTS_PER_SOL: u64 = 1000000000;


#[derive(Accounts)]
pub struct Initialize<'info> {
    #[account(mut)]
    pub initializer: Signer<'info>,
    #[account(
        init,
        payer = initializer,
        space = Config::INIT_SPACE + 8,
        seeds = [b"config", initializer.key().as_ref(), b"bet_raceonlife"],
        bump
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
        bump
    )]
    pub vault: SystemAccount<'info>,
    pub system_program: Program<'info, System>,
}

impl<'info> Initialize<'info> {
    pub fn initialize_bet_with_initializer(&mut self, bumps: &InitializeBumps, amount_of_bet_in_sol: u64, rating: u64) -> Result<()> {

        require!(self.initializer.get_lamports() >= self.config.amount_of_bet_in_sol * LAMPORTS_PER_SOL, BetContractError::NotEnoughFunds);

        self.config.set_inner(Config {
            pubkey_initializer: self.initializer.key(),
            rating_initializer: rating,
            config_bump: bumps.config,
            vault_bump: bumps.vault,
            pubkey_taker: None,
            rating_taker: None,
            winner: None,
            has_been_taken: false,
            amount_of_bet_in_sol,
        });

        let cpi_program = self.system_program.to_account_info();
        let cpi_accounts = Transfer {
            from: self.initializer.to_account_info(),
            to: self.vault.to_account_info(),
        };

        let cpi_ctx = CpiContext::new(cpi_program, cpi_accounts);

        transfer(cpi_ctx, amount_of_bet_in_sol * LAMPORTS_PER_SOL)
    }
}