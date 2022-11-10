﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGenerator.Core
{
	public class MsTestGenerator 
	{
		CodeAnalysis codeAnalysis = new CodeAnalysis();

		BlockSyntax assertBlock = SyntaxFactory.Block
			(
				SyntaxFactory.ExpressionStatement
				( 
					SyntaxFactory.InvocationExpression
					( 
						SyntaxFactory.MemberAccessExpression
						(
							SyntaxKind.SimpleMemberAccessExpression,
							SyntaxFactory.IdentifierName( "Assert" ),
							SyntaxFactory.IdentifierName( "Fail" ) 
						) 
					)
					.WithArgumentList
					(
						SyntaxFactory.ArgumentList
						(
							SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>
							(
								SyntaxFactory.Argument
								(
									SyntaxFactory.LiteralExpression
									(
											SyntaxKind.StringLiteralExpression,
											SyntaxFactory.Literal( "autogenerated" ) 
									) 
								) 
							) 
						) 
					) 
				) 
			);

		SyntaxList<AttributeListSyntax> methodAttributeListSyntax = new SyntaxList<AttributeListSyntax>().Add(
												SyntaxFactory.AttributeList(
													new SeparatedSyntaxList<AttributeSyntax>().Add(
														SyntaxFactory.Attribute( SyntaxFactory.ParseName( "TestMethod" ) ) ) ) );


		SyntaxList<AttributeListSyntax> classAttributeListSyntax = new SyntaxList<AttributeListSyntax>().Add(
												SyntaxFactory.AttributeList(
													new SeparatedSyntaxList<AttributeSyntax>().Add(
														SyntaxFactory.Attribute( SyntaxFactory.ParseName( "TestClass" ) ) ) ) );



		public string Generate( string code, string fileName )
		{
			var root = CSharpSyntaxTree.ParseText( code ).GetRoot();
			codeAnalysis.Visit( root );
			SyntaxList<MemberDeclarationSyntax> methodDeclarations = new();
			foreach (var namespaceName in codeAnalysis.FileStructure.Keys)
			{				
				foreach(var className in codeAnalysis.FileStructure[namespaceName].Keys)
				{
					
					foreach( var methdodName in codeAnalysis.FileStructure[namespaceName][className])
					{

						methodDeclarations = methodDeclarations.Add( SyntaxFactory.MethodDeclaration( SyntaxFactory.ParseTypeName( "void" ), string.Format( "{0}__{1}__{2}{3}", namespaceName, className, methdodName, "TestMethod" ).Replace( ".", "_" ) )
						.WithAttributeLists( methodAttributeListSyntax )
						.WithBody( assertBlock ) );
					}
				}
			}

			var classDeclaration = SyntaxFactory.ClassDeclaration( fileName.Split(".").First() + "TestClass" )
			.WithAttributeLists( classAttributeListSyntax )			
			.WithMembers( methodDeclarations );

			var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration( SyntaxFactory.ParseName( fileName.Split( "." ).First() + ".Tests" ) )
			.WithMembers( new SyntaxList<MemberDeclarationSyntax>().Add( classDeclaration ) );

			var usings = new SyntaxList<UsingDirectiveSyntax>();
			usings = usings.Add( SyntaxFactory.UsingDirective( SyntaxFactory.ParseName( " Microsoft.VisualStudio.TestTools.UnitTesting" ) ) );
			foreach(var _namespace in codeAnalysis.FileStructure.Keys)
			{
				usings = usings.Add( SyntaxFactory.UsingDirective( SyntaxFactory.ParseName( " " + _namespace ) ) );
			}

			var compilationUnit = SyntaxFactory.CompilationUnit()
				.WithUsings( usings )
				.WithMembers( new SyntaxList<MemberDeclarationSyntax>().Add( namespaceDeclaration ) )
				.NormalizeWhitespace();

			return compilationUnit.ToFullString();

		}
		
	}
}
