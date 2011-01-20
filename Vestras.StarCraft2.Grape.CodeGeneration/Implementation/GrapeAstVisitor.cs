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
        private GrapeCodeGeneratorConfiguration config;

        private void VisitNodesForEntityList(List<GrapeEntity> entities) {
            foreach (GrapeEntity entity in entities) {
                foreach (IAstNodeVisitor nodeVisitor in nodeVisitors) {
                    if (entity.GetType() == nodeVisitor.NodeType) {
                        nodeVisitor.Config = config;
                        nodeVisitor.VisitNode(entity);
                        break;
                    }
                }

                if (entity is GrapeEntityWithBlock) {
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
            VisitNodesForEntityList(config.Ast.Children);
        }
    }
}
