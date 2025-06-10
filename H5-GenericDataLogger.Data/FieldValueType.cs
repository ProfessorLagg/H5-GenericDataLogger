using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data {
    public enum FieldValueType: long {
        Unknown = 0,
        Text = 1,
        Integer = 2,
        Float = 3,
        DateTime = 4,
        Location = 5,
        Image = 6,
        Bool = 7,
        Blob = 8,
    }

    public static class FieldValueTypeExtentions {
        public static long ToInt(this FieldValueType fvt) {
            return (long)fvt;
        }

        public static string TableName(this FieldValueType fvt) {
            switch (fvt) {
                default: throw new ArgumentException(nameof(fvt));
                case FieldValueType.Text: return "text_log_values";
                case FieldValueType.Integer: return "integer_log_values";
                case FieldValueType.Float: return "real_log_values";
                case FieldValueType.DateTime: return "integer_log_values";
                case FieldValueType.Location: return "integer_log_values";
                case FieldValueType.Image: return "blobl_log_values";
                case FieldValueType.Bool: return "integer_log_values";
                case FieldValueType.Blob: return "blob_log_values";
            }
        }
    }
}
