using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed class IntegerLogValue : ILogFieldValue {
	public long Value;
	public FieldValueType GetFieldValueType() => FieldValueType.Integer;
	public Type GetValueType() => typeof(long);
	public object GetValue() => Value;

	public IntegerLogValue(long? value = null) {
		// TODO handle default value
		this.Value = value ?? -1;
	}
}
