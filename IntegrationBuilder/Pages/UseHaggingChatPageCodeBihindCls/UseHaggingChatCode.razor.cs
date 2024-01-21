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
    public partial class UseHaggingChat
    {
        private string _userQuestion;
        private string _huggingResponse;
        private string _infomsgs;
        private bool _isNewChat;
        private string _conversation_id;
        private bool _isLoading;
        private HuggingChatUtilitiesService huggingChatUtilitiesService;
        protected override async Task OnInitializedAsync()
        {
            this._isNewChat = true;
            this.huggingChatUtilitiesService = new HuggingChatUtilitiesService();
            this._conversation_id = "";
            _isLoading = false;
        }

        async Task BtnAskHuggingChat()
        {
            try
            {
                this._isLoading = true;
                this.StateHasChanged();
                this._huggingResponse = "";
                this._infomsgs = "";
                string error = "";
                string id = "";
                await Task.Run(async () =>
                {
                    if (string.IsNullOrEmpty(this._userQuestion))
                    {
                        this._infomsgs = "Insert a Question!";
                        return;
                    }

                    StringBuilder urlBuilder = new StringBuilder();
                    urlBuilder.Append(configuration["ApplicationInfo:HuggingChatMainURL"].ToString());
                    urlBuilder.Append("/");
                    urlBuilder.Append(configuration["ApplicationInfo:useHaggingChat"].ToString());
                    this._huggingResponse = this.huggingChatUtilitiesService.UseHaggingChat(urlBuilder.ToString(), this._userQuestion, this._isNewChat, out id, out error, _conversation_id);
                });
                if (string.IsNullOrEmpty(this._huggingResponse))
                {
                    this._infomsgs = $"Something went wrong in Use Hugging Chat. Error message: {error}";
                }

                this._isLoading = false;
                this.StateHasChanged();
            }
            catch (Exception exc)
            {
                _infomsgs = $"Exception in BtnAskHuggingChat {exc.Message}.";
            }
        }
    }
}