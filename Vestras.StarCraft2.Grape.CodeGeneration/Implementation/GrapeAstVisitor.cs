using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    [Export]
    internal sealed class GrapeAstVisitor {
        [ImportMany]
        private IEnumerable<IAstNodeVisitor> nodeVisitors = null;
        [ImportMany]
        private IEnumerable<IAstNodeValidator> nodeValidators = null;
        private GrapeCodeGeneratorConfiguration config;

        private IAstNodeValidator FindValidatorForVisitor(IAstNodeVisitor nodeVisitor) {
            foreach (IAstNodeValidator nodeValidator in nodeValidators) {
                foreach (Type type in nodeValidator.NodeType) {
                    if (IsTypeInTypeArray(type, nodeVisitor.NodeType)) {
                        return nodeValidator;
                    }
                }
            }

            return null;
        }

        internal static bool IsTypeInTypeArray(Type type, Type[] array) {
            foreach (Type t in array) {
                if (t == type) {
                    return true;
                }
            }

            return false;
        }

        private void VisitNodesForEntityList(IEnumerable<GrapeEntity> entities) {
            foreach (GrapeEntity entity in entities) {
                foreach (IAstNodeVisitor nodeVisitor in nodeVisitors) {
                    if (IsTypeInTypeArray(entity.GetType(), nodeVisitor.NodeType)) {
                        nodeVisitor.Config = config;
                        nodeVisitor.Validator = FindValidatorForVisitor(nodeVisitor);
                        if (nodeVisitor.Validator != null) {
                            nodeVisitor.Validator.Config = config;
                        }

                        if (config.GenerateCode) {
                            nodeVisitor.VisitNode(entity);
                            // TODO: implement end block } code generation here.
                        } else {
                            nodeVisitor.Validator.ValidateNode(entity);
                        }

                        break;
                    }
                }

                VisitNodesForEntityList(entity.GetChildren());
            }
        }

        public void VisitNodes(GrapeCodeGeneratorConfiguration config) {
            this.config = config;
            foreach (IAstNodeVisitor visitor in nodeVisitors) {
                visitor.Config = config;
            }

            foreach (IAstNodeValidator validator in nodeValidators) {
                validator.Config = config;
            }

            VisitNodesForEntityList(config.Ast.Children);
        }
    }
}
