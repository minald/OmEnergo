using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OmEnergo.Infrastructure
{
    public class FileManager
    {
        private Repository Repository { get; set; }
        private IHostingEnvironment HostingEnvironment { get; set; }

        public FileManager(Repository repository, IHostingEnvironment hostingEnvironment)
        {
            Repository = repository;
            HostingEnvironment = hostingEnvironment;
        }

        public async Task<bool> UploadFileAsync(string englishName, IFormFile uploadedFile)
        {
            if (uploadedFile == null)
            {
                return false;
            }

            CommonObject obj = Repository.GetObjectByEnglishName(englishName);
            string path;
            if (Path.GetExtension(uploadedFile.FileName).ToLower() == ".pdf")
            {
                string directoryPath = HostingEnvironment.WebRootPath + obj.GetDirectoryPath();
                path = directoryPath + $@"\{obj.Id}_{uploadedFile.FileName}";
                if (File.Exists(path))
                {
                    return false;
                }
            }
            else
            {
                string mainImagePath = HostingEnvironment.WebRootPath + obj.GetMainImageFullLink();
                path = File.Exists(mainImagePath) ? GetPathWithGuid(mainImagePath, obj.Id) : mainImagePath;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }

            return true;
        }

        public void DeleteFile(string deletedFilePath, string englishName)
        {
            if (File.Exists(deletedFilePath))
            {
                File.Delete(deletedFilePath);
            }

            CommonObject obj = Repository.GetObjectByEnglishName(englishName);
            string mainImagePath = HostingEnvironment.WebRootPath + obj.GetMainImageFullLink();
            if (deletedFilePath == mainImagePath)
            {
                MakeSecondaryImageMain(obj, mainImagePath);
            }
        }

        private void MakeSecondaryImageMain(CommonObject obj, string mainImagePath)
        {
            string secondaryImagePath = GetFilesPaths(obj)
                .FirstOrDefault(p => p.ToLower().EndsWith("jpg") && p != mainImagePath);
            if (secondaryImagePath != null)
            {
                File.Move(secondaryImagePath, mainImagePath);
            }
        }

        public List<string> GetFilesPaths(CommonObject commonObject)
        {
            string directoryPath = HostingEnvironment.WebRootPath + commonObject.GetDirectoryPath();
            string[] mainImage = Directory.GetFiles(directoryPath, $"{commonObject.Id}.*");
            string[] otherFiles = Directory.GetFiles(directoryPath, $"{commonObject.Id}_*");
            return mainImage.Union(otherFiles).ToList();
        }

        public void MakeImageMain(string path, string englishName)
        {
            CommonObject obj = Repository.GetObjectByEnglishName(englishName);
            string mainImagePath = HostingEnvironment.WebRootPath + obj.GetMainImageFullLink();
            if (path != mainImagePath)
            {
                File.Move(mainImagePath, GetPathWithGuid(mainImagePath, obj.Id));
                File.Move(path, mainImagePath);
            }
        }

        private string GetPathWithGuid(string path, int id) => path.Replace($"{id}.", $"{id}_{Guid.NewGuid()}.");
    }
}
