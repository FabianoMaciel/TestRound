using System;
using System.Collections.Generic;
using System.Text;

namespace DataAcess
{
    public static class DBUtils
    {
        public enum TransactionReturn
        {
            Success = 0,
            ISINDuplicate = 1001,
            ISINNotExist = 1002,
            Unexpected = 1009
        }
    }
}
