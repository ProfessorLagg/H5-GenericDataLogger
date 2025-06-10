using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public interface ILogFieldValue {
    FieldValueType GetFieldValueType();
    Type GetValueType();
    object GetValue();
}

public static class LogFieldValueCreator {
    public static ILogFieldValue CreateValue(FieldValueType type, object? value) {
        switch (type) {
            default: throw new ArgumentException(nameof(type));
            case FieldValueType.Text: return new TextLogValue((string?)value);
            case FieldValueType.Integer: return new IntegerLogValue((long?)value);
            case FieldValueType.Float: return new FloatLogValue((double?)value);
            case FieldValueType.DateTime: return new DateTimeLogValue((long?)value);
            case FieldValueType.Location: return new LocationLogValue((long?)value);
            case FieldValueType.Image: return new ImageLogValue((byte[]?)value);
            case FieldValueType.Bool: return new BooleanLogValue((long?)value);
            case FieldValueType.Blob: return new BlobLogValue((byte[]?)value);
        }
    }
}