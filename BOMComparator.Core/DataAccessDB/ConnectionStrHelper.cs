using System.Configuration;

namespace BOMComparator.Core.DataAccessDB
{
    public static class ConnectionStrHelper
    {
        public static string CnnVal(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
