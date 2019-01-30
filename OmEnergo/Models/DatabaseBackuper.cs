using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OmEnergo.Models
{
    public class DatabaseBackuper
    {
        private IConfiguration Configuration { get; }

        public DatabaseBackuper(IConfiguration configuration) => Configuration = configuration;

        public void BackupDatabase(string databaseName, string backupPath)
        {
            string commandText = $@"BACKUP DATABASE [{databaseName}] TO DISK = N'{backupPath}'";
            string connectionString = Configuration.GetConnectionString("OmEnergoConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
