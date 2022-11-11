using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestGenerator.Core;

namespace TestGenerator.Tests
{
	[TestClass]
	public class CoreTests
	{
		[TestMethod]
		public void GenerateNormalTestMethod()
		{
			var code = @"
			namespace MyCode
			{
				public class MyClass
				{
					public int Multiply(int x, int y)
					{
						return x * y;
					}

					public int Sum( int x, int y)
					{
						return x + y;
					}

					public void DoNothing()					
					{}
				}
			}
			";

			var testGen = new MsTestGenerator();
			var testCode = testGen.Generate( code, "MyClass.cs", true );
			Assert.IsTrue( testCode.Contains( "using Microsoft.VisualStudio.TestTools.UnitTesting;" ) );
			Assert.IsTrue( testCode.Contains( "using MyCode" ) );
			Assert.IsTrue( testCode.Contains( "namespace MyClass.Tests" ) );
			Assert.IsTrue( testCode.Contains( "public class MyClassTestClass" ) );
			Assert.IsTrue( testCode.Contains( "public void MultiplyTestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "public void SumTestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "public void DoNothingTestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "Assert.Fail(\"autogenerated\");" ) );

		}

		[TestMethod]
		public void GenerateOverloadTestMethod()
		{
			var code = @"
			namespace MyCode
			{
				public class MyClass
				{
					public int Multiply(int x, int y)
					{
						return x * y;
					}

					public int Multiply(int x, int y, int z)
					{
						return (x * y) % z;
					}

					public int Multiply(int x, int y, int z, bool j)
					{
						return (x * y) % z;
					}

					public int Sum( int x, int y)
					{
						return x + y;
					}

					public void DoNothing()					
					{}
				}
			}
			";

			var testGen = new MsTestGenerator();
			var testCode = testGen.Generate( code, "MyClass.cs", true );
		
			Assert.IsFalse( testCode.Contains( "public void MultiplyTestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "public void Multiply1TestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "public void Multiply2TestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "public void Multiply3TestMethod()" ) );			
		}

		[TestMethod]
		public void GeneratePrivateMethodTestMethod()
		{
			var code = @"
			namespace MyCode
			{
				public class MyClass
				{
					public int Multiply(int x, int y)
					{
						return x * y;
					}

					public int Sum( int x, int y)
					{
						return x + y;
					}

					private void DoNothing()					
					{}
				}
			}
			";

			var testGen = new MsTestGenerator();
			var testCode = testGen.Generate( code, "MyClass.cs", true );
			Assert.IsFalse( testCode.Contains( "public void DoNothingTestMethod()" ) );
		}

		[TestMethod]
		public void GenerateOneNamespaceManyClassesTestMethod()
		{
			var code = @"
			namespace MyCode
			{
				public class MyClassMulty
				{
					public int Multiply(int x, int y)
					{
						return x * y;
					}
				}

				public class MyClassSum
				{
					public int Sum(int x, int y)
					{
						return x + y;
					}
				}

				public class MyClassNothing
				{
					public void DoNothing()
					{						
					}
				}

			}
			";
			var testGen = new MsTestGenerator();
			var testCode = testGen.Generate( code, "MyClass.cs", true );
			Assert.IsTrue( testCode.Contains( "using Microsoft.VisualStudio.TestTools.UnitTesting;" ) );
			Assert.IsTrue( testCode.Contains( "using MyCode" ) );
			Assert.IsTrue( testCode.Contains( "namespace MyClass.Tests" ) );

			Assert.IsFalse( testCode.Contains( "public class MyClassMultyTestClass" ) );
			Assert.IsFalse( testCode.Contains( "public class MyClassSumTestClass" ) );
			Assert.IsFalse( testCode.Contains( "public class MyClassNothingTestClass" ) );

			Assert.IsTrue( testCode.Contains( "public class MyClassTestClass" ) );
		
			Assert.IsFalse( testCode.Contains( "public void MultiplyTestMethod()" ) );
			Assert.IsFalse( testCode.Contains( "public void SumTestMethod()" ) );
			Assert.IsFalse( testCode.Contains( "public void DoNothingTestMethod()" ) );

			Assert.IsTrue( testCode.Contains( "public void MyClassMulty_MultiplyTestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "public void MyClassSum_SumTestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "public void MyClassNothing_DoNothingTestMethod()" ) );

			Assert.IsTrue( testCode.Contains( "Assert.Fail(\"autogenerated\");" ) );
		}

		[TestMethod]
		public void GenerateManyNamespacesTestMethod()
		{
			var code = @"
			namespace MyAriphmetics
			{
				public class MyClassMulty
				{
					public int Multiply(int x, int y)
					{
						return x * y;
					}
				}

				public class MyClassSum
				{
					public int Sum(int x, int y)
					{
						return x + y;
					}
				}
			}

			namespace MyNothing
			{
				public class MyClassNothing
				{
					public void DoNothing()
					{						
					}
				}
			}

			namespace MyFooBarNamespace
			{
				public class Foo
				{
					public int Baz(int x)
					{
						return x * x;
					}
				}

				public class Bar
				{
					public void Inner(int r)
					{
						int x = r * r;
					}
				}
			}
			";

			var testGen = new MsTestGenerator();
			var testCode = testGen.Generate( code, "MyClass.cs", true );
			Assert.IsTrue( testCode.Contains( "using Microsoft.VisualStudio.TestTools.UnitTesting;" ) );
			Assert.IsTrue( testCode.Contains( "using MyAriphmetics" ) );
			Assert.IsTrue( testCode.Contains( "using MyNothing" ) );
			Assert.IsTrue( testCode.Contains( "using MyFooBarNamespace" ) );

			

			Assert.IsTrue( testCode.Contains( "namespace MyClass.Tests" ) );

			Assert.IsFalse( testCode.Contains( "public class MyClassMultyTestClass" ) );
			Assert.IsFalse( testCode.Contains( "public class MyClassSumTestClass" ) );
			Assert.IsFalse( testCode.Contains( "public class MyClassNothingTestClass" ) );

			Assert.IsTrue( testCode.Contains( "public class MyClassTestClass" ) );

			Assert.IsFalse( testCode.Contains( "public void MyClassMulty_MultiplyTestMethod()" ) );
			Assert.IsFalse( testCode.Contains( "public void MyClassSum_SumTestMethod()" ) );
			Assert.IsFalse( testCode.Contains( "public void MyClassNothing_DoNothingTestMethod()" ) );

			Assert.IsTrue( testCode.Contains( "public void MyAriphmetics_MyClassMulty_MultiplyTestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "public void MyAriphmetics_MyClassSum_SumTestMethod()" ) );			
			Assert.IsTrue( testCode.Contains( "public void MyNothing_MyClassNothing_DoNothingTestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "public void MyFooBarNamespace_Foo_BazTestMethod()" ) );
			Assert.IsTrue( testCode.Contains( "public void MyFooBarNamespace_Bar_InnerTestMethod()" ) );


			Assert.IsTrue( testCode.Contains( "Assert.Fail(\"autogenerated\");" ) );
		}
	}
}