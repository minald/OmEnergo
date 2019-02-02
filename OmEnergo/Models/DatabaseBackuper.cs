using ClosedXML.Excel;
using System.Linq;

namespace OmEnergo.Models
{
    public class DatabaseBackuper
    {
        private Repository Repository { get; }

        public DatabaseBackuper(Repository repository) => Repository = repository;

        public void BackupDatabase(string databaseName, string backupPath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sample Sheet");

                var models = Repository.GetAllProductModels();
                for (int i = 1; i < models.Count(); i++)
                {
                    worksheet.Cell($"A{i}").Value = models.ElementAt(i).EnglishName;
                    worksheet.Cell($"B{i}").Value = models.ElementAt(i).Price;
                }
                
                workbook.SaveAs(backupPath);
            }
        }
    }
}
