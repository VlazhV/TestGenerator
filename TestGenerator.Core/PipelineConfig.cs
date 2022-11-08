using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.Core
{
	public class Config
	{
		public int MaxReadingTasks{ get; set; }
		public int MaxProcessingTasks{ get; set; }
		public int MaxWritingTasks{ get; set; }

		public Config(int maxReadingTasks, int maxProcessingTasks, int maxWritingTasks)
		{
			MaxReadingTasks = maxReadingTasks;
			MaxProcessingTasks = maxProcessingTasks;			
			MaxWritingTasks = maxWritingTasks;
		}
	}
}
