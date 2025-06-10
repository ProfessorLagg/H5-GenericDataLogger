using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed class DateTimeLogValue : ILogFieldValue {
    public long Value;
    public FieldValueType GetFieldValueType() => FieldValueType.DateTime;
    public Type GetValueType() => typeof(DateTime);
    public object GetValue() => new DateTime(this.Value);

    public DateTimeLogValue(DateTime? value = null) {
        // TODO Handle default value
        DateTime v = value ?? DateTime.MinValue;
        this.Value = v.Ticks;
    }

    public DateTimeLogValue(long? ticks = null) {
        // TODO Handle default value
        this.Value = ticks ?? DateTime.MinValue.Ticks;
    }
}
