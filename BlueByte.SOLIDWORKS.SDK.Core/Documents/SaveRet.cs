using BlueByte.SOLIDWORKS.SDK.Core.Enums;
using System;
using System.Collections.Generic;
/// <summary>
/// Save operation result.
/// </summary>
public struct SaveRet
{
    /// <summary>
    /// Gets or sets the extension.
    /// </summary>
    /// <value>
    /// The extension.
    /// </value>
    public FileExtension_e Extension { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="SaveRet"/> is success.
    /// </summary>
    /// <value>
    ///   <c>true</c> if success; otherwise, <c>false</c>.
    /// </value>
    public bool Success { get; set; }
    /// <summary>
    /// Gets or sets the errors.
    /// </summary>
    /// <value>
    /// The errors.
    /// </value>
    public int Errors { get; set; }
    /// <summary>
    /// Gets or sets the warnings.
    /// </summary>
    /// <value>
    /// The warnings.
    /// </value>
    public int Warnings { get; set; }

    /// <summary>
    /// Gets the error.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public string[] GetErrors()
    {
        swDescriptiveFileSaveError_e e = (swDescriptiveFileSaveError_e)Errors;

        var es = new List<string>();
 
        var members = Enum.GetValues(typeof(swDescriptiveFileSaveError_e));

        foreach (var member in members)
        {
            var m = (swDescriptiveFileSaveError_e)member;

            if (e.HasFlag(e) == false)
                continue;

            es.Add(EnumHelper.DescriptionAttr<swDescriptiveFileSaveError_e>(m));
        }



        return es.ToArray();
    }



    /// <summary>
    /// Gets the warning messages.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.Exception"></exception>
    public string[] GetWarning()
    {
        swDescriptiveFileWarningError_e e = (swDescriptiveFileWarningError_e)Warnings;

        var es = new List<string>();

        var members = Enum.GetValues(typeof(swDescriptiveFileWarningError_e));

        foreach (var member in members)
        {

            

            var m = (swDescriptiveFileWarningError_e)member;

            if (e.HasFlag(e) == false)
                continue;

            es.Add(EnumHelper.DescriptionAttr<swDescriptiveFileWarningError_e>(m));
        }



        return es.ToArray();
    }
}





