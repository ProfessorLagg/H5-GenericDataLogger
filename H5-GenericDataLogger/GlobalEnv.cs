using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data {
	internal static class GlobalEnvironment {
		public static void GetDebugLogs() {
			List<Log> result = new();
			Random rand = new(2025_06_08);

			#region Ciggies
			LogField[] ciggies_fields = new LogField[] {
				new LogField(){Label = "Timestamp", ValueType = FieldValueType.DateTime}
			};
			Log ciggies_log = new Log(1, "ciggies");
			ciggies_log.AddFields(ciggies_fields);

			const long FirstCiggieDateTimeTicks = 638849664000000000; // 2025-06-08 08:00:00
			List<DateTime> ciggieTimestamps = new();
			for (int di = 0; di < 30; di++) {
				for (int hi = 0; hi < 10; hi++) {
					DateTime timestamp = new DateTime(FirstCiggieDateTimeTicks)
						.AddDays(di)
						.AddHours(hi)
						.AddMinutes(rand.NextDouble() * 60)
						.AddSeconds(rand.NextDouble() * 60)
						.AddMilliseconds(rand.NextDouble() * 1000)
						.AddMicroseconds(rand.NextDouble() * 1000);

					ciggieTimestamps.Add(timestamp);
				}
			}
			ciggieTimestamps.Sort();

			#endregion
		}
		private static DBConnector? _Connector = null;
		public static DBConnector Connector {
			get {
				if (_Connector is null) {
					_Connector = new DBConnector();
					_Connector.EnsureRequiredSchema();
				}
				return _Connector;
			}
		}
	}
}
