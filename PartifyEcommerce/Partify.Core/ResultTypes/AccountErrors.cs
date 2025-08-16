namespace CSOS.Core.ResultTypes
{
    public static class AccountErrors
    {
        public static readonly Error UserIsNotLoggedIn = new Error(
          "User.AccountNotFound", "Log in first to access this content");

        public static readonly Error AccountNotFound = new Error(
            "User.AccountNotFound", "Account of given id not found");

        public static readonly Error WrongPassword = new Error(
            "User.WrongPassword", "You've given wrong password");

        public static readonly Error PasswordsDoestMatch = new Error(
           "User.PasswordsDoestMatch", "New and Confirmation passwords doesnt match");

        public static readonly Error PasswordChangeFailed = new Error(
           "User.PasswordChangeFailed", "Failed to change passwords");
        
        public static readonly Error MissingAccountUpdateRequest = new Error (
            "MissingAccountUpdateRequest", "User's account update request not found");
        
        public static readonly Error MissingPasswordChangeRequest = new Error (
            "MissingPasswordChangeRequest", "User's password request not found");

    }
}
