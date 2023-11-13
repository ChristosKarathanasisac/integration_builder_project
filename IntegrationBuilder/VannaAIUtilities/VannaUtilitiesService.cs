using IntegrationBuilder.SQLServerUtilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IntegrationBuilder.VannaAIUtilities
{
    public class VannaUtilitiesService
    {
        public bool CheckIfVannaModelExistOrCreated(string url,string modelName,out string error) 
        {
            //Just For Test
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
                    error = "Error in CheckIfVannaModelExistOrCreated (SendRequest). Error message: " + sentRequetsError;
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
                    return (List<string>) oResponse.data;
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

        public bool TrainModelWithTables(string url, List<string> tables, string modelName, Credentials credentials, out string error) 
        {
            try 
            {
                error = "";
                RequestTrainWithTables req = new RequestTrainWithTables();
                req.model = modelName;
                req.server = credentials.Server;
                req.db = credentials.Database;
                req.desired_table_names = tables;

                string jsonData = JsonConvert.SerializeObject(req, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });


                string sresult = "";
                string sentRequetsError = "";
                if (!GeneralUtilities.GeneralUtilities.SendRequest(jsonData, url, out sentRequetsError, out sresult))
                {
                    error = "Error in TrainModelWithTables (SendRequest). Error message: " + sentRequetsError;
                    return false;
                }
                return true;
            }
            catch (Exception exc) 
            {
                error = $"Exception in TrainModelWithTables. Exception message:{exc.Message}";
                return false;
            }
        }

        public bool TrainModelWithViews(string url, List<string> views, string modelName, Credentials credentials, out string error)
        {
            try
            {
                error = "";
                RequestTrainWithViews req = new RequestTrainWithViews();
                req.model = modelName;
                req.server = credentials.Server;
                req.db = credentials.Database;
                req.desired_view_names = views;

                string jsonData = JsonConvert.SerializeObject(req, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });


                string sresult = "";
                string sentRequetsError = "";
                if (!GeneralUtilities.GeneralUtilities.SendRequest(jsonData, url, out sentRequetsError, out sresult))
                {
                    error = "Error in TrainModelWithViews (SendRequest). Error message: " + sentRequetsError;
                    return false;
                }
                return true;
            }
            catch (Exception exc)
            {
                error = $"Exception in TrainModelWithViews. Exception message:{exc.Message}";
                return false;
            }
        }

        public bool TrainWithDocumentation(string url, string statements, string modelName, out string error) 
        {
            try
            {
                error = "";
                RequestTrainWithStatement req = new RequestTrainWithStatement();
                req.model = modelName;
                req.statement = statements;

                string jsonData = JsonConvert.SerializeObject(req, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });


                string sresult = "";
                string sentRequetsError = "";
                if (!GeneralUtilities.GeneralUtilities.SendRequest(jsonData, url, out sentRequetsError, out sresult))
                {
                    error = "Error in TrainWithDocumentation (SendRequest). Error message: " + sentRequetsError;
                    return false;
                }
                return true;
            }
            catch (Exception exc)
            {
                error = $"Exception in TrainWithDocumentation. Exception message:{exc.Message}";
                return false;
            }
        }
        
    }
}
