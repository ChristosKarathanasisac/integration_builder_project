using IntegrationBuilder.HuggingChatUtilities;
using System.Text;

namespace IntegrationBuilder.Pages
{
    public partial class Main
    {
        private HuggingChatUtilitiesService _huggingChatUtilitiesService;
        private string _query;
        private string _objClassStr;
        private string _mappingDesc;
        private string _resultClass;

        async Task BtnCreateMapping()
        {
            this._loadignBarValue = 100;
            this.StateHasChanged();
            await Task.Run(async () =>
            {
                this._infomsgs = "";
                if (string.IsNullOrEmpty(_query))
                {
                    this._infomsgs = "Query field is empty!";
                    this._loadignBarValue = 0;
                    return;
                }

                if (string.IsNullOrEmpty(_objClassStr))
                {
                    this._infomsgs = "Object class field is empty!";
                    this._loadignBarValue = 0;
                    return;
                }

                if (string.IsNullOrEmpty(_mappingDesc))
                {
                    this._infomsgs = "Mapping Description field is empty!";
                    this._loadignBarValue = 0;
                    return;
                }

                StringBuilder urlBuilder = new StringBuilder();
                urlBuilder.Append(configuration["ApplicationInfo:HuggingChatMainURL"].ToString());
                urlBuilder.Append("/");
                urlBuilder.Append(configuration["ApplicationInfo:useHaggingChat"].ToString());
                string resp = "";
                string error = "";
                string instructions = @"Create a method that executes the given query on a SQL Server database.
                                It should store the returned records in a DataTable, then create an object of the root class based on this data.
                                Subsequently, convert this object to JSON using the Newtonsoft library and execute a POST request to a specified URL.
                                Do the entire mapping. Give me only the method with the mapping";
                string huggingChatInput = this._query + '\n' + this._objClassStr + '\n' + this._mappingDesc + '\n' + instructions;
                string conversation_id = "";
                resp = this._huggingChatUtilitiesService.UseHaggingChat(urlBuilder.ToString(), huggingChatInput, true, out conversation_id, out error, "-999");
                if (string.IsNullOrEmpty(resp))
                {
                    this._infomsgs = $"Something went wrong in Use Hugging Chat. Error message: {error}";
                    this._loadignBarValue = 0;
                    return;
                }

                this._resultClass = resp;

            });
            this._loadignBarValue = 0;
            this.StateHasChanged();

        }
    
        async Task BtnDownloadProject()
        {
            try 
            {
                string sourceDirectory = @"C:\ChristosProjects\IntegrationWindowsService";
                string destinationDirectory = @"C:\ChristosProjects\NewIntegrationWinService";

                CopyDirectory(sourceDirectory, destinationDirectory);

                //Check If Directory Created
                if (!Directory.Exists(destinationDirectory))
                {
                    this._infomsgs = "Something went wrong with project creation.";
                    return;
                }

                string filePath = $"{destinationDirectory}\\IntegrationWinService\\IntegrationWinService\\IntegrationBuilder.cs";
                string fileContent = File.ReadAllText(filePath);
                string modifiedContent = fileContent.Replace(@"code1", "new_text");
                File.WriteAllText(filePath, modifiedContent);

                //I should delete the file after download!
            }
            catch (Exception exc) 
            {
                string test = exc.Message;
            }
            

          
        }

        private void CopyDirectory(string source, string destination)
        {
            // Δημιουργία του νέου φακέλου αν δεν υπάρχει
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            // Αντιγραφή αρχείων από τον αρχικό φάκελο στον φάκελο προορισμού
            string[] files = Directory.GetFiles(source);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destinationPath = Path.Combine(destination, fileName);
                File.Copy(file, destinationPath, true);
            }

            // Αντιγραφή υπο-φακέλων
            string[] subDirectories = Directory.GetDirectories(source);
            foreach (string subDirectory in subDirectories)
            {
                string subDirectoryName = Path.GetFileName(subDirectory);
                string newDestination = Path.Combine(destination, subDirectoryName);
                // Καλεί τη συνάρτηση αναδρομικά για τον κάθε υπο-φάκελο
                CopyDirectory(subDirectory, newDestination);
            }
        }
    }
}