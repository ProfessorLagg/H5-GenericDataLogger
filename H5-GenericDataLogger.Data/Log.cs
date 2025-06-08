using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace H5_GenericDataLogger.Data {
	public sealed class Log {
		/// <summary>
		/// SQLite id.
		/// <remark>Only has a value for Log's that actually exist in the database</remark>
		/// </summary>
		public readonly long Id = -1;
		public readonly string Title = "INVALID";

		public Log(long id, string title) {
			this.Title = title;
			this.Id = id;
		}


		#region DEBUG
		// TODO Actually use the database. This is just here for debugging purposes
		public Log(string title) { this.Title = title; }
		private List<LogField> _fields = new();


		private List<Log>
		#endregion


		/// <summary>Gets fields schema from the Database</summary>
		public IEnumerable<LogField> LoadFields(DBConnector connector) {
#if DEBUG
			foreach (LogField field in this._fields) {
				yield return field;
			}
			yield break;
#else
			using SqliteCommand command = new SqliteCommand();
			command.CommandText = @"SELECT TOP(1) FROM logs WHERE id = $id";
			command.Parameters.AddWithValue("$id", this.Id);
			using SqliteDataReader dataReader = connector.ExecuteReader(command);
			do {
				yield return new LogField {
					Label = dataReader.GetString(0),
					ValueType = (FieldValueType)dataReader.GetInt64(1),
				};
			} while (dataReader.Read());
#endif
		}

		public void AddFields(IEnumerable<LogField> fields) {
			_fields.AddRange(fields);
#if DEBUG
			_fields.AddRange(fields);
#else
			throw new NotImplementedException();
#endif
		}






	}
}
