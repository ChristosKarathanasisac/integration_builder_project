namespace IntegrationBuilder.VannaAIUtilities
{
    public class VannaModel
    {
        private string modelName;
        private bool newModel;

        public VannaModel()
        {
            this.newModel = false;
        }

        public string ModelName { get => modelName; set => modelName = value; }
        public bool NewModel { get => newModel; set => newModel = value; }
    }
}
