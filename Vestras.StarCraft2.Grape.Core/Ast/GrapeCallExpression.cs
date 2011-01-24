using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Vestras.StarCraft2.Grape.Core.Ast {
    public class GrapeCallExpression : GrapeMemberExpression {
        private ObservableCollection<GrapeExpression> parameters = new ObservableCollection<GrapeExpression>();

        public ObservableCollection<GrapeExpression> Parameters {
            get {
                return parameters;
            }
        }

        public GrapeCallExpression() {
            parameters.CollectionChanged += (sender, e) => {
                if (e.Action == NotifyCollectionChangedAction.Add) {
                    foreach (object item in e.NewItems) {
                        if (item is GrapeExpression) {
                            (item as GrapeExpression).Parent = this;
                        }
                    }
                }
            };
        }
    }
}
