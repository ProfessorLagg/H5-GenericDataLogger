using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed record class BlobLogValue : ILogFieldValue {
    public byte[] Value;
    public FieldValueType GetFieldValueType() => FieldValueType.Blob;
    public Type GetValueType() => typeof(byte[]);
    public object GetValue() => Value;

    public BlobLogValue(byte[]? value = null) {
        this.Value = value ?? Array.Empty<byte>();
    }
}
