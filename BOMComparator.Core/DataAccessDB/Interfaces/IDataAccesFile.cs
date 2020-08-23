using BOMComparator.Core.Models;
using System.Collections.Generic;

namespace BOMComparator.Core.DataAccessDB
{
    internal interface IDataAccessFile
    {
        IEnumerable<Motor> GetAllMotors(string filePath);
    }
}
