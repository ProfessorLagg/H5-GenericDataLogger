using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
					_JsonSerializerConfig.Converters.Add(new FullDateTimeConverter());
				}
				return _JsonSerializerConfig;
			}
		}
		private sealed class FullDateTimeConverter : JsonConverter<DateTime> {
			public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				return DateTime.ParseExact(reader.GetString()!, "MM/dd/yyyy", CultureInfo.InvariantCulture);
			}

			public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
				string y = value.Year.ToString("0000");
				string m = value.Month.ToString("00");
				string d = value.Day.ToString("00");
				writer.WriteStringValue(value.ToString(@"yyyy\-MM\-dd\THH\:mm\:ss\.fffzzz"));
			}
		}
		public static string ToFullJson(this Log log) {
			LogFieldJsonDTO[] fields_jsondto = log.Fields
				.OrderBy(f => f.Id)
				.Select(f => new LogFieldJsonDTO(f.Label, f.ValueType))
				.ToArray();
			LogEntryJsonDTO[] entries_jsondto = Connector.GetLogEntries(log)
				.AsParallel()
				.Select(e => new LogEntryJsonDTO(e.Select(v => v.GetValue())))
				.ToArray();

			LogJsonDTO resultdto = new(log.Id, log.Title, fields_jsondto, entries_jsondto);
			string result = JsonSerializer.Serialize(resultdto, JsonSerializerConfig);
			return result;
		}
	}
}
