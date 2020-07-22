using System;
using System.IO;
using System.Windows.Forms;
using Fotosteam.Service.Imaging;
using Newtonsoft.Json;

namespace Fotosteam.Console {
    static class Program {

        [STAThread]
        static void Main(string[] args) {

            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Image Files (*.jpg)|*.jpg";
            fd.Multiselect = true;
            
            if (fd.ShowDialog() == DialogResult.OK) {

                foreach (String file in fd.FileNames){

                    //System.Console.WriteLine(file);

                    FileInfo fi = new FileInfo(file);

                    MemoryStream sourceStream = new MemoryStream(File.ReadAllBytes(file));
                    var meta = Processing.ReadExifData(sourceStream);

                    var json = JsonConvert.SerializeObject(meta);
                    //System.Console.Write(json);
                    //Clipboard.SetText(json);

                    System.IO.File.WriteAllText(string.Concat(fi.Directory, "\\meta.json"), json);

                }
            }

            //System.Console.ReadLine();

        }
    }
}
