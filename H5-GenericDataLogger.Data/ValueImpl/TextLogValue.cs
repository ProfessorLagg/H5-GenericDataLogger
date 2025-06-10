using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed record class TextLogValue : ILogFieldValue {
	public string Value;
	public FieldValueType GetFieldValueType() => FieldValueType.Text;
	public Type GetValueType() => typeof(string);
	public object GetValue() => Value;

	public TextLogValue(string? value = null) {
		// TODO Handle default value
		this.Value = value ?? string.Empty;
	}
}
