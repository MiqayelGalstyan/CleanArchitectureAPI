using System;
using System.IO;
using LayeredAPI.Domain.Models.Request;

namespace Domain.Utils;

public static class FileUploadHelper
{
    public static string ImageUpload(ImageRequest imageRequest, string folder)
    {
        byte[] fileDecoded = Convert.FromBase64String(imageRequest.Base64Image);

        string baseDirectory = Path.Combine(AppContext.BaseDirectory, "../../../../../LayeredApiImages");
        string destinationPath = Path.Combine(baseDirectory, folder);

        if (!Directory.Exists(destinationPath))
        {
            Directory.CreateDirectory(destinationPath);
        }
        
        
        string extension = imageRequest.Extension.StartsWith(".") ? imageRequest.Extension : "." + imageRequest.Extension;

        string imagePath = Guid.NewGuid() + extension;

        string fullPath = Path.Combine(destinationPath, imagePath);

        File.WriteAllBytes(fullPath, fileDecoded);

        return imagePath;
    }
}