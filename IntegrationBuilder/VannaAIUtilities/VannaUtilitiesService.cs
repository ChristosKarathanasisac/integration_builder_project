using IntegrationBuilder.SQLServerUtilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Text;

namespace IntegrationBuilder.VannaAIUtilities
{
    public class VannaUtilitiesService
    {
        public bool CheckIfVannaModelExistOrCreated(string url,string modelName,out string error) 
        {
            error = "";
            try
            {
                RequestVannaModel req = new RequestVannaModel();
                req.model = modelName;

                string jsonData = JsonConvert.SerializeObject(req, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });


                string sresult = "";
                string sentRequetsError = "";
                if (!GeneralUtilities.GeneralUtilities.SendRequest(jsonData, url, out sentRequetsError, out sresult))
                {
                    error = "Error in CheckIfVannaModelExistOrCreated (SendRequest). Exception message: " + sentRequetsError;
                    return false;
                }
                ResponseVannaModel oResponse = new ResponseVannaModel();
                oResponse = JsonConvert.DeserializeObject<ResponseVannaModel>(sresult);


                if (oResponse.success == true)
                {
                    return true;

                }
                else 
                {
                    error =  oResponse.error;
                    return false;
                }

            }
            catch (Exception exc) 
            {
                error = "Exception in CheckIfVannaModelExistOrCreated. Exception message: " + exc.Message;
                return false;
            }
        }

        public List<string> GetInfosFromDB(string url, Credentials credentials, out string error) 
        {
            error = "";
            try
            {
                RequestWithServerAndDB req = new RequestWithServerAndDB();
                req.server = credentials.Server;
                req.db = credentials.Database;

                string jsonData = JsonConvert.SerializeObject(req, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });


                string sresult = "";
                string sentRequetsError = "";
                if (!GeneralUtilities.GeneralUtilities.SendRequest(jsonData, url, out sentRequetsError, out sresult))
                {
                    error = "Error in GetAllTablesNames (SendRequest). Exception message: " + sentRequetsError;
                    return null;
                }
                ResponseWithServerAndDB oResponse = new ResponseWithServerAndDB();
                oResponse = JsonConvert.DeserializeObject<ResponseWithServerAndDB>(sresult);

                if (oResponse.success == true)
                {
                    return oResponse.data;
                }
                else
                {
                    error = oResponse.error;
                    return null;
                }

            }
            catch (Exception exc)
            {
                error = "Exception in GetAllTablesNames. Exception message: " + exc.Message;
                return null;
            }
        }
    }
}
