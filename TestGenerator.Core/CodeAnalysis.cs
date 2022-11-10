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
							

		private string _namespace = "";
		private string _class = "";
		private int _classCount = 0;


		public CodeAnalysis () : base(SyntaxWalkerDepth.Token)
		{}

		

		public void Analyze (SyntaxNode? node, bool needToRefactor)
		{		
			base.Visit( node );
			for ( int i = 0; i < MethodNames.Count - 1; i++ )
			{
				int num = 2;
				bool isRepeated = false;
				for ( int j = i + 1; j < MethodNames.Count; ++j )
				{

					if ( MethodNames[ i ].Equals( MethodNames[ j ] ) )
					{

						MethodNames[ j ] = MethodNames[ j ] + num.ToString();
						isRepeated = true;
						++num;
					}
				}
				if ( isRepeated )
					MethodNames[ i ] = MethodNames[ i ] + "1";				
			}

			for ( int i = 0; i < MethodNames.Count; ++i )
				MethodNames[ i ] = MethodNames[ i ].Replace( ".", "_" );

			if ( needToRefactor ) RefactorNames();

		}

		public override void VisitNamespaceDeclaration( NamespaceDeclarationSyntax node )
		{
			var newNamespace = node.Name.ToString();
			if ( !newNamespace.Equals( _namespace ) ) 
			{
				Namespaces.Add( newNamespace );
				_namespace = newNamespace;
			}
			
			base.VisitNamespaceDeclaration( node );
		}

		public override void VisitFileScopedNamespaceDeclaration( FileScopedNamespaceDeclarationSyntax node )
		{
			_namespace = node.Name.ToString();		
			base.VisitFileScopedNamespaceDeclaration( node );
		}

		public override void VisitClassDeclaration( ClassDeclarationSyntax node )
		{
			var newClass = node.Identifier.Text;
			if ( !newClass.Equals( _class ) )
			{
				_class = newClass;
				++_classCount;
			}
			base.VisitClassDeclaration( node );
		}

		public override void VisitMethodDeclaration( MethodDeclarationSyntax node )
		{
			if (node.Modifiers.First().Text.Equals( "public" ))
				MethodNames.Add( string.Format( "{0}.{1}.{2}", _namespace, _class, node.Identifier.Text ) );			
			base.VisitMethodDeclaration( node );
		}


		private void RefactorNames()
		{
			if (_classCount == 1)
			{
				for (int i = 0; i < MethodNames.Count; ++i )
				{
					MethodNames[ i ] = MethodNames[ i ].Split( "_" ).Last();
				}
				return;
			}

			if (Namespaces.Count == 1)
			{
				for ( int i = 0; i < MethodNames.Count; ++i )
				{
					var names = MethodNames[ i ].Split( "_" );
					MethodNames[ i ] = names[ ^2 ] + "_" + names.Last();
				}
			}

			
		}
	}
}
