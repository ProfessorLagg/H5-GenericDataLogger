using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data {
    public sealed class DBConnector {
        private const string DefaultDataSource = "GenericDataLogger.db";
        private static readonly string[] RequiredSchemaSql = {
            H5_GenericDataLogger.Data.Properties.Resources.sql_ensure_logs_table,
            H5_GenericDataLogger.Data.Properties.Resources.sql_ensure_logfields_table
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


        public void EnsureRequiredSchema() {
            for (ushort i = 0; i < RequiredSchemaSql.Length; i++) {
                using (SqliteCommand command = new SqliteCommand(RequiredSchemaSql[i])) {
                    _ = this.ExecuteNonQuery(command);
                }
            }
        }
    }
}
