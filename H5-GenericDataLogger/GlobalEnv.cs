using H5_GenericDataLogger.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger {
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
    }
}
