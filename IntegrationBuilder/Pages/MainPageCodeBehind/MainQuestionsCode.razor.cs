using Radzen;
using System.Text;

namespace IntegrationBuilder.Pages
{
    public partial class Main
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

        async Task BtnSaveQuery()
        {
            this._infomsgs = "";
            if (string.IsNullOrEmpty(this._vannaResponse))
            {

                this._infomsgs = "Vanna response text box is empty!";
                return;
            }
            this._savedQuery = this._vannaResponse.ToString();
        }

        async Task BtnShowSavedQuery()
        {
            this._infomsgs = "";
            if (string.IsNullOrEmpty(this._vannaResponse))
            {

                this._infomsgs = "Vanna response text box is empty!";
                return;
            }
            await ShowQueryPopup();
        }

        async Task BtnClearSavedQuery()
        {
            this._savedQuery = "";
            InvokeAsync(StateHasChanged);
            DialogService.Refresh();
            //this.StateHasChanged();
        }
    }
}