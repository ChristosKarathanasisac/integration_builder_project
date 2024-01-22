using IntegrationBuilder.SQLServerUtilities;

namespace IntegrationBuilder.Pages
{
    public partial class Main
    {
        //All code related to SqlServer Connection
        //Parameters
        //---------------------------------
        private Credentials _sqlServerCredentials;
        //---------------------------------
        //Functions
        //---------------------------------
        private void CheckConnection(Credentials arg)
        {
            //Code Only For Windows Authentication
            this._infomsgs = "";
            try
            {
                string error = "";
                bool connSuccess = SQLServerLib.CheckConnection(this._sqlServerCredentials.Server.Trim(), this._sqlServerCredentials.Database.Trim(), out error);
                if (connSuccess)
                {
                    this._infomsgs = "Connect to database Success!";
                    this._DisabledStep2 = false;
                }
                else
                {
                    this._infomsgs = $"Connect to database Failed. Error message: {error}";
                }
            }
            catch (Exception exc)
            {
                this._infomsgs = $"Exception in CheckConnection. Exception message: {exc.Message}";
            }
        }
    }
}