using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed class BooleanLogValue : ILogFieldValue {
    public long Value;
    public FieldValueType GetFieldValueType() => FieldValueType.Bool;
    public Type GetValueType() => typeof(bool);
    public object GetValue() {
        Debug.Assert(Value >= 0 && Value <= 1);
        return Value == 1;
    }

    public BooleanLogValue(bool? value = null) {
        bool v = value ?? false;
        this.Value = v ? 1 : 0;
    }

    public BooleanLogValue(long? value = null) {
        Debug.Assert(value == 0 || value == 1 || value == null);
        this.Value = value ?? 0;
    }
}
