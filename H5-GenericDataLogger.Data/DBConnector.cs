using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
            lock (this.ConnectionLock) {
                this.Connection.Open();
                try {
                    long log_id = -1;
                    using (SqliteCommand createlog_cmd = this.Connection.CreateCommand()) {
                        createlog_cmd.CommandText = @"INSERT INTO logs (title) VALUES ($title) RETURNING id";
                        createlog_cmd.Parameters.AddWithValue("$title", title);
                        object? createlog_result = createlog_cmd.ExecuteScalar();
                        // TODO Error Handling
                        log_id = (long)createlog_result!;
                    }

                    using (SqliteCommand createfields_cmd = this.Connection.CreateCommand()) {
                        // TODO use parameters to make this safer
                        createfields_cmd.CommandText =
                            "INSERT INTO log_fields (log_id, label, data_type) VALUES\n"
                            + string.Join(",\n", fields.Select(f => $"({log_id},'{f.Label}',{f.ValueType.ToInt()})")) + ';';
                        int createfields_result = createfields_cmd.ExecuteNonQuery();
                        // TODO Error Handling
                    }
                    return new Log(log_id, title, fields);
                }
                finally { this.Connection.Close(); }
            }
        }
        public void AddLogEntry(Log log, IList<ILogFieldValue> values) {
            // TODO Validate that values matches fields
            lock (this.ConnectionLock) {
                this.Connection.Open();
                using (SqliteCommand cmd = this.Connection.CreateCommand()) {
                    cmd.CommandText = "INSERT INTO log_entries VALUES\n";

                }
                this.Connection.Close();
            }
        }
        public DataTable GetRawLogEntries(Log log) {
            Debug.Assert(log.Id > 0);
            ReadOnlySpan<LogField> fields = log.Fields.ToArray();
            Debug.Assert(fields.Length > 0);
            const string entry_name = @"e";
            using (SqliteCommand cmd = new()) {
                List<string> col_names = new() { $"{entry_name}.id" };
                List<string> joins = new();
                cmd.CommandText = @"SELECT e.id FROM log_entries as e";
                for (int i = 0; i < fields.Length; i++) {
                    string field_name = $"v{i}";
                    string field_id_col_name = $"{field_name}.field_id";
                    string field_val_col_name = $"{field_name}.val";
                    string join_statement = $"JOIN {fields[i].ValueType.TableName()} as {field_name} ON {field_name}.entry_id == e.id";

                    col_names.Add(field_id_col_name);
                    col_names.Add(field_val_col_name);
                    joins.Add(join_statement);
                }


                cmd.CommandText =
                    @"SELECT" +
                    ' ' + string.Join(',', col_names) +
                    @" FROM log_entries as e" +
                    ' ' + string.Join('\n', joins) +
                    $" WHERE e.log_id = {log.Id}";
                return this.ExecuteReader(cmd);
            }
        }
        public IEnumerable<IList<ILogFieldValue>> GetLogEntries(Log log) {
            
            LogField[] fields = log.Fields.ToArray();
            using DataTable entries = this.GetRawLogEntries(log);
            //foreach (DataRow raw_row in entries.Rows) {
            for(int rawi = 0; rawi < entries.Rows.Count; rawi++) {
                DataRow raw_row = entries.Rows[rawi];
                ILogFieldValue[] row = new ILogFieldValue[fields.Length];
                for (int i = 0; i < fields.Length; i++) {
                    int ri = i * 2 + 2;
                    row[i] = LogFieldValueCreator.CreateValue(fields[i].ValueType, raw_row[ri]);
                }
                yield return row;
            }
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
