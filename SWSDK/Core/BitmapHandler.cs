using System;
using System.Collections;
using System.Drawing;
using System.IO;
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
            Stream manifestResourceStream;
            Bitmap bitmap;
            try
            {
                manifestResourceStream = callingAssy.GetManifestResourceStream(bitmapName);
                bitmap = new Bitmap(manifestResourceStream);
            }
            catch (Exception ex)
            {
                throw ex;

               
            }
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

