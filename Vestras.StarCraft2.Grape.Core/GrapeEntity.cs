using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

using bsn.GoldParser.Parser;
using bsn.GoldParser.Semantic;

namespace Vestras.StarCraft2.Grape.Core {
    public abstract class GrapeEntity : SemanticToken {
        protected GrapeEntity() {
            FileName = string.Empty;
        }

        public IEnumerable<GrapeEntity> GetChildren() {
            return GetChildren<GrapeEntity>();
        }

        Hashtable typeProperties = new Hashtable();
        private PropertyInfo[] GetPropertiesForType(Type type) {
            if (!typeProperties.ContainsKey(type)) {
                PropertyInfo[] properties = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                typeProperties[type] = properties;
                return properties;
            }

            return typeProperties[type] as PropertyInfo[];
        }

        public IEnumerable<T> GetChildren<T>() where T : GrapeEntity {
            PropertyInfo[] properties = GetPropertiesForType(GetType());
            foreach (PropertyInfo property in properties) {
                if (!property.Name.Equals("Parent", StringComparison.Ordinal)) {
                    foreach (GrapeEntity child in EnumerateEntitiesOfProperty(property)) {
                        T childAsT = child as T;
                        if (childAsT != null) {
                            yield return childAsT;
                        }
                        IEnumerable<T> children = child.GetChildren<T>();
                        foreach (T childOfChild in children) {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        private IEnumerable<GrapeEntity> EnumerateEntitiesOfProperty(PropertyInfo property) {
            if (typeof(GrapeEntity).IsAssignableFrom(property.PropertyType)) {
                GrapeEntity result = (GrapeEntity)property.GetValue(this, null);
                if ((result != null) && (result != Parent)) {
                    yield return result;
                }
            } else {
                if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType)) {
                    IEnumerator enumerator = ((IEnumerable)property.GetValue(this, null)).GetEnumerator();
                    using (enumerator as IDisposable) {
                        while (enumerator.MoveNext()) {
                            GrapeEntity result = enumerator.Current as GrapeEntity;
                            if (result != null) {
                                yield return result;
                            }
                        }
                    }
                }
            }
        }

        public int EndColumn {
            get;
            private set;
        }

        public int EndLine {
            get;
            private set;
        }

        public string FileName {
            get;
            private set;
        }

        public int Length {
            get;
            private set;
        }

        public int Offset {
            get {
                return (int)((IToken)this).Position.Index;
            }
        }

        public GrapeEntity Parent {
            get;
            internal set;
        }

        public int StartColumn {
            get {
                return ((IToken)this).Position.Column;
            }
        }

        public int StartLine {
            get {
                return ((IToken)this).Position.Line;
            }
        }

        internal void InitalizeFromPosition(string fileName, LineInfo endPosition) {
            FileName = fileName;
            EndColumn = endPosition.Column;
            EndLine = endPosition.Line;
            Length = (int)(endPosition.Index - ((IToken)this).Position.Index);
            Debug.Assert(Length >= 0);
        }

        internal void InitializeFromChildren(string fileName, IEnumerable<GrapeEntity> children) {
            FileName = fileName;
            GrapeEntity lastChild = null;
            if (children != null) {
                foreach (GrapeEntity child in children) {
                    child.Parent = this;
                    lastChild = child;
                }
            }
            if (lastChild != null) {
                EndColumn = lastChild.EndColumn;
                EndLine = lastChild.EndLine;
                Length = (lastChild.Offset + lastChild.Length) - Offset;
            } else if (Length == 0) {
                EndColumn = StartColumn;
                EndLine = StartLine;
            }
        }

        internal void InitializeFromTemplate(GrapeEntity template) {
            if (template == null) {
                throw new ArgumentNullException("template");
            }
            Initialize(((IToken)template).Symbol, ((IToken)template).Position);
            Parent = template.Parent;
            Length = template.Length;
            EndColumn = template.EndColumn;
            EndLine = template.EndLine;
            FileName = template.FileName;
        }

        internal void SetStartPosition(LineInfo lineInfo) {
            Initialize(((IToken)this).Symbol, lineInfo);
        }
    }
}
