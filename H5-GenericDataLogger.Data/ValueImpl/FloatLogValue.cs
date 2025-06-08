using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed class FloatLogValue : ILogFieldValue {
	public double Value = 0;
	public FieldValueType GetFieldValueType() => FieldValueType.Real;
	public Type GetValueType() => typeof(double);
	public object GetValue() => Value;
}
