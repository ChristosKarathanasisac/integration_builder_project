namespace IntegrationBuilder.VannaAIUtilities
{
    public class RequestTrainWithTables
    {
        public string model { get; set; }
        public string server { get; set; }
        public string db { get; set; }
        public List<string> desired_table_names { get; set; }
    }
}
