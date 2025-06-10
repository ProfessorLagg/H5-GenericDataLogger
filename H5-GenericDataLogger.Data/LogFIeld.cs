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
        public long Id { get; internal set; } = -1;
        public long LogId { get; internal set; } = -1;

        public string Label = "INVALID";
        public FieldValueType ValueType = FieldValueType.Unknown;
        // TODO Default Values
    }
}
