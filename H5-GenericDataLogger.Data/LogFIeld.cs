using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data {
    /// <summary>
    /// Log Field schema data
    /// </summary>
    public sealed record class LogField {
        public string Label = "INVALID";
        public ValueType ValueType = ValueType.Unknown;
    }
}
