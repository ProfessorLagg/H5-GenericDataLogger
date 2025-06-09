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
		public readonly List<LogField> Fields = new();

		internal Log(long id, string title, IEnumerable<LogField> fields) {
			this.Title = title;
			this.Id = id;
			this.Fields.AddRange(fields);
		}
		internal Log(long id, string title) : this(id, title, Enumerable.Empty<LogField>()) { }

	}
}
