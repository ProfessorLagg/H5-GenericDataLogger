using Microsoft.Data.Sqlite;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
		private readonly List<LogField> _Fields = new();
		public IEnumerable<LogField> Fields {
			get { foreach (LogField f in _Fields) { yield return f; } }
		}

		internal Log(long id, string title, IEnumerable<LogField> fields) {
			this.Title = title;
			this.Id = id;
			this._Fields.AddRange(fields);
		}
		internal Log(long id, string title) : this(id, title, Enumerable.Empty<LogField>()) { }

		public ILogFieldValue[] CreateEntry(params object?[] values) {
			ILogFieldValue[] result = new ILogFieldValue[this._Fields.Count];
			if (values.Length != result.Length) { throw new ArgumentException($"Invalid value count passed! Expected {result.Length}, but found {values.Length}"); }

			for (int i = 0; i < result.Length; i++) {
				Debug.Assert(this._Fields[i].Id > 0);
				switch (this._Fields[i].ValueType) {
					default: throw new NotImplementedException();
					case FieldValueType.Unknown: throw new InvalidDataException("Unkown value type");
					case FieldValueType.Text: result[i] = new TextLogValue((string?)values[i]); break;
					case FieldValueType.Integer: result[i] = new IntegerLogValue((long?)values[i]); break;
					case FieldValueType.Float: result[i] = new FloatLogValue((double?)values[i]); break;
					case FieldValueType.DateTime: result[i] = new DateTimeLogValue((DateTime?)values[i]); break;
					case FieldValueType.Location: result[i] = new LocationLogValue((LocationLogValue.LocationData?)values[i]); break;
					case FieldValueType.Image: throw new NotImplementedException(); // TODO
					case FieldValueType.Bool: throw new NotImplementedException(); // TODO
				}
			}

			return result;
		}

		/// <summary>
		/// Returns the information about this Log as json
		/// </summary>
		/// <remarks>
		/// Does not include entries 
		/// </remarks>
		public string ToJson() {
			StringBuilder sb = new();
			sb.Append('{');

			// id
			sb.Append("\"id\":");
			sb.Append(this.Id.ToString("0"));
			sb.Append(',');

			// title
			sb.Append("\"title\":\"");
			sb.Append(this.Title);
			sb.Append("\",");

			// fields
			sb.Append("\"fields\":[");
			for (int i = 0; i < this._Fields.Count; i++) {
				LogField field = this._Fields[i];
				sb.Append('{');

                // id
                sb.Append("\"id\":\"");
                sb.Append(field.Id);
                sb.Append("\",");

                // label
                sb.Append("\"label\":\"");
				sb.Append(field.Label);
				sb.Append("\",");

				// value_type
				sb.Append("\"value_type\":");
				sb.Append(field.ValueType.ToInt().ToString("0", System.Globalization.CultureInfo.InvariantCulture));

				sb.Append('}');
				if (i < this._Fields.Count - 1) { sb.Append(','); }
			}
			sb.Append(']');

			sb.Append('}');

			return sb.ToString();
		}
	}
}
