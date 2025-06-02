using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace H5_GenericDataLogger.Data {
    public sealed class Log {
        /// <summary>
        /// SQLite id.
        /// <remark>Only has a value for Log's that actually exist in the database</remark>
        /// </summary>
        public readonly long Id;
        public readonly string Title;

        /// <summary>Gets fields schema from the Database</summary>
        public IEnumerable<LogField> LoadFields(DBConnector connector) {
            using SqliteCommand command = new SqliteCommand();
            command.CommandText = @"SELECT TOP(1) FROM logs WHERE id = $id";
            command.Parameters.AddWithValue("$id", this.Id);
            using SqliteDataReader dataReader = connector.ExecuteReader(command);
            do {
                yield return new LogField {
                    Label = dataReader.GetString(0),
                    ValueType = (ValueType)dataReader.GetInt64(1),
                };
            } while (dataReader.NextResult());
        }


        private Log(string title) {
            this.Title = title;
        }


    }
}
