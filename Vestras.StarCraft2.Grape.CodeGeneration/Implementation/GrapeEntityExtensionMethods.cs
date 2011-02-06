using System;
using System.Collections.Generic;
using Vestras.StarCraft2.Grape.Core;
using Vestras.StarCraft2.Grape.Core.Ast;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal static class GrapeEntityExtensionMethods {
        public static GrapeEntity GetActualParent(this GrapeEntity entity) {
            if (entity.Parent != null) {
                if (entity.Parent is GrapeBlock) {
                    return ((GrapeBlock)entity.Parent).Parent;
                }

                return entity.Parent;
            }

            return null;
        }

        public static List<GrapeFunction> GetConstructors(this GrapeClass c) {
            List<GrapeFunction> list = new List<GrapeFunction>();
            if (c.Block != null) {
                foreach (GrapeEntity e in c.Block.Children) {
                    if (e is GrapeFunction && ((GrapeFunction)e).Type == GrapeFunction.GrapeFunctionType.Constructor) {
                        list.Add((GrapeFunction)e);
                    }
                }
            }

            return list;
        }

        public static List<GrapeFunction> GetDestructors(this GrapeClass c) {
            List<GrapeFunction> list = new List<GrapeFunction>();
            if (c.Block != null) {
                foreach (GrapeEntity e in c.Block.Children) {
                    if (e is GrapeFunction && ((GrapeFunction)e).Type == GrapeFunction.GrapeFunctionType.Destructor) {
                        list.Add((GrapeFunction)e);
                    }
                }
            }

            return list;
        }

        public static bool IsLogicalChildOfEntityType<TEntity>(this GrapeEntity entity)
            where TEntity : GrapeEntity {
            if (entity is TEntity) {
                return true;
            } else if (entity.Parent is TEntity) {
                return true;
            } else if (entity.Parent != null) {
                return entity.Parent.IsLogicalChildOfEntityType<TEntity>();
            }

            return false;
        }

        public static TEntity GetLogicalParentOfEntityType<TEntity>(this GrapeEntity entity)
            where TEntity : GrapeEntity {
            if (entity is TEntity) {
                return (TEntity)entity;
            } else if (entity.Parent is TEntity) {
                return (TEntity)entity.Parent;
            } else if (entity.Parent != null) {
                return entity.Parent.GetLogicalParentOfEntityType<TEntity>();
            }

            return default(TEntity);
        }
    }
}
