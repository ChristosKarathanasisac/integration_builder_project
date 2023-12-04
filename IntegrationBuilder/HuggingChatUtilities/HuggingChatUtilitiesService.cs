using IntegrationBuilder.VannaAIUtilities;
using Newtonsoft.Json;

namespace IntegrationBuilder.HuggingChatUtilities
{
    public class HuggingChatUtilitiesService
    {
        public string UseHaggingChat(string url, string question, bool isNewModel, out string error) 
        {
            try 
            {
                error = "";
                RequestUseHaggingChat req = new RequestUseHaggingChat();
                req.newChat = isNewModel;
                req.inpout = question;

                string jsonData = JsonConvert.SerializeObject(req, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });


                string sresult = "";
                string sentRequetsError = "";
                if (!GeneralUtilities.GeneralUtilities.SendRequest(jsonData, url, out sentRequetsError, out sresult))
                {
                    error = "Error in UseHaggingChat (SendRequest). Error message: " + sentRequetsError;
                    return "";
                }

                ResponseUseHaggingChat oResponse = new ResponseUseHaggingChat();
                oResponse = JsonConvert.DeserializeObject<ResponseUseHaggingChat>(sresult);

                if (oResponse.success == true)
                {
                    return oResponse.data;
                }
                else
                {
                    error = oResponse.error;
                    return "";
                }
            }
            catch (Exception exc) 
            {
                error = $"Exception in UseHaggingChat. Exception message:{exc.Message}";
                return "";
            }
        }
    }
}
