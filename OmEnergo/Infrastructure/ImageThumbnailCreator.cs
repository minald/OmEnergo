using OmEnergo.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace OmEnergo.Infrastructure
{
    public class ImageThumbnailCreator
    {
        private int MaxSize { get; set; }

        public ImageThumbnailCreator(int maxSize)
        {
            MaxSize = maxSize;
        }

        public void Create(List<CommonObject> commonObjects)
        {
            foreach (var commonObject in commonObjects)
            {
                List<string> imagePaths = FileManager.GetFullImagePaths(commonObject);
                foreach (string imagePath in imagePaths)
                {
                    string thumnailPath = GetThumbnailPath(imagePath, commonObject);
                    if (!File.Exists(thumnailPath))
                    {
                        Image image = Image.FromFile(imagePath);
                        Image thumbnail = GetThumbnailFrom(image);
                        thumbnail.Save(thumnailPath);
                    }
                }
            }
        }

        private string GetThumbnailPath(string imagePath, CommonObject obj) => 
            imagePath.Replace($"{obj.GetImageNamePrefix()}{obj.Id}", $"{obj.GetImageNamePrefix()}{obj.Id}-{MaxSize}");

        private Image GetThumbnailFrom(Image image)
        {
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            int biggerSize = Math.Max(originalWidth, originalHeight);
            double compressionRatio = biggerSize > MaxSize ? (double)biggerSize / MaxSize : 1;
            int width = (int)(originalWidth / compressionRatio);
            int height = (int)(originalHeight / compressionRatio);

            return image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
        }
    }
}
