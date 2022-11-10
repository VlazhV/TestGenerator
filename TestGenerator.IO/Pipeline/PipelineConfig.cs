using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator.Console
{
	public class PipelineConfig
	{
		public int MaxReadingTasks{ get; set; }
		public int MaxProcessingTasks{ get; set; }
		public int MaxWritingTasks{ get; set; }

		public PipelineConfig(int maxReadingTasks, int maxProcessingTasks, int maxWritingTasks)
		{
			MaxReadingTasks = maxReadingTasks;
			MaxProcessingTasks = maxProcessingTasks;			
			MaxWritingTasks = maxWritingTasks;
		}
	}
}
