using H5_GenericDataLogger.Data;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Debugger;
internal static class DBDebugger {

    public static void DebugConnector() {
        DBConnector connector = new DBConnector();
        connector.EnsureRequiredSchema();

        var logs = connector.GetLogs().ToArray();
        using DataTable log0_entries = connector.GetLogEntries(logs[0]);
    }
}
