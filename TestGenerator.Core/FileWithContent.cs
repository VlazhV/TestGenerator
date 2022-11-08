using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.Core
{
	public class FileWithContent
	{
		public string Content { get; }
		public string FilePath { get; }

		public FileWithContent(string content, string filePath)
		{
			Content = content;
			FilePath = filePath;
		}
	}
}
