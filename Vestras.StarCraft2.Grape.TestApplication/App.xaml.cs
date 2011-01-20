using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows;

namespace Vestras.StarCraft2.Grape.TestApplication {
    public partial class App : Application {
        private CompositionContainer container;

        [Import(typeof(Window))]
        public new Window MainWindow {
            get { return base.MainWindow; }
            set { base.MainWindow = value; }
        }

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            if (Compose()) {
                MainWindow.Show();
            } else {
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            if (container != null) {
                container.Dispose();
            }
        }

        private bool Compose() {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(GetType().Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog("."));
            container = new CompositionContainer(catalog);
            try {
                container.ComposeParts(catalog, this);
            } catch (CompositionException compositionException) {
                MessageBox.Show(compositionException.ToString());
                Shutdown(1);
            }

            return true;
        }
    }
}
