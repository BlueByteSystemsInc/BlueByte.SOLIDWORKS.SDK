using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BlueByte.SOLIDWORKS.SDK.Core
{
    internal class BitmapHandler : IDisposable
    {
        private ArrayList files;

        public BitmapHandler() => this.files = new ArrayList();

        public void Dispose() => this.CleanFiles();

        public string CreateFileFromResourceBitmap(string bitmapName, Assembly callingAssy)
        {
            string tempFileName = Path.GetTempFileName();
            tempFileName = Path.ChangeExtension(tempFileName, "bmp");
            Stream manifestResourceStream;
            Bitmap bitmap;
            try
            {

                var names = callingAssy.GetManifestResourceNames();
                
                if (names != null)
                {
                    var name = names.ToList().FirstOrDefault(x => x.EndsWith(bitmapName, StringComparison.OrdinalIgnoreCase));
                    if (string.IsNullOrWhiteSpace(name) == false)
                    {
                        manifestResourceStream = callingAssy.GetManifestResourceStream(name);
                        bitmap = new Bitmap(manifestResourceStream);

                        try
                        {
                            bitmap.Save(tempFileName);
                            this.files.Add((object)tempFileName);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        finally
                        {
                            bitmap.Dispose();
                            manifestResourceStream.Close();
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                throw ex;

               
            }

            
            return tempFileName;
        }

        public bool CleanFiles()
        {
            foreach (string file in this.files)
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            this.files.Clear();
            this.files = (ArrayList)null;
            return true;
        }
    }
}

