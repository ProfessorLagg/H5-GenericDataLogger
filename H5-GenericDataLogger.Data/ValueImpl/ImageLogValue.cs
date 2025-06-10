using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed record class ImageLogValue : ILogFieldValue {
	public byte[] Value;
	public FieldValueType GetFieldValueType() => FieldValueType.Image;
	// TODO Figure out how to encode Image Data
	public Type GetValueType() => typeof(byte[]);
	public object GetValue() => Value;
	public ImageLogValue(byte[]? value = null) {
		this.Value = value ?? Array.Empty<byte>();
	}
}
