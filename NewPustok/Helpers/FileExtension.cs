namespace NewPustok.Helpers
{
    public static class FileExtension
    {
        public static bool IsValidSize(this IFormFile file, float kb = 2000)
            => file.Length <= kb * 1024;
        public static bool IsCorrectType(this IFormFile file, string contentType = "image")
            => file.ContentType.Contains(contentType);
        public static async Task<string> SaveAsync(this IFormFile file, string path)
        {
            string extension = Path.GetExtension(file.FileName);
            string fileName = Path.GetFileNameWithoutExtension(file.FileName).Length > 129 ?
                file.FileName.Substring(0, 129) :
                Path.GetFileNameWithoutExtension(file.FileName);
            fileName = Path.Combine(path, Path.GetRandomFileName() + fileName + extension);
            using (FileStream fs = File.Create(Path.Combine(PathConstants.RootPath, fileName)))
            {
                await file.CopyToAsync(fs);
            }
            return fileName;
        }
        
    }
}
