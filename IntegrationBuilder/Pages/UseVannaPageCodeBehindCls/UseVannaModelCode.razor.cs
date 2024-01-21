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
        //All code related to Vanna AI Model
        //Parameters
        //---------------------------------
        private VannaModel _vannaModel;
        //---------------------------------
        //Functions
        //---------------------------------
        async Task CreateOrSetModel(VannaModel arg)
        {
            //Code Only For Windows Authentication
            this._infomsgs = "";
            try
            {
                this._isLoadingConnectToVanna = true;
                this.StateHasChanged();
                await Task.Run(async () =>
                {
                    StringBuilder urlBuilder = new StringBuilder();
                    urlBuilder.Append(configuration["ApplicationInfo:vannaMainURL"].ToString());
                    urlBuilder.Append("/");
                    _vannaModel.ModelName = _vannaModel.ModelName.ToLower();
                    if (this._vannaModel.NewModel)
                    {
                        urlBuilder.Append(configuration["ApplicationInfo:createModelService"].ToString());
                        string error = "";
                        if (_vannaUtilitiesService.CheckIfVannaModelExistOrCreated(urlBuilder.ToString(), this._vannaModel.ModelName.ToLower(), out error))
                        {
                            _infomsgs = $"The model {this._vannaModel.ModelName} Created. You can use it!";
                            if (GetAllInitialDataForStep3())
                            {
                                _infomsgs += "\n" + "Initial data for Step 3 are set";
                                this._DisabledStep3 = false;
                                this._DisabledStep4 = false;
                            }
                        }
                        else
                        {
                            this._DisabledStep3 = true;
                            this._DisabledStep4 = true;
                            _infomsgs = $"Something went wrong. Error message: {error}. If the name exists try amother name." + "If you want to use an existing model deselect the Create Model option and try again!";
                        }
                    }
                    else
                    {
                        urlBuilder.Append(configuration["ApplicationInfo:checkIfModelExistService"].ToString());
                        string error = "";
                        //Vanna saves the model names with lower characters
                        if (_vannaUtilitiesService.CheckIfVannaModelExistOrCreated(urlBuilder.ToString(), this._vannaModel.ModelName.ToLower(), out error))
                        {
                            _infomsgs = $"The model {this._vannaModel.ModelName} Exist. You can use it!";
                            if (GetAllInitialDataForStep3())
                            {
                                _infomsgs += "\n" + "Initial data for Step 3 are set";
                                this._DisabledStep3 = false;
                                this._DisabledStep4 = false;
                            }
                        }
                        else
                        {
                            this._DisabledStep3 = true;
                            this._DisabledStep4 = true;
                            _infomsgs = $"Something went wrong. Error message: {error}";
                        }
                    }
                });
                this._isLoadingConnectToVanna = false;
                this.StateHasChanged();
            }
            catch (Exception exc)
            {
                this._infomsgs = $"Exception in CreateOrSetModel. Exception message: {exc.Message}";
            }
        }
    }
}