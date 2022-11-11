using TestGenerator.Core;
using TestGenerator.Console;


string? userInput;
string inputDirectory;
string outputDirectory;
int maxRead;
int maxWrite;
int maxProcess;

Console.WriteLine( "Input 'exit' for exit\n" );

Console.WriteLine( "Input input-directory" );
do
{

	userInput = Console.ReadLine();
	if ( userInput.Equals( "exit" ) )
		break;
	while ( !Directory.Exists( userInput ) )
	{
		Console.WriteLine( "There is no such directory. Repeat." );
		userInput = Console.ReadLine();
		if ( userInput.Equals( "exit" ) )
			break;
	}

	if ( userInput.Equals( "exit" ) )
		break;

	inputDirectory = userInput;

	Console.WriteLine( "Input output-directory" );
	userInput = Console.ReadLine();
	
	if ( userInput.Equals( "exit" ) )
		break;

	
	while(true)
	{
		if ( Directory.Exists( userInput ) )
			break;
				
		try
		{				
			Directory.CreateDirectory( userInput );
			break;
		}
		catch ( Exception e)
		{
			Console.WriteLine( "There is no such directory and can't create new directory. Repeat" );
			userInput = Console.ReadLine();
			if ( userInput.Equals( "exit" ) )
				break;
		}
	}

	if ( userInput.Equals( "exit" ) )
		break;

	outputDirectory = userInput;


	Console.WriteLine( "Input max reading tasks" );
	userInput = Console.ReadLine();
	if ( userInput.Equals( "exit" ) )
		break;
	while (!int.TryParse(userInput, out maxRead) )
	{
		Console.WriteLine( "Incorrect format. Repeat");
		Console.WriteLine( "Input max reading tasks" );
		userInput = Console.ReadLine();
		if ( userInput.Equals( "exit" ) )
			break;
	}
	if ( userInput.Equals( "exit" ) )
		break;

	Console.WriteLine( "Input max processing tasks" );
	userInput = Console.ReadLine();
	if ( userInput.Equals( "exit" ) )
		break;
	while ( !int.TryParse( userInput, out maxProcess ) )
	{
		Console.WriteLine( "Incorrect format. Repeat" );
		Console.WriteLine( "Input max processing tasks" );
		userInput = Console.ReadLine();
		if ( userInput.Equals( "exit" ) )
			break;
	}
	if ( userInput.Equals( "exit" ) )
		break;


	Console.WriteLine( "Input max writing tasks" );
	userInput = Console.ReadLine();
	if ( userInput.Equals( "exit" ) )
		break;
	while ( !int.TryParse( userInput, out maxWrite ) )
	{
		Console.WriteLine( "Incorrect format. Repeat" );
		Console.WriteLine( "Input max writing tasks" );
		userInput = Console.ReadLine();
		if ( userInput.Equals( "exit" ) )
			break;
	}
	if ( userInput.Equals( "exit" ) )
		break;


	PipelineConfig config = new PipelineConfig( maxRead, maxProcess, maxWrite );
	Pipeline pipeline = new Pipeline( config );

	Console.WriteLine( "Processing..." );

	await pipeline.PerformProcessing( Directory.GetFiles( inputDirectory, "*.cs" ), outputDirectory );
	Console.WriteLine( "Ready" );
	Console.WriteLine( "\n\n\n\n\n\n\n\n" );
	Console.WriteLine( "Input input-directory" );

} while ( !userInput.Equals( "exit" ) );

