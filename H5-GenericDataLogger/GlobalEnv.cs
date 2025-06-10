using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

using Windows.UI.WebUI;

namespace H5_GenericDataLogger.Data {
    internal static class GlobalEnvironment {
        private static DBConnector? _Connector = null;
        public static DBConnector Connector {
            get {
                if (_Connector is null) {
                    _Connector = new DBConnector();
                    _Connector.EnsureRequiredSchema();
                }
                return _Connector;
            }
        }

        private sealed record class LogFieldJsonDTO(string label, FieldValueType value_type);
        private sealed record class LogEntryJsonDTO(params object[] values);
        private sealed record class LogJsonDTO(long id, string title, IList<LogFieldJsonDTO> fields, IList<LogEntryJsonDTO> entries);

        private static JsonSerializerOptions? _JsonSerializerConfig = null;
        public static JsonSerializerOptions JsonSerializerConfig {
            get {
                if (_JsonSerializerConfig is null) {
                    _JsonSerializerConfig = new();
                    _JsonSerializerConfig.IncludeFields = true;
                    _JsonSerializerConfig.WriteIndented = false;
                    //_JsonSerializerConfig.NewLine = "\n";
                    _JsonSerializerConfig.AllowTrailingCommas = false;
                }
                return _JsonSerializerConfig;
            }
        }
        public static string ConvertToJson(this Log log) {
            LogFieldJsonDTO[] fields_jsondto = log.Fields
                .OrderBy(f => f.Id)
                .Select(f => new LogFieldJsonDTO(f.Label, f.ValueType))
                .ToArray();
            LogEntryJsonDTO[] entries_jsondto = Connector.GetLogEntries(log)
                .AsParallel()
                .Select(e => new LogEntryJsonDTO(e.Select(v => v.GetValue())))
                .ToArray();

            LogJsonDTO result = new(log.Id, log.Title, fields_jsondto, entries_jsondto);
            return System.Text.Json.JsonSerializer.Serialize(result, JsonSerializerConfig);
        }
    }
}
