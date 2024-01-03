using IntegrationBuilder.VannaAIUtilities;
using Newtonsoft.Json;

namespace IntegrationBuilder.HuggingChatUtilities
{
    public class HuggingChatUtilitiesService
    {
        public string UseHaggingChat(string url, string question, bool isNewModel,out string created_id, out string error,string conversation_id) 
        {
            try
            {
                created_id = "";
                error = "";
                RequestUseHaggingChat req = new RequestUseHaggingChat();
                req.newChat = isNewModel;
                req.inpout = question;
                req.conversation_id = conversation_id;

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
                    created_id = oResponse.conversation_id;
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
                created_id = "-999";
                error = $"Exception in UseHaggingChat. Exception message:{exc.Message}";
                return "";
            }
        }
    }
}
