using IntegrationBuilder.VannaAIUtilities;
using System.Text;

namespace IntegrationBuilder.Pages
{
    public partial class Main
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
                this._loadignBarValue = 100;
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
                this._loadignBarValue = 0;
                this.StateHasChanged();
            }
            catch (Exception exc)
            {
                this._loadignBarValue = 0;
                this._infomsgs = $"Exception in CreateOrSetModel. Exception message: {exc.Message}";
            }
        }
    }
}