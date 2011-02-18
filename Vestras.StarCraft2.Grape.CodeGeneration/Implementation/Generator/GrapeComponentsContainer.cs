using System;
using System.Collections.Generic;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.CodeGeneration.Implementation.Generation {
    internal class GrapeComponentsContainer {

        private List<string> variables = new List<string>();

        public void AddVariable(GrapeVariable v) {
            string fullName = v.Name;
            GrapeEntity e = v.Parent;

            while (e != null) {
                fullName = e.GetPotentialEntityName() + "__" + fullName;
                e = e.Parent;
            }

            if (v.Length == 0) {
                fullName = v.Type + " " + fullName + ";" + Environment.NewLine;
            } else {
                fullName = v.Type + "[" + v.Length + "] " + fullName + ";" + Environment.NewLine;
            }


            variables.Add(fullName);
        }
    }
}