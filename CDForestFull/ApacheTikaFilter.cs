using System.IO;

namespace CDForest
{
	public class ApacheTikaFilter
	{

		public bool IsKnownFile(string path)
		{
			switch(Path.GetExtension(path).ToLower())
			{
				case ".pdf":
				case ".doc":
				case ".docx":
				case ".xls":
				case ".xlsx":
				case ".ppt":
				case ".pptx":
				case ".rtf":
				case ".jpg":
					return true;
			}

			return false;
		}

		public bool IsOnlyMethadataFile(string path)
		{
			switch (Path.GetExtension(path).ToLower())
			{
				case ".pdf":
				case ".doc":
				case ".docx":
				case ".xls":
				case ".xlsx":
				case ".ppt":
				case ".pptx":
				case ".rtf":
					return false;
				case ".jpg":
					return true;
			}

			return false;
		}

		public bool IsTextFile(string path)
		{
			switch (Path.GetExtension(path).ToLower())
			{
				case ".pdf":
				case ".doc":
				case ".docx":
				case ".xls":
				case ".xlsx":
				case ".ppt":
				case ".pptx":
				case ".rtf":
				case ".jpg":
					return true;
			}

			return false;
		}
	}
}
