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
        Real = 3,
        DateTime = 4,
        Location = 5,
        Image = 6,
        Bool = 7,
    }

    public static class FieldValueTypeExtentions {
        public static long ToInt(this FieldValueType fvt) {
            return (long)fvt;
        }
    }
}
