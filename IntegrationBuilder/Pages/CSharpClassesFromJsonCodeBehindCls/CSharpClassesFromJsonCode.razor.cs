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
using JsonToCSharpClasses;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Xamasoft.JsonClassGenerator;
using IntegrationBuilder.HuggingChatUtilities;

namespace IntegrationBuilder.Pages
{
    public partial class CSharpClassesFromJson
    {
        private string _inputJson = "";
        private string _cSharpClasses = "";
        private string _infomsgs = "";
        private int _loadignBarValue;
        protected override async Task OnInitializedAsync()
        {
            this._loadignBarValue = 0;
        }

        async Task GetClasses()
        {
            try
            {
                string error = "";
                if (!IsValidJson(this._inputJson.Trim(), out error))
                {
                    this._infomsgs = error;
                    return;
                }

                this._loadignBarValue = 100;
                await Task.Run(async () =>
                {

                });
                Xamasoft.JsonClassGenerator.JsonClassGenerator jsonClassGenerator = new JsonClassGenerator();
                string srtClasses = jsonClassGenerator.GenerateClasses(this._inputJson.Trim(), out error).ToString();
                if (string.IsNullOrEmpty(srtClasses))
                {
                    this._infomsgs = $"Something went wrong. No classes created! Error message:{error}";
                }
                else
                {
                    this._cSharpClasses = srtClasses;
                }
                this._loadignBarValue = 0;
            }
            catch (Exception exc)
            {
                this._loadignBarValue = 0;
                this._infomsgs = $"Exception in GetClasses. Exception message:{exc.Message}";
            }
        }

        private bool IsValidJson(string strInput, out string err)
        {
            err = "";
            if (string.IsNullOrWhiteSpace(strInput))
            {
                err = "Json Input is empty.";
                return false;
            }

            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
 (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    err = jex.Message;
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    err = ex.ToString();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}