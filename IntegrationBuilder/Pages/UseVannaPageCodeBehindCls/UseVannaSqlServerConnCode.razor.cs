using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::System.Threading.Tasks;
using global::Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using IntegrationBuilder;
using IntegrationBuilder.Shared;
using Radzen;
using Radzen.Blazor;
using IntegrationBuilder.SQLServerUtilities;
using IntegrationBuilder.VannaAIUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace IntegrationBuilder.Pages
{
    public partial class UseVanna
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