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
		//				   namespace          class        method
		public Dictionary<string, Dictionary<string, List<string>>> FileStructure { get; } = new();

		private string _namespace = "";
		private string _class = "";


		public CodeAnalysis () : base(SyntaxWalkerDepth.Token)
		{}

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
			FileStructure[ _namespace ].Add( _class, new List<string>() );
			base.VisitClassDeclaration( node );
		}

		public override void VisitMethodDeclaration( MethodDeclarationSyntax node )
		{			
			FileStructure[ _namespace ][ _class ].Add( node.Identifier.Text );
			base.VisitMethodDeclaration( node );
		}
	}
}
