using System.Text;
using Xamasoft.JsonClassGenerator.CodeWriters;
using Xamasoft.JsonClassGenerator;
using Xamasoft.JsonClassGenerator.CodeWriterConfiguration;
using Xamasoft.JsonClassGenerator.Models;

namespace JsonToCSharpClasses
{
    public class Helper
    {
        public CSharpCodeWriterConfig writerConfig { get; set; } = new CSharpCodeWriterConfig();
        public void GenerateCSharp()
        {
           
        String jsonText = "{\r\n    \"name\": \"John Doe\"," +
                "\r\n    \"age\": 30,\r\n    \"email\": \"johndoe@example.com\"," +
                "\r\n    \"isSubscribed\": true,\r\n    \"address\": {\r\n        \"street\": \"123 Main St\",\r\n        \"city\": \"Anytown\",\r\n        \"zipcode\": \"12345\"\r\n    },\r\n    \"hobbies\": [\"reading\", \"hiking\", \"gaming\"]\r\n}";
            if (String.IsNullOrWhiteSpace(jsonText))
            {
                //this.csharpOutputTextbox.Text = String.Empty;
                return;
            }

            JsonClassGenerator generator = new JsonClassGenerator();
            generator.CodeWriter = new CSharpCodeWriter(writerConfig);
            this.ConfigureGenerator(generator);

            try
            {
                StringBuilder sb = generator.GenerateClasses(jsonText, errorMessage: out String errorMessage);
                if (!String.IsNullOrWhiteSpace(errorMessage))
                {
                    //this.csharpOutputTextbox.Text = "Error:\r\n" + errorMessage;
                }
                else
                {
                    string result = sb.ToString();
                    // this.copyOutput.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ///this.csharpOutputTextbox.Text = "Error:\r\n" + ex.ToString();
                string exmsg = ex.Message.ToString();
            }
        }

        private void ConfigureGenerator(JsonClassGenerator config)
        {
            writerConfig.UsePascalCase = false;

            if (false)
            {
                writerConfig.AttributeLibrary = JsonLibrary.NewtonsoftJson;
            }
            else// implicit: ( this.optAttribJpn.Checked )
            {
                writerConfig.AttributeLibrary = JsonLibrary.SystemTextJson;
            }

            //

            if (false)
            {
                writerConfig.OutputMembers = OutputMembers.AsProperties;
            }
            else// implicit: ( this.optMemberFields.Checked )
            {
                writerConfig.OutputMembers = OutputMembers.AsPublicFields;
            }

            //

            if (false)
            {
                writerConfig.OutputType = OutputTypes.ImmutableClass;
            }
            else if (false)
            {
                writerConfig.OutputType = OutputTypes.MutableClass;
            }
            else// implicit: ( this.optTypesRecords.Checked )
            {
                writerConfig.OutputType = OutputTypes.ImmutableRecord;
            }
        }
    }
}
