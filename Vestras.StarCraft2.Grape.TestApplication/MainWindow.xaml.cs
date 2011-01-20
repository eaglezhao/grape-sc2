using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Vestras.StarCraft2.Grape.CodeGeneration;
using Vestras.StarCraft2.Grape.Core;

namespace Vestras.StarCraft2.Grape.TestApplication {
    [Export(typeof(Window))]
    public partial class MainWindow : Window {
        [Import]
        private IGrapeParser parser;
        [Import]
        private IGrapeCodeGenerator codeGenerator;

        private void openButton_Click(object sender, RoutedEventArgs e) {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Grape Files (*.gp)|*.gp|All Files (*.*)|*.*" }) {
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    using (FileStream stream = new FileStream(openFileDialog.FileName, FileMode.Open)) {
                        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8)) {
                            fileContent.Text = reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private string ValidateFileContent(string fileContent) {
            return fileContent;
        }

        private void generateButton_Click(object sender, RoutedEventArgs e) {
            string fileName = Path.GetTempFileName();
            using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.OpenOrCreate), Encoding.ASCII)) {
                writer.Write(ValidateFileContent(fileContent.Text));
            }

            GrapeAst ast = parser.Parse(fileName, true, false);
            treeView.ItemsSource = ast.Children;
            string outputFileName = null;
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Galaxy Files (*.galaxy)|*.galaxy", FileName = "Grape.galaxy" }) {
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    outputFileName = saveFileDialog.FileName;
                }
            }

            if (outputFileName == null) {
                return;
            }

            codeGenerator.Generate(ast, true, false, outputFileName);
        }

        public MainWindow() {
            InitializeComponent();
        }
    }
}
