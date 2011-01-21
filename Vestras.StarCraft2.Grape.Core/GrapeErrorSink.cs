using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;

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
            public ErrorType Type { get; internal set; }
            public string FileName { get; internal set; }
            public string Description { get; internal set; }
            public int Offset { get; internal set; }
            public int Length { get; internal set; }

            public Error() {
                Type = ErrorType.Error;
            }

            public enum ErrorType {
                Error
            }
        }
    }
}
