﻿using TestGenerator.Core;

var code = @"
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
		string _namespace = "";


		public CodeAnalysis () : base(SyntaxWalkerDepth.Token)
		{
		}

		public override void VisitNamespaceDeclaration( NamespaceDeclarationSyntax node )
		{
			_namespace = node.Name.ToString();
			base.VisitNamespaceDeclaration( node );
		}

		public override void VisitClassDeclaration( ClassDeclarationSyntax node )
		{
			base.VisitClassDeclaration( node );
		}

		public override void VisitMethodDeclaration( MethodDeclarationSyntax node )
		{			
			base.VisitMethodDeclaration( node );
		}
	}
}
";

MsTestGenerator msTestGen = new MsTestGenerator();
Console.WriteLine(msTestGen.Generate( code, "CodeAnalysis.cs" ));