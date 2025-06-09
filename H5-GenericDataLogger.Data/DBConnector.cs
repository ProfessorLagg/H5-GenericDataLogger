using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data {
	public sealed class DBConnector {
		private const string DefaultDataSource = "GenericDataLogger.db";
		private static readonly string[] RequiredSchema = {
			H5_GenericDataLogger.Data.Properties.Resources.sql_ensure_logs,
			H5_GenericDataLogger.Data.Properties.Resources.sql_ensure_log_fields,
			H5_GenericDataLogger.Data.Properties.Resources.sql_ensure_log_entries,
			H5_GenericDataLogger.Data.Properties.Resources.sql_ensure_log_values,
		};
		private static string MakeConnectionString(string dataSource) {
			ArgumentException.ThrowIfNullOrWhiteSpace(dataSource);
			return new SqliteConnectionStringBuilder() {
				DataSource = dataSource,
				Cache = SqliteCacheMode.Default,
				Pooling = false,
				ForeignKeys = true,
				DefaultTimeout = 10 * 60,
				Mode = SqliteOpenMode.ReadWriteCreate,
			}.ToString();
		}

		public readonly string ConnectionString;
		private readonly SqliteConnection Connection;
		private object ConnectionLock = new();
		public DBConnector(string dataSource = DefaultDataSource) {
			this.ConnectionString = MakeConnectionString(DefaultDataSource);
			this.Connection = new SqliteConnection(this.ConnectionString);
		}

		/// <inheritdoc cref="SqliteCommand.ExecuteScalar"/>
		public object? ExecuteScalar(SqliteCommand command) {
			object? result = null;
			lock (this.ConnectionLock) {
				this.Connection.Open();
				try {
					command.Connection = this.Connection;
					result = command.ExecuteScalar();
				}
				finally {
					this.Connection.Close();
				}
			}
			return result;
		}
		/// <inheritdoc cref="SqliteCommand.ExecuteNonQuery"/>
		public int ExecuteNonQuery(SqliteCommand command) {
			int result;
			lock (this.ConnectionLock) {
				this.Connection.Open();
				try {
					command.Connection = this.Connection;
					result = command.ExecuteNonQuery();
				}
				finally {
					this.Connection.Close();
				}
			}
			return result;
		}
		/// <inheritdoc cref="SqliteCommand.ExecuteReader"/>
		public DataTable ExecuteReader(SqliteCommand command) {
			DataTable result = new();
			lock (this.ConnectionLock) {
				this.Connection.Open();
				try {
					command.Connection = this.Connection;
					SqliteDataReader reader = command.ExecuteReader();
					result.Load(reader);
				}
				finally {
					this.Connection.Close();
				}
			}
			return result;
		}
		/// <summary>Reads all logs from the database</summary>
		public IEnumerable<Log> GetLogs() {
			using SqliteCommand command = new SqliteCommand();
			command.CommandText = @"SELECT * FROM logs";
			using (DataTable tbl = this.ExecuteReader(command)) {
				foreach (DataRow row in tbl.Rows) {
					long log_id = (long)row[0];
					string log_title = (string)row[1];
					yield return new Log(log_id, log_title, this.ReadFields(log_id));
				}
			}
		}
		private IEnumerable<LogField> ReadFields(long log_id) {
			using SqliteCommand cmd = new();
			cmd.CommandText = "SELECT * FROM log_fields WHERE log_id = $log_id";
			cmd.Parameters.AddWithValue("$log_id", log_id);

			using (DataTable tbl = this.ExecuteReader(cmd)) {
				foreach (DataRow row in tbl.Rows) {
					yield return new LogField() {
						Id = (long)row[0],
						LogId = (long)row[1],
						Label = (string)row[2],
						ValueType = (FieldValueType)row[3]
					};
				}
			}

		}

		public Log CreateLog(string title, IEnumerable<LogField> fields) {
			using SqliteCommand createlog_cmd = new();
			createlog_cmd.CommandText = @"INSERT INTO logs (title) VALUES ($title) RETURNING id";
			createlog_cmd.Parameters.AddWithValue("$title", title);
			object? createlog_result = this.ExecuteScalar(createlog_cmd);
			// TODO Error Handling
			// TODO Handle 0 return
			long log_id = (long)createlog_result!;

			foreach (LogField field in fields) {
				using (SqliteCommand createfields_cmd = new()) {
					createfields_cmd.CommandText = "INSERT INTO log_fields (log_id, label, data_type) VALUES ($log_id, $label, $data_type) RETURNING id";
					createfields_cmd.Parameters.AddWithValue("$log_id", log_id);
					createfields_cmd.Parameters.AddWithValue("$label", field.Label);
					createfields_cmd.Parameters.AddWithValue("$data_type", field.ValueType.ToInt());
					object? createfields_result = this.ExecuteScalar(createlog_cmd);
					// TODO Handle errors
					// TODO Handle 0 result
				}
			}
			return new Log(log_id, title, fields);
		}

		public void EnsureRequiredSchema() {
			for (ushort i = 0; i < RequiredSchema.Length; i++) {
				using (SqliteCommand command = new SqliteCommand(RequiredSchema[i])) {
					_ = this.ExecuteNonQuery(command);
				}
			}
		}
	}
}
