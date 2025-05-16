use anchor_lang::error_code;

#[error_code]
pub enum BetContractError {
    #[msg("The bet has already been taken")]
    BetAlreadyTaken,
    #[msg("There is not enough funds")]
    NotEnoughFunds,
    #[msg("Another wallet address was inserted")]
    NotTheCorrectsWinner
}