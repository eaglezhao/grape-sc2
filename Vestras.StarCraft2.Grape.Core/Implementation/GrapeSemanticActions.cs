using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using bsn.GoldParser.Grammar;
using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.Core.Implementation {
	public class GrapeSemanticActions: SemanticTypeActions<GrapeEntity> {
		private class GrapeAccessExpressionFactory: SemanticNonterminalFactory<GrapeEntity, GrapeAccessExpression> {
			private readonly ReadOnlyCollection<Type> inputTypes = Array.AsReadOnly(new[] {typeof(GrapeMember)});

			public override ReadOnlyCollection<Type> InputTypes {
				get {
					return inputTypes;
				}
			}

			public override GrapeAccessExpression Create(Rule rule, IList<GrapeEntity> tokens) {
				return ((GrapeMember)tokens[0]).ToExpression();
			}
		}

		public GrapeSemanticActions(): base(CompiledGrammar.Load(typeof(GrapeParser), "grape.cgt")) {}

		protected override void InitializeInternal(ICollection<string> errors, bool trace) {
			GrapeAccessExpressionFactory factory = new GrapeAccessExpressionFactory();
			RegisterNonterminalFactory(new RuleAttribute("<Value> ::= <Method Call>").Bind(Grammar), factory);
			RegisterNonterminalFactory(new RuleAttribute("<Member Access> ::= <Array Access>").Bind(Grammar), factory);
			RegisterNonterminalFactory(new RuleAttribute("<Member Access> ::= <Field Access>").Bind(Grammar), factory);
			base.InitializeInternal(errors, trace);
		}
	}
}
