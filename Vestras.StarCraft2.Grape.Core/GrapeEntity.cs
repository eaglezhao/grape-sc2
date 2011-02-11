using System;
using System.Collections.Generic;

namespace Vestras.StarCraft2.Grape.Core {
    public abstract class GrapeEntity {
        public string FileName { get; protected internal set; }
        public int Offset { get; protected internal set; }
        public int Length { get; protected internal set; }
        public int StartLine { get; protected internal set; }
        public int StartColumn { get; protected internal set; }
        public int EndLine { get; protected internal set; }
        public int EndColumn { get; protected internal set; }
        public GrapeEntity Parent { get; protected internal set; }
    }
}
