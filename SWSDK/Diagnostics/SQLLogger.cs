using BlueByte.SOLIDWORKS.SDK.Core;
using System;
using System.Data.SqlClient;

namespace BlueByte.SOLIDWORKS.SDK.Diagnostics
{
    internal class SQLLogger : LoggerBase, ILogger
    {


        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>
        /// The connection string.
        /// </value>
        public string ConnectionString { get; private set; }

        internal void Verify()
        {
            var type = GetLoggerType();
            switch (type)
            {
                case LoggerType_e.Console:
                case LoggerType_e.File:
                    throw new Exception($"Chosen logger type is {type.ToString()}");
                case LoggerType_e.SQL:
                    break;
                default:
                    break;
            }

            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new Exception("ConnectionString was not set.");

        }

        /// <summary>
        /// Starts the connection.
        /// </summary>
        public void StartConnection()
        {
            Verify();

            if (cnn == null)
            {
                cnn = new SqlConnection(ConnectionString);
                cnn.Open();
            }
            else if (cnn.State == System.Data.ConnectionState.Closed)
            {
                cnn.Open();
            }



        }

        /// <summary>
        /// Ends the connection.
        /// </summary>
        /// <returns></returns>
        public void EndConnection()
        {
            Verify();

            if (cnn.State == System.Data.ConnectionState.Open)
                cnn.Close();
        }


        /// <summary>
        /// Sets the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string OutputLocation { get; set; }

        public void Init(Identity identity, string connectionString)
        {
            SetConnectionString(connectionString);

           
            this.identity = identity;
        }

        


      

        public void LogToOutput(string target, string value)
        {
            Verify();
            var sql = $"INSERT into {target}(TimeStamp,AddInName,Message) VALUES (@TimeStamp,@AddInName,@Message)";



            var command = new SqlCommand(sql, base.cnn);

            command.Parameters.Add(new SqlParameter("@TimeStamp", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@AddInName", string.IsNullOrWhiteSpace(identity.Name) ? string.Empty : identity.Name));
         
            command.Parameters.Add(new SqlParameter("@Message", value));

            command.ExecuteNonQuery();
        }
    }



}
