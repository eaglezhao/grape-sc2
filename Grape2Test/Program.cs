using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;
using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Grape2Test {
	class Program {
		static void Main(string[] args) {
			GrapeParser parser = new GrapeParser();
			GrapeAst ast = parser.Parse(@"c:\Grape2.txt", false, false);
			foreach (GrapeEntity entity in ast.Children) {
				Debug.WriteLine("Entity: "+entity.GetType().Name);
				foreach (GrapeEntity child in entity.GetChildren<GrapeExpression>()) {
					Debug.WriteLine(" -> child: "+child.GetType().Name);
				}
			}
			Console.ReadKey(true);
		}
	}
}
