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

        private void VisitNodesForEntityList(List<GrapeEntity> entities) {
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
                            GrapeBlock logicalBlockParent = entity.GetLogicalParentOfEntityType<GrapeBlock>();
                            if (logicalBlockParent != null) {
                                if (logicalBlockParent.Children.FindLast(new Predicate<GrapeEntity>(delegate(GrapeEntity e) {
                                    return e == entity;
                                })) == entity) {
                                    config.OutputCode += Environment.NewLine + "}" + Environment.NewLine;
                                }
                            }
                        } else {
                            nodeVisitor.Validator.ValidateNode(entity);
                        }

                        break;
                    }
                }

                if (entity is GrapeStatement) {
                    if (((GrapeStatement)entity).CanHaveBlock) {
                        VisitNodesForEntityList(((GrapeStatement)entity).Block.Children);
                    }
                } else if (entity is GrapeEntityWithBlock) {
                    VisitNodesForEntityList(((GrapeEntityWithBlock)entity).Block.Children);
                } else if (entity is GrapeBlock) {
                    VisitNodesForEntityList(((GrapeBlock)entity).Children);
                } else if (entity is GrapePackageDeclaration) {
                    VisitNodesForEntityList(((GrapePackageDeclaration)entity).Children);
                }
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
