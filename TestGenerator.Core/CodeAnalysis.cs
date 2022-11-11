using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGenerator.Core
{
	public class CodeAnalysis : CSharpSyntaxWalker
	{
		

		public List<string> Namespaces { get; } = new();
		public List<string> MethodNames { get; } = new();
		//				namespace			 class		  method
		public Dictionary<string, Dictionary<string, List<string>>> FileStructure { get; }
							

		private string _namespace = "";
		private string _class = "";
		private int _classCount = 0;


		public CodeAnalysis () : base(SyntaxWalkerDepth.Token)
		{
			FileStructure = new();
		}

		

		public void Analyze (SyntaxNode? node)
		{		
			base.Visit( node );
			NumerateOverloadedMethods();
		}

		public override void VisitNamespaceDeclaration( NamespaceDeclarationSyntax node )
		{
			_namespace = node.Name.ToString();
			FileStructure.Add( _namespace, new Dictionary<string, List<string>>() );			
			base.VisitNamespaceDeclaration( node );
		}

		public override void VisitFileScopedNamespaceDeclaration( FileScopedNamespaceDeclarationSyntax node )
		{
			_namespace = node.Name.ToString();
			FileStructure.Add( _namespace, new Dictionary<string, List<string>>() );
			base.VisitFileScopedNamespaceDeclaration( node );
		}

		public override void VisitClassDeclaration( ClassDeclarationSyntax node )
		{
			_class = node.Identifier.Text;
			FileStructure[ _namespace ].Add( _class, new() );
			base.VisitClassDeclaration( node );
		}

		public override void VisitMethodDeclaration( MethodDeclarationSyntax node )
		{
			if (node.Modifiers.First().Text.Equals("public"))
				FileStructure[ _namespace ][ _class ].Add( node.Identifier.Text );
			base.VisitMethodDeclaration( node );
		}

		private void NumerateOverloadedMethods()
		{
			foreach(var @namespace in FileStructure.Keys)
			{
				foreach(var @class in FileStructure[@namespace].Keys)
				{
					var methodList = FileStructure[@namespace][@class];
					for (int i = 0; i < methodList.Count - 1; i++)
					{						
						int num = 2;
						bool isRepeated = false;
						for ( int j = i + 1; j < methodList.Count; ++j )
						{

							if ( methodList[ i ].Equals( methodList[ j ] ) )
							{

								methodList[ j ] = methodList[ j ] + num.ToString();
								isRepeated = true;
								++num;
							}
						}
						if ( isRepeated )
							methodList[ i ] = methodList[ i ] + "1";

					}
				}
			}
		}
	}
}
