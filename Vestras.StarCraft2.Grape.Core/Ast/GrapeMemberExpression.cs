using System;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeMemberExpression : GrapeExpression {
        private GrapeExpression member;
        private GrapeMemberExpression next;

        public GrapeExpression Member {
            get {
                return member;
            }
            internal set {
                member = value;
                if (member != null) {
                    member.Parent = this;
                    member.FileName = FileName;
                }
            }
        }

        public GrapeMemberExpression Next {
            get {
                return next;
            }
            internal set {
                next = value;
                if (next != null) {
                    next.Parent = this;
                    next.FileName = FileName;
                }
            }
        }
    }
}
