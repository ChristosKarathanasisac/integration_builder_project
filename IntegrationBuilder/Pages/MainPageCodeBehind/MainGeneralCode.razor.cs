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
        }

        private void OnChangeStep(int index)
        {
            this._infomsgs = String.Empty;
        }
        private void OnChangeTab(int index) 
        {
            if (index == 2) 
            {
                this._query = this._savedQuery;
                this._objClassStr = this._cSharpClasses;
            }
        }
    }
}