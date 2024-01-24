using System.Data.SqlClient;

namespace IntegrationBuilder.SQLServerUtilities
{
    public static class SQLServerLib
    {
        public static bool CheckConnection(string server, string db, out string error)
        {
            error = "";
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.DataSource = server;

                builder.InitialCatalog = db;
                //For Windows Authentication
                builder.IntegratedSecurity = true;
                builder.ConnectTimeout = 30;
                
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    bool flag = false;
                    try 
                    {
                        connection.Open();
                        flag = true;
                    }catch (Exception ex) 
                    {
                        error = ex.ToString();
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return flag;
                }
}
            catch (SqlException e)
            {
    error = $"Connect to database error. Error message: {e.ToString()}";
    return false;
}
return true;
        }
    }
}
