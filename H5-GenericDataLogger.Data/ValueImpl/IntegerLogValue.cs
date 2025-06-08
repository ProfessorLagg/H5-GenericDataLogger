using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed class IntegerLogFieldValue : ILogFieldValue {
	public long Value = 0;
	public FieldValueType GetFieldValueType() => FieldValueType.Integer;
	public Type GetValueType() => typeof(long);
	public object GetValue() => Value;
}
