namespace IntegrationBuilder.VannaAIUtilities
{
    public class RequestTrainWithViews
    {
        public string model { get; set; }
        public string server { get; set; }
        public string db { get; set; }
        public List<string> desired_view_names { get; set; }
    }
}
