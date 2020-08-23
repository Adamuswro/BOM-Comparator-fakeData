using BOMComparator.Core.Models;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;

namespace BOMComparator.Core.DataAccessDB
{
    public interface ISheetReader
    {
        XSSFWorkbook Workbook { get; }
        string FormatDescription { get; }
        string SheetName { get; }

        IEnumerable<Motor> ReadSheetAndConvertForMotors();
    }
}