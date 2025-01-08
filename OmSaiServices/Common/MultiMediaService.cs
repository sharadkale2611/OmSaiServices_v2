//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.IO;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace LmsServices.Common	
{
	public class MultiMediaService
	{
		public string ImageUpload(IFormFile profilePhoto, string uploadPath)
		{

			if (profilePhoto == null || profilePhoto.Length == 0)
				return "";

			// Create the directory if it doesn't exist
			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
			}

			// Generate a unique file name
			var fileName = $"{Guid.NewGuid()}{Path.GetExtension(profilePhoto.FileName)}";

			// Get the complete file path
			var filePath = Path.Combine(uploadPath, fileName);

			// Save the file to the specified path
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				profilePhoto.CopyTo(stream);
			}

			// Return the complete file path or relative path (based on your requirement)
			return filePath;
		}



		public async Task<string> SaveAndCompressImageAsync(IFormFile imageFile, string uploadPath, int maxFileSizeKb, int quality = 30)
		{
			try
			{
				uploadPath = $"media/{uploadPath}";

				if (imageFile == null || imageFile.Length == 0)
					throw new ArgumentException("Invalid image file.");

				var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uploadPath);
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				var fileName = $"{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(imageFile.FileName)}.jpg";
				var filePath = Path.Combine(uploadsFolder, fileName);

				using (var inputStream = imageFile.OpenReadStream())
				using (var outputStream = new FileStream(filePath, FileMode.Create))
				{
					using (var image = await Image.LoadAsync(inputStream))
					{
						image.Mutate(x => x.Resize(new ResizeOptions
						{
							Mode = ResizeMode.Max,
							Size = new Size(800, 800) // Adjust dimensions as needed
						}));

						var encoder = new JpegEncoder
						{
							Quality = quality // Compression level
						};

						await image.SaveAsync(outputStream, encoder);
					}
				}

				var fileInfo = new FileInfo(filePath);
				if (fileInfo.Length > maxFileSizeKb * 1024)
				{
					System.IO.File.Delete(filePath);
					throw new InvalidOperationException($"Compressed image exceeds the maximum file size of {maxFileSizeKb} KB.");
				}

				var relativePath = "/" + Path.Combine(uploadPath, fileName).Replace("\\", "/");
				return relativePath;
			}
			catch (Exception ex)
			{
				// Log the exception for debugging
				Console.WriteLine($"Error in SaveAndCompressImageAsync: {ex.Message}");
				throw;
			}
		}





	}
}
