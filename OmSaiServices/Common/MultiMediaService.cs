//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

using System.IO;


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
	}
}
