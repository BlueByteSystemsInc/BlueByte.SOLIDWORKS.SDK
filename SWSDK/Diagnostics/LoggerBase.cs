using BlueByte.SOLIDWORKS.SDK.Core;
using BlueByte.SOLIDWORKS.SDK.Exceptions;
using System;
using System.Data.SqlClient;

namespace BlueByte.SOLIDWORKS.SDK.Diagnostics
{
    /// <summary>
    /// Base class for all loggers.
    /// </summary>
    public abstract class LoggerBase 
    {

      

       

        internal Identity identity;

        internal SqlConnection cnn;

      


        /// <summary>
        /// Gets the type of the logger.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Could not infer type of logger. Make sure to choose the logger type.</exception>
        public LoggerType_e GetLoggerType()
        {
            var typeName = this.GetType().Name.Replace("Logger","");
            LoggerType_e type;
            var rt = Enum.TryParse(typeName, out type);
            if (rt)
                return type;
            else
                throw new SOLIDWORKSSDKException("Could not infer type of logger. Make sure to choose the logger type.", null);
        }
    }



}
