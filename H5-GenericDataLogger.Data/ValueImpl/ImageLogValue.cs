using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed record class ImageLogValue : ILogFieldValue {
	public byte[] Value = Array.Empty<byte>();
	public FieldValueType GetFieldValueType() => FieldValueType.Real;
	// TODO Figure out how to encode Image Data
	public Type GetValueType() => typeof(byte[]);
	public object GetValue() => Value;
}
