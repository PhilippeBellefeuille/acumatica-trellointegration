using PX.Common;

namespace PX.TrelloIntegration
{
    [PXLocalizable(Messages.Prefix)]
    public static class Messages
    {
        // Add your messages here as follows (see line below):
        // public const string YourMessage = "Your message here.";
        #region Validation and Processing Messages
        public const string Prefix = "Trello Error";
        public const string ValidationDeleteChildren = "Are you sure you want to delete this mapping with all it's children ?";
        public const string NotLoggedIn = "You are not logged in to Trello";
        public const string UserNotInOrganization = "User is not a member of the selected organization";
        #endregion

        public const string NewBoard = "<NEW>";

    }
}
