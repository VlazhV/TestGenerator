using System.Collections.Concurrent;
using System.Threading.Tasks.Dataflow;
using TestGenerator.Core;

namespace TestGenerator.Console
{
	public class Pipeline
	{
		private volatile int _readingCount;
		private volatile int _processingCount;
		private volatile int _writingCount;

		private readonly PipelineConfig _config;

		public ConcurrentBag<int> NumberOfReadingTasks { get; set; }
		public ConcurrentBag<int> NumberOfProcessingTasks { get; set; }
		public ConcurrentBag<int> NumberOfWritingTasks { get; set; }

		public Pipeline(PipelineConfig config)
		{
			_config = config;
			NumberOfReadingTasks = new ConcurrentBag<int>();
			NumberOfProcessingTasks = new ConcurrentBag<int>();
			NumberOfWritingTasks = new ConcurrentBag<int>();
		}

		public async Task PerformProcessing( IEnumerable<string> files, string outputDirectoryPath)
		{
			_readingCount = 0;
			_processingCount = 0;
			_writingCount = 0;

			NumberOfReadingTasks.Clear();
			NumberOfProcessingTasks.Clear();
			NumberOfWritingTasks.Clear();

			var linkOptions = new DataflowLinkOptions() { PropagateCompletion = true };

			var readingBlock = new TransformBlock<string, FileWithContent>
			(
				async path => new FileWithContent( await ReadFile( path ), path ),
				new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _config.MaxReadingTasks }
			);

			var processingBlock = new TransformManyBlock<FileWithContent, FileWithContent>
			(
				fwc => {
					var results = ProcessFile( fwc.Content, fwc.FilePath );
					var filesWithContent = new List<FileWithContent>();
					foreach( var fileName in results.Keys )
					{
						filesWithContent.Add( new FileWithContent( results[ fileName ], outputDirectoryPath + "\\" + fileName ) );
					}
					return filesWithContent;
				},
				new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _config.MaxProcessingTasks }
			);

			var writingBlock = new ActionBlock<FileWithContent>
			(
				async fwc => await WriteFile(fwc),
				new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = _config.MaxWritingTasks }
			);


			readingBlock.LinkTo( processingBlock, linkOptions );
			processingBlock.LinkTo( writingBlock, linkOptions );

			foreach(string file in files)
			{
				readingBlock.Post(file);
			}

			readingBlock.Complete();

			await writingBlock.Completion;
		}

		private async Task<string> ReadFile(string filePath)
		{
			int incremented = Interlocked.Increment( ref _readingCount );
			NumberOfReadingTasks.Add( incremented );

			string result;
			using ( var streamReader = new StreamReader( filePath ) )
			{
				result = await streamReader.ReadToEndAsync();	
			}

			Interlocked.Decrement( ref _readingCount );
			return result;
		}

		private Dictionary<string, string> ProcessFile(string fileContent, string filePath )
		{
			int incremented = Interlocked.Increment( ref _processingCount );
			NumberOfProcessingTasks.Add( incremented );

			string fileName = filePath.Split( "\\" ).Last();
			MsTestGenerator testGenerator = new ();
			var result = testGenerator.Generate(fileContent);

			Interlocked.Decrement( ref _processingCount );
			return result;
		}

		private async Task WriteFile(FileWithContent fileWithContent)
		{
			int incremented = Interlocked.Increment( ref _writingCount );
			NumberOfWritingTasks.Add( incremented );

			using ( var streamWriter = new StreamWriter( fileWithContent.FilePath ) )
				await streamWriter.WriteAsync( fileWithContent.Content );

			Interlocked.Decrement( ref _writingCount );
			
		}

	}
}