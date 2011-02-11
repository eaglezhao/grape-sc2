using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;

namespace Vestras.StarCraft2.Grape.Core {
    [Export]
    public sealed class GrapeErrorSink {
        private List<Error> errors = new List<Error>();

        public ReadOnlyCollection<Error> Errors {
            get {
                return new ReadOnlyCollection<Error>(errors);
            }
        }

        internal void AddError(Error error) {
            errors.Add(error);
        }

        internal void Clear() {
            errors.Clear();
        }

        public class Error {
            private GrapeEntity entity;

            public ErrorType Type { get; internal set; }
            public string FileName { get; internal set; }
            public string Description { get; internal set; }
            public int Offset { get; internal set; }
            public int Length { get; internal set; }
            public int StartLine { get; internal set; }
            public int StartColumn { get; internal set; }
            public int EndLine { get; internal set; }
            public int EndColumn { get; internal set; }
            public GrapeEntity Entity {
                get {
                    return entity;
                }
                set {
                    entity = value;
                    if (entity != null) {
                        Offset = entity.Offset;
                        Length = entity.Length;
                        StartLine = entity.StartLine;
                        StartColumn = entity.StartColumn;
                        EndLine = entity.EndLine;
                        EndColumn = entity.EndColumn;
                    }
                }
            }

            public override string ToString() {
                return string.Format("{0}: Line: {1}, column: {2} -- \"{3}\"", Path.GetFileName(FileName), StartLine + 1, StartColumn + 1, Description);
            }

            public Error() {
                Type = ErrorType.Error;
            }

            public enum ErrorType {
                Error
            }
        }
    }
}
