using System.Collections.ObjectModel;

using bsn.GoldParser.Semantic;

using Vestras.StarCraft2.Grape.Core.Implementation;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeMember : GrapeEntity {
        private readonly ReadOnlyCollection<GrapeIdentifier> identifiers;
        private readonly GrapeMember ofMember;

        [Rule("<Field Access> ::= <Method Call> ~'.' <Qualified ID>")]
        [Rule("<Field Access> ::= <Array Access> ~'.' <Qualified ID>")]
        public GrapeMember(GrapeMember ofMember, GrapeList<GrapeIdentifier> identifiers) {
            this.ofMember = ofMember;
            this.identifiers = identifiers.ToList(this).AsReadOnly();
        }

        [Rule("<Field Access> ::= <Qualified ID>")]
        public GrapeMember(GrapeList<GrapeIdentifier> identifiers) : this((GrapeMember)null, identifiers) { }

        [Rule("<Field Access> ::= <Object> ~'.' <Qualified ID>")]
        public GrapeMember(GrapeObject grapeObject, GrapeList<GrapeIdentifier> identifiers) : this((GrapeMember)null, new GrapeList<GrapeIdentifier>(grapeObject, identifiers)) { }

        public GrapeAccessExpression ToExpression() {
            GrapeAccessExpression expression = ToExpression(null);
            return (ofMember != null) ? ofMember.ToExpression(expression) : expression;
        }

        protected virtual GrapeAccessExpression GetAccessor(GrapeAccessExpression next) {
            return next;
        }

        protected virtual GrapeAccessExpression ToExpression(GrapeIdentifier identifier, GrapeAccessExpression next) {
            return GrapeMemberExpression.Create(identifiers.Count == 1 ? GrapeAccessExpression.GrapeAccessExpressionType.Root : GrapeAccessExpression.GrapeAccessExpressionType.Field, identifier, next);
        }

        private GrapeAccessExpression ToExpression(GrapeAccessExpression next) {
            next = GetAccessor(next);
            if (identifiers.Count > 0) {
                next = ToExpression(identifiers[identifiers.Count - 1], next);
                for (int i = identifiers.Count - 2; i >= 0; i--) {
                    next = GrapeMemberExpression.Create(i == 0 ? GrapeAccessExpression.GrapeAccessExpressionType.Root : GrapeAccessExpression.GrapeAccessExpressionType.Field, identifiers[i], next);
                }
            }
            return next;
        }
    }
}