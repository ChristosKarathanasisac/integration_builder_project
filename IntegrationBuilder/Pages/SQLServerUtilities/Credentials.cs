namespace IntegrationBuilder.Pages.SQLServerUtilities
{
    public class Credentials
    {
        //For Windows Authentication
        private string server;
        private string database;

        public string Server { get => server; set => server = value; }
        public string Database { get => database; set => database = value; }
    }
}
