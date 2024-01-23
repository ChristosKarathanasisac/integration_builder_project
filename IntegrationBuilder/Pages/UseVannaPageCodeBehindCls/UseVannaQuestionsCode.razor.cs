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
        //All code related to Vanna Questions
        //Parameters
        private string _userQuestion = "";
        private string _vannaResponse = "";
        //---------------------------------
        //Functions
        //---------------------------------
        async Task BtnAskVanna()
        {
            try
            {
                this._infomsgs = "";
                this._vannaResponse = "";
                if (string.IsNullOrEmpty(this._userQuestion))
                {
                    this._infomsgs = "Insert a Question!";
                    return;
                }

                this._loadignBarValue = 100;
                this.StateHasChanged();
                await Task.Run(async () =>
                {
                    StringBuilder urlBuilder = new StringBuilder();
                    urlBuilder.Append(configuration["ApplicationInfo:vannaMainURL"].ToString());
                    urlBuilder.Append("/");
                    urlBuilder.Append(configuration["ApplicationInfo:generateSQL"].ToString());
                    string error = "";
                    this._vannaResponse = this._vannaUtilitiesService.GenerateSQL(urlBuilder.ToString(), this._userQuestion, this._vannaModel.ModelName.ToLower(), out error);
                    if (string.IsNullOrEmpty(this._vannaResponse))
                    {
                        this._infomsgs = $"Something went wrong in Vanna Generate SQL. Error message: {error}";
                        this._loadignBarValue = 0;
                        return;
                    }
                });
                this._loadignBarValue = 0;
                this.StateHasChanged();
            }
            catch (Exception exc)
            {
                _infomsgs = $"Exception in BtnAskVanna {exc.Message}.";
                this._loadignBarValue = 0;
            }
        }
    }
}