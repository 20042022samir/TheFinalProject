namespace FinalProject.Extentions
{
	public static class FileUpload
	{
		public static string CreateImage(this IFormFile file,string folder, string path)
		{
			string fullName = Guid.NewGuid().ToString() + file.FileName;
			string fulPath=Path.Combine(folder, path,fullName);
			using(FileStream stream = new(fulPath, FileMode.Create))
			{
				file.CopyTo(stream);
			}
			return fullName;
		}
	}
}
