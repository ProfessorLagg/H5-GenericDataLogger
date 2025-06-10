using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed class FloatLogValue : ILogFieldValue {
	public double Value;
	public FieldValueType GetFieldValueType() => FieldValueType.Float;
	public Type GetValueType() => typeof(double);
	public object GetValue() => Value;

	public FloatLogValue(double? value = null) {
		// TODO Handle Default Value
		this.Value = value ?? double.NaN;
	}
}

