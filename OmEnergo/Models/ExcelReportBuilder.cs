using ClosedXML.Excel;
using System.Linq;

namespace OmEnergo.Models
{
    public class ExcelReportBuilder
    {
        private Repository Repository { get; }

        public ExcelReportBuilder(Repository repository) => Repository = repository;

        public void BackupDatabase(string databaseName, string backupPath)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sections");

                worksheet.Cell($"A1").Value = "Id";
                worksheet.Cell($"B1").Value = "Name";
                worksheet.Cell($"C1").Value = "EnglishName";
                worksheet.Cell($"D1").Value = "SequenceNumber";
                worksheet.Cell($"E1").Value = "Description";
                worksheet.Cell($"F1").Value = "ParentSectionId";
                worksheet.Cell($"G1").Value = "ProductProperties";
                worksheet.Cell($"H1").Value = "ProductModelProperties";

                var sections = Repository.GetAllSections();
                for (int i = 1; i < sections.Count(); i++)
                {
                    worksheet.Cell($"A{i + 1}").Value = sections.ElementAt(i).Id;
                    worksheet.Cell($"B{i + 1}").Value = sections.ElementAt(i).Name;
                    worksheet.Cell($"C{i + 1}").Value = sections.ElementAt(i).EnglishName;
                    worksheet.Cell($"D{i + 1}").Value = sections.ElementAt(i).SequenceNumber;
                    worksheet.Cell($"E{i + 1}").Value = sections.ElementAt(i).Description;
                    worksheet.Cell($"F{i + 1}").Value = sections.ElementAt(i).ParentSection?.Id;
                    worksheet.Cell($"G{i + 1}").Value = sections.ElementAt(i).ProductProperties;
                    worksheet.Cell($"H{i + 1}").Value = sections.ElementAt(i).ProductModelProperties;
                }
                
                workbook.SaveAs(backupPath);
            }
        }
    }
}
