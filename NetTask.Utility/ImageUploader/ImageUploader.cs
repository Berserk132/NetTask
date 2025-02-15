

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace NetTask.Utility.ImageUploader
{
    public class ImageUploader
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageUploader(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public bool DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return false;
            }

            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('\\'));

            if (File.Exists(finalPath))
            {
                File.Delete(finalPath);
                return true;
            }

            return false;
        
        
        }

        public string UploadImage(IFormFile file, string oldImagePath = null, string subFolder = "images/employee")
        {
            if (file == null || file.Length <= 0)
                return oldImagePath;

            // Delete old image if exists
            DeleteImage(oldImagePath);

            // Upload new image
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, subFolder);
            string finalPath = Path.Combine(uploadFolder, fileName);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            using (var stream = new FileStream(finalPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return $@"\{subFolder}\{fileName}";
        }
    }
}
