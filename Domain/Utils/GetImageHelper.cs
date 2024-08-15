namespace Domain.Utils;

public static class ImageHelper
{
    public static string GetImageBase64(string imagePath, string folder)
    {
        string baseDirectory = Path.Combine(AppContext.BaseDirectory, "../../../../../LayeredApiImages");
        string fullPath = Path.Combine(baseDirectory, folder, imagePath);

        if (System.IO.File.Exists(fullPath))
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(fullPath);
            string imageBase64 = Convert.ToBase64String(imageBytes);
            string fileExtension = Path.GetExtension(imagePath).TrimStart('.');
            return $"data:image/{fileExtension};base64,{imageBase64}";
        }

        return null;
    }
}