﻿using OmEnergo.Models;
using OmEnergo.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace OmEnergo.Infrastructure
{
	public class ImageThumbnailCreator
	{
		private readonly int maxSize;

		public ImageThumbnailCreator(int maxSize)
		{
			this.maxSize = maxSize;
		}

		public void Create(List<CommonObject> commonObjects)
		{
			foreach (var commonObject in commonObjects)
			{
				var imagePaths = AdminFileManagerService.GetFullPathsOfObjectImages(commonObject);
				foreach (var imagePath in imagePaths)
				{
					var thumbnailPath = GetThumbnailPath(imagePath, commonObject);
					if (!File.Exists(thumbnailPath))
					{
						var image = Image.FromFile(imagePath);
						var thumbnail = GetThumbnailFrom(image);
						thumbnail.Save(thumbnailPath);
					}
				}
			}
		}

		private string GetThumbnailPath(string imagePath, CommonObject obj) => 
			imagePath.Replace($"{obj.GetImageNamePrefix()}{obj.Id}", $"{obj.GetImageNamePrefix()}{obj.Id}-{maxSize}");

		private Image GetThumbnailFrom(Image image)
		{
			var originalWidth = image.Width;
			var originalHeight = image.Height;
			var biggerSize = Math.Max(originalWidth, originalHeight);
			var compressionRatio = biggerSize > maxSize ? (double)biggerSize / maxSize : 1;
			var width = (int)(originalWidth / compressionRatio);
			var height = (int)(originalHeight / compressionRatio);

			return image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
		}
	}
}
