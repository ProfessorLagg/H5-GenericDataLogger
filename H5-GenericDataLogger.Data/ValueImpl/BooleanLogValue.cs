using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed class BooleanLogValue : ILogFieldValue {
	public long Value = 0;
	public FieldValueType GetFieldValueType() => FieldValueType.Bool;
	public Type GetValueType() => typeof(bool);
	public object GetValue() {
		Debug.Assert(Value >= 0 && Value <= 1);
		return Value == 1;
	}
}
