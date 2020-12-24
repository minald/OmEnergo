using Microsoft.AspNetCore.Http;
using OmEnergo.Infrastructure.Excel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OmEnergo.Services
{
	public class ExcelService
	{
		private readonly ExcelWriter excelWriter;
		private readonly ExcelDbUpdater excelDbUpdater;

		public ExcelService(ExcelWriter excelWriter, ExcelDbUpdater excelDbUpdater)
		{
			this.excelWriter = excelWriter;
			this.excelDbUpdater = excelDbUpdater;
		}

		public MemoryStream GetBackupFileStream()
		{
			return excelWriter.CreateExcelStream();
		}

		public string GetBackupFileName()
		{
			var currentDatetime = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
			return $"OmEnergoDB_{currentDatetime}.xlsx";
		}

		public async Task UploadExcelAsync(IFormFile uploadedFile)
		{
			using var excelFileStream = uploadedFile.OpenReadStream();
			await excelDbUpdater.ReadExcelAndUpdateDbAsync(excelFileStream);
		}
	}
}
