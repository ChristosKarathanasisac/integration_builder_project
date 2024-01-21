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
using System.Text;
using IntegrationBuilder.HuggingChatUtilities;

namespace IntegrationBuilder.Pages
{
    public partial class CreateMapping
    {
        private HuggingChatUtilitiesService huggingChatUtilitiesService;
        private string _query;
        private string _objClassStr;
        private string _mappingDesc;
        private string _resultClass;
        private string _infomsgs;
        protected override async Task OnInitializedAsync()
        {
            this.huggingChatUtilitiesService = new HuggingChatUtilitiesService();
        }

        async Task BtnCreateMapping()
        {
            this._infomsgs = "";
            if (string.IsNullOrEmpty(_query))
            {
                this._infomsgs = "Query field is empty!";
                return;
            }

            if (string.IsNullOrEmpty(_objClassStr))
            {
                this._infomsgs = "Object class field is empty!";
                return;
            }

            if (string.IsNullOrEmpty(_mappingDesc))
            {
                this._infomsgs = "Mapping Description field is empty!";
                return;
            }

            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(configuration["ApplicationInfo:HuggingChatMainURL"].ToString());
            urlBuilder.Append("/");
            urlBuilder.Append(configuration["ApplicationInfo:useHaggingChat"].ToString());
            string resp = "";
            string error = "";
            string instructions = @"Create a method that executes the given query on a SQL Server database.
                                It should store the returned records in a DataTable, then create an object of the root class based on this data.
                                Subsequently, convert this object to JSON using the Newtonsoft library and execute a POST request to a specified URL.
                                Do the entire mapping. Give me only the method with the mapping";
            string huggingChatInput = this._query + '\n' + this._objClassStr + '\n' + this._mappingDesc + '\n' + instructions;
            string conversation_id = "";
            resp = this.huggingChatUtilitiesService.UseHaggingChat(urlBuilder.ToString(), huggingChatInput, true, out conversation_id, out error, "-999");
            if (string.IsNullOrEmpty(resp))
            {
                this._infomsgs = $"Something went wrong in Use Hugging Chat. Error message: {error}";
                return;
            }

            this._resultClass = resp;
        }
    }
}