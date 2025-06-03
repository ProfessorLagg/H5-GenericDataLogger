using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Linq;
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
        public SqliteDataReader ExecuteReader(SqliteCommand command) {
            SqliteDataReader result;
            lock (this.ConnectionLock) {
                this.Connection.Open();
                try {
                    command.Connection = this.Connection;
                    result = command.ExecuteReader();
                }
                finally {
                    this.Connection.Close();
                }
            }
            return result;
        }

        public SqliteDataReader ExecuteNonQuery(IEnumerable<SqliteCommand> commands) {
            lock (this.ConnectionLock) {
                // TODO Build this
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Log> GetLogs() {
            using SqliteCommand command = new SqliteCommand();
            command.CommandText = @"SELECT * FROM logs";
            using SqliteDataReader reader = this.ExecuteReader(command);
            do {
                long id = reader.GetInt64(0);
                string title = reader.GetString(1);
                yield return new Log(id, title);
            } while (reader.Read());
        }

        public Log CreateLog(string title, IEnumerable<LogField> fields) {
            throw new NotImplementedException();
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
