﻿using TestGenerator.Core;
using TestGenerator.Console;

PipelineConfig config = new PipelineConfig(5, 5, 5);
Pipeline pipeline = new Pipeline(config);
string? userInput;
string inputDirectory;
string outputDirectory;

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

	Console.WriteLine( "Processing..." );

	await pipeline.PerformProcessing( Directory.GetFiles( inputDirectory, "*.cs" ), outputDirectory );
	Console.WriteLine( "Ready" );
	Console.WriteLine( "\n\n\n\n\n\n\n\n" );
	Console.WriteLine( "Input input-directory" );

} while ( !userInput.Equals( "exit" ) );

