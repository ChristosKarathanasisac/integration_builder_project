using IntegrationBuilder.HuggingChatUtilities;
using Microsoft.JSInterop;
using System.IO.Compression;
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
                string instructions = @"Create a method private Data MappingFun(DataTable d). 
                                        Inside this method, I want to create an object of the Data class. 
                                        The data to be inserted into the object is located within the table d passed as a parameter. 
                                        Please write the entire necessary code for the functionality I described.
                                        Give only the method and nothing else";

                string huggingChatInput = this._query + '\n' + this._objClassStr + '\n' + this._mappingDesc + '\n'+ instructions;
                string conversation_id = "";
                resp = this._huggingChatUtilitiesService.UseHaggingChat(urlBuilder.ToString(), huggingChatInput, true, out conversation_id, out error, "-999");
                if (string.IsNullOrEmpty(resp))
                {
                    this._infomsgs = $"Something went wrong in Use Hugging Chat. Error message: {error}";
                    this._loadignBarValue = 0;
                    return;
                }

                error = "";
                string cleanResp = GeneralUtilities.GeneralUtilities.GetCleanCodeFromHuggingChat(resp,out error);
                if (!string.IsNullOrEmpty(error)) 
                {
                    this._infomsgs = error;
                    this._loadignBarValue = 0;
                    return;
                }

                this._resultClass = cleanResp;

            });
            this._loadignBarValue = 0;
            this.StateHasChanged();

        }
        async Task BtnDownloadProject()
        {
            try 
            {
                if (string.IsNullOrEmpty(_resultClass))
                {
                    this._infomsgs = "Result method is empty. Create the code first.";
                    return;
                }

                string sourceDirectory = @"C:\ChristosProjects\IntegrationWindowsService";
                string destinationDirectory = @"C:\ChristosProjects\NewIntegrationWinService";

                CopyDirectory(sourceDirectory, destinationDirectory);

                //Check If Directory Created
                if (!Directory.Exists(destinationDirectory))
                {
                    this._infomsgs = "Something went wrong with project creation.";
                    return;
                }


                //Step 1 insert method in template project
                string filePath = $"{destinationDirectory}\\IntegrationWinService\\IntegrationWinService\\IntegrationBuilder.cs";
                string fileContent = File.ReadAllText(filePath);
                string modifiedContent = fileContent.Replace("//rep_code1", _resultClass);
                modifiedContent = modifiedContent.Replace("//rep_DataSource", this._sqlServerCredentials.Server);
                modifiedContent = modifiedContent.Replace("//rep_Initialcatalog", this._sqlServerCredentials.Database);
                File.WriteAllText(filePath, modifiedContent);

                //Step 2 insert the IntegrationData class.
                filePath = $"{destinationDirectory}\\IntegrationWinService\\IntegrationWinService\\IntegrationData.cs";
                fileContent = File.ReadAllText(filePath);
                modifiedContent = fileContent.Replace("//rep_code2", _objClassStr);
                File.WriteAllText(filePath, modifiedContent);

                //I should download the creatded project
                ZipFolder($"{destinationDirectory}\\IntegrationWinService", $"{ destinationDirectory}\\IntegrationWinService.zip");
                await DownLoadFile();

                //I should delete the created project (Folder and zip) after download!
                Directory.Delete(destinationDirectory+ "\\IntegrationWinService", true);
                File.Delete($"{destinationDirectory}\\IntegrationWinService.zip");
            }
            catch (Exception exc) 
            {
                this._infomsgs = $"Exception in BtnDownloadProject. Exception message:{exc.Message}";
            }
        }

        private async Task DownLoadFile()
        {
            this._infomsgs = "";
            var fileStream = CopyFileToMemory();
            if (fileStream == null)
            {
                return;
            }
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            try
            {
                await JS.InvokeVoidAsync("downloadFileFromStream", "IntegrationWinService.zip", streamRef);
            }
            catch (Exception exc)
            {
                _infomsgs = "Exception in DownLoadFile. Exception message: " + exc.Message;
            }
        }
        private Stream CopyFileToMemory()
        {
            try
            {
                var ms = new MemoryStream();
                string dir = configuration["ApplicationInfo:NewProject"].ToString();
               
                if (!Directory.Exists(dir))
                {
                    this._infomsgs = "Directory of the new project missing";
                    return null;
                }
                string destinationDirectory = @"C:\ChristosProjects\NewIntegrationWinService";
                string file = $"{destinationDirectory}\\IntegrationWinService.zip";

                //Check if file exist

                using (FileStream fs = File.OpenRead(file))
                {
                    fs.CopyTo(ms);
                }

                //For file download. Without this I will download empty file
                ms.Position = 0;
                return ms;
            }
            catch (Exception exc)
            {
                _infomsgs = "Exception in CopyFileToMemory. Exception message: " + exc.Message;
                return null;
            }
        }
        static void ZipFolder(string folderPath, string zipFilePath)
        {
            try
            {
                ZipFile.CreateFromDirectory(folderPath, zipFilePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating zip file: {ex.Message}");
            }
        }

        private void CopyDirectory(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            string[] files = Directory.GetFiles(source);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destinationPath = Path.Combine(destination, fileName);
                File.Copy(file, destinationPath, true);
            }
            string[] subDirectories = Directory.GetDirectories(source);
            foreach (string subDirectory in subDirectories)
            {
                string subDirectoryName = Path.GetFileName(subDirectory);
                string newDestination = Path.Combine(destination, subDirectoryName);
                CopyDirectory(subDirectory, newDestination);
            }
        }
    }
}