using H5_GenericDataLogger.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Debugger;
internal static class DBDebugger {

	public static void DebugConnector() {
		DBConnector connector = new DBConnector();
		connector.EnsureRequiredSchema();

		_ = connector.CreateLog("Ciggies", new LogField[]{
				new LogField() {
					Label = "Timestamp",
					ValueType = FieldValueType.DateTime
				}
		});

		var logs = connector.GetLogs().ToArray();
	}
}
