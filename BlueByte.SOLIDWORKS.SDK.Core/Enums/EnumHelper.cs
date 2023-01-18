using BlueByte.SOLIDWORKS.SDK.Core.Documents;
using System.ComponentModel;
using System.Reflection;

namespace BlueByte.SOLIDWORKS.SDK.Core.Enums
{
    public static class EnumHelper
    {
        public static string DescriptionAttr<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }

        public static bool Equals(this FileExtension_e e, IDocument doc)
        {
            switch (e)
            {
                case FileExtension_e.Default:
                    break;
                case FileExtension_e.sldprt:
                    if (doc.DocumentType == SolidWorks.Interop.swconst.swDocumentTypes_e.swDocPART)
                        return true;
                    break;
                case FileExtension_e.sldasm:
                    if (doc.DocumentType == SolidWorks.Interop.swconst.swDocumentTypes_e.swDocASSEMBLY)
                        return true;
                    break;
                case FileExtension_e.slddrw:
                    if (doc.DocumentType == SolidWorks.Interop.swconst.swDocumentTypes_e.swDocDRAWING)
                        return true;
                    break;
                case FileExtension_e.stp:
                case FileExtension_e.x_t:
                case FileExtension_e.pdf:
                case FileExtension_e.igs:
                    break;
                default:
                    break;
            }


            return false;
        }
    }
}
