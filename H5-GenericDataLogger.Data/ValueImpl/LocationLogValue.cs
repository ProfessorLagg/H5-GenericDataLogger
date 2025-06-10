using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed class LocationLogValue : ILogFieldValue {
    public record struct LocationData {
        public static readonly LocationData Default = new LocationData { Latitude = 0.0f, Longitude = 0.0f };

        float Latitude;
        float Longitude;

        // TODO use an offset union to pack/unpack instantly
        public long Pack() {
            Span<byte> spanResult = new byte[sizeof(long)];
            BitConverter.GetBytes(this.Latitude).CopyTo(spanResult.Slice(0, sizeof(float)));
            BitConverter.GetBytes(this.Longitude).CopyTo(spanResult.Slice(sizeof(float), sizeof(float)));
            return BitConverter.ToInt64(spanResult);
        }

        public static LocationData Unpack(long packed) {
            Span<byte> spanResult = BitConverter.GetBytes(packed);
            return new LocationData() {
                Latitude = BitConverter.ToSingle(spanResult.Slice(0, sizeof(float))),
                Longitude = BitConverter.ToSingle(spanResult.Slice(sizeof(float), sizeof(float)))
            };
        }
    }
    public long Value;
    public FieldValueType GetFieldValueType() => FieldValueType.DateTime;
    public Type GetValueType() => typeof(LocationData);
    public object GetValue() => LocationData.Unpack(this.Value);

    public LocationLogValue(LocationData? value = null) {
        // TODO Handle default value
        LocationData v = value ?? LocationData.Default;
        this.Value = v.Pack();
    }

    public LocationLogValue(long? packed = null) {
        // TODO Handle default value
        this.Value = packed ?? 0;
    }
}
