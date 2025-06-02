using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H5_GenericDataLogger.Data {
    public enum ValueType: long {
        Unknown = 0,
        String = 1,
        Integer = 2,
        Float = 3,
        DateTime = 4,
        Location = 5,
        Image = 6,
    }
}
