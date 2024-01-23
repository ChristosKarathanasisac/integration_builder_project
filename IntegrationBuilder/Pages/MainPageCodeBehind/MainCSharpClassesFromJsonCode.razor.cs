using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamasoft.JsonClassGenerator;

namespace IntegrationBuilder.Pages
{
    public partial class Main
    {

        private string _inputJson = "";
        private string _cSharpClasses = "";
        private string _savedCSharpClasses;

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

       

        async Task BtnSaveCSharpClasses()
        {
            this._infomsgs = "";
            if (string.IsNullOrEmpty(this._cSharpClasses))
            {
                this._infomsgs = "C# Classes box is empty!";
                return;
            }
            this._savedCSharpClasses = this._cSharpClasses;
        }

        async Task BtnShowCSharpClasses()
        {
            this._infomsgs = "";
            if (string.IsNullOrEmpty(this._savedCSharpClasses))
            {

                this._infomsgs = "There are no saved C# classes!";
                return;
            }
            await ShowCSharpClasses();
        }

        async Task BtnClearSavedClasses()
        {
            this._savedCSharpClasses = "";
            InvokeAsync(StateHasChanged);
            DialogService.Refresh();
            //this.StateHasChanged();
        }
    }
}