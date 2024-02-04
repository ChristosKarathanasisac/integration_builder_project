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
        //All general code of the page
        //Parameters
        //---------------------------------
        private VannaUtilitiesService _vannaUtilitiesService;
        private string _infomsgs;
        private bool _DisabledStep2 = true;
        private bool _DisabledStep3 = true;
        private bool _DisabledStep4 = true;
        private int _loadignBarValue;
        //---------------------------------
        //Functions
        //---------------------------------
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _sqlServerCredentials = new Credentials();
            _vannaModel = new VannaModel();
            _vannaUtilitiesService = new VannaUtilitiesService();
            _tableDDLDocumentations = new List<DDLDocumentation>();
            _viewDDLDocumentations = new List<DDLDocumentation>();
            _sqlStatementForTrain = "";
            _documentationForTrain = "";
            this._loadignBarValue = 0;
        }

        // void OnChangeTab(int index)
        // {
        //     //I should Remove this
        //     //console.Log($"Tab with index {index} was selected.");
        // }
        private void OnChangeStep(int index)
        {
            this._infomsgs = String.Empty;
            if (index == 1)
            {
            }
            else if (index == 2)
            {
            }
        }
    //---------------------------------
    }
}