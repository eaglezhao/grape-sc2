using System;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation {
    internal static class GrapeEntityExtensionMethods {
        public static bool IsLogicalChildOfEntityType<TEntity>(this GrapeEntity entity)
            where TEntity : GrapeEntity {
            if (entity.Parent is TEntity) {
                return true;
            } else if (entity.Parent != null) {
                return entity.Parent.IsLogicalChildOfEntityType<TEntity>();
            }

            return false;
        }

        public static TEntity GetLogicalParentOfEntityType<TEntity>(this GrapeEntity entity)
            where TEntity : GrapeEntity {
            if (entity.Parent is TEntity) {
                return (TEntity)entity.Parent;
            } else if (entity.Parent != null) {
                return entity.Parent.GetLogicalParentOfEntityType<TEntity>();
            }

            return default(TEntity);
        }
    }
}
