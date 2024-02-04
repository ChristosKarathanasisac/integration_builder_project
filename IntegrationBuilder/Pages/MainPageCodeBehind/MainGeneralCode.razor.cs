using IntegrationBuilder.HuggingChatUtilities;
using IntegrationBuilder.SQLServerUtilities;
using IntegrationBuilder.VannaAIUtilities;
using Radzen;

namespace IntegrationBuilder.Pages
{
    public partial class Main
    {
        //All general code of the page
        //Parameters
        //---------------------------------
        private VannaUtilitiesService _vannaUtilitiesService;
        private string _infomsgs;
        private bool _DisabledStep2 = true;
        private bool _DisabledStep3 = true;
        private bool _DisabledStep4 = true;
        private string _savedQuery;
        TabPosition tabPosition = TabPosition.Top;
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
            _savedQuery = "";
            this._savedCSharpClasses = "";
            this._huggingChatUtilitiesService = new HuggingChatUtilitiesService();

            //For Testing Download File
            //this._query = @"SELECT mtrl.MTRL AS mtrl,
            //                   mtrgroup.CODE AS groupcode,
            //                   ISNULL(mtrl.CODE, '') + ISNULL(cccVMtrlDim.CDIM1CODE, '') AS code,
            //                   ISNULL(mtrl.CODE, '') + ISNULL(cccVMtrlDim.CDIM1CODE, '') + ISNULL(cccVMtrlDim.CDIM2CODE, '') AS sku,
            //                   mtrl.PRICER AS retail_price,
            //                   mtrl.NAME,
            //                   mtrgroup.MTRGROUP AS group_id,
            //                   mtrgroup.NAME AS group_name,
            //                   mtrmark.MTRMARK AS mark_id,
            //                   mtrmark.NAME AS mark_name,
            //                   mtrmodel.MTRMODEL AS model_id,
            //                   mtrmodel.NAME AS model_name,
            //                   mtrl.REMARKS,
            //                   mtrl.WEIGHT,
            //                   mtrl.MTRMANFCTR AS manufacturer,
            //                   mtrl.APVCODE,
            //                   cccVMtrlDim.CDIM1NAME AS color,
            //                   cccVMtrlDim.CDIM1CODE AS CDIM01,
            //                   cccVMtrlDim.CDIM2NAME AS size,
            //                   cccVMtrlDim.CDIM2CODE AS CDIM02,
            //                   mtrl.WEBVIEW,
            //                   mtrl.INSDATE AS insertion_date,
            //                   mtrl.CCCESHOPSTATUS AS e_shop_status
            //            FROM MTRL
            //            INNER JOIN mtrgroup ON mtrl.COMPANY = mtrgroup.COMPANY AND mtrl.SODTYPE = mtrgroup.SODTYPE AND mtrl.MTRGROUP = mtrgroup.MTRGROUP
            //            INNER JOIN mtrmark ON mtrl.COMPANY = mtrmark.COMPANY AND mtrl.SODTYPE = mtrmark.SODTYPE AND mtrl.MTRMARK = mtrmark.MTRMARK
            //            INNER JOIN mtrmodel ON mtrl.COMPANY = mtrmodel.COMPANY AND mtrl.SODTYPE = mtrmodel.SODTYPE AND mtrl.MTRMODEL = mtrmodel.MTRMODEL
            //            LEFT JOIN cccVMtrlDim ON mtrl.MTRL = cccVMtrlDim.MTRL
            //            WHERE mtrl.ISACTIVE = 1 AND mtrl.CCCESHOPSTATUS = 1
            //            ";
            //this._objClassStr = @"public class Payload
            //                        {
            //                            public string attribute_set { get; set; }
            //                            public string category_ids { get; set; }
            //                            public string description { get; set; }
            //                            public string groupCode { get; set; }
            //                            public string code { get; set; }
            //                            public string name { get; set; }
            //                            public int price { get; set; }
            //                            public int quantity { get; set; }
            //                            public string sku { get; set; }
            //                            public bool status { get; set; }
            //                            public string size { get; set; }
            //                            public string color { get; set; }
            //                            public string mtrl { get; set; }
            //                            public string color_softone { get; set; }
            //                            public string size_softone { get; set; }
            //                            public int include_skroutz { get; set; }
            //                            public string skroutz_availability { get; set; }
            //                            public string skroutz_ean { get; set; }
            //                            public string skroutz_id { get; set; }
            //                            public string created_at { get; set; }
            //                        }

            //                        public class IntegrationData
            //                        {
            //                            public string type { get; set; }
            //                            public Payload payload { get; set; }
            //                        }
            //                    ";
            //this._mappingDesc = @"Create a method private IntegrationData MappingFun(DataTable d). Inside this method,
            //                        I want to create an object of the IntegrationData class. The data to be inserted into the object is located within the table d passed as a parameter.
            //                        Please write the entire necessary code for the functionality I described.
            //                        The code of the payload is the code of the sql query
            //                        ";
        }

        private void OnChangeStep(int index)
        {
            this._infomsgs = String.Empty;
        }
        private void OnChangeTab(int index) 
        {
            if (index == 2) 
            {
                //For test
                this._query = this._savedQuery;
                this._objClassStr = this._cSharpClasses;
            }
        }
    }
}