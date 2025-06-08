using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data;
public sealed class LocationLogValue : ILogFieldValue {
	public record struct LocationData {
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
	public long Value = 0;
	public FieldValueType GetFieldValueType() => FieldValueType.DateTime;
	public Type GetValueType() => typeof(LocationData);
	public object GetValue() => LocationData.Unpack(this.Value);
}
