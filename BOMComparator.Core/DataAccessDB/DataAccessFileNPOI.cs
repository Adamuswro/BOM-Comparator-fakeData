using BOMComparator.Core.Models;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace BOMComparator.Core.DataAccessDB
{
    public class DataAccessFileNPOI : IDataAccessFile
    {
        public ISheetReader _fileReader;
        public List<string> Logs { get; private set; } = new List<string>();

        public IEnumerable<Motor> GetAllMotors(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }
            if (!IsFileExtensionCorrect(filePath))
            {
                throw new FileFormatException(String.Join(Environment.NewLine, Logs));
            }

            var wb = LoadWorkbook(filePath);

            _fileReader = recognizeFileReader(wb) ?? throw new FileFormatException(Logs.ToString());

            var result = _fileReader.ReadSheetAndConvertForMotors();

            Logs.Add($"Files loaded from file {filePath}.");
            return result;
        }

        private ISheetReader recognizeFileReader(XSSFWorkbook wb)
        {
            if (IsExportTC7_5Format(wb))
            {
                return new ExportTC7_5Reader(wb);
            }

            if (IsCEWBFormat(wb))
            {
                return new CEWBReader(wb);
            }

            return null;
        }

        private bool IsCEWBFormat(XSSFWorkbook wb)
        {
            var sheetName = "Sheet1";
            List<string> Errors = new List<string>();
            var sheet = wb.GetSheet(sheetName);

            if (sheet == null)
            {
                Errors.Add($"Sheet {sheetName} not found.");
            }
            else
            {
                string materialText = String.Empty;
                try
                {
                    materialText = sheet.GetRow(0).GetCell(0).StringCellValue;
                }
                catch
                {
                    materialText = String.Empty;
                }
                finally
                {
                    if (!materialText.Equals("Material"))
                    {
                        Errors.Add("Cell A,1 do not contain 'Material' text");
                    }
                }
                string componentText = String.Empty;
                try
                {
                    componentText = sheet.GetRow(0).GetCell(1).StringCellValue;
                }
                catch
                {
                    componentText = String.Empty;
                }
                finally
                {
                    if (!componentText.Equals("Component"))
                    {
                        Errors.Add("Cell A,2 do not contain 'Component' text");
                    }
                }
            }

            if (Errors.Count == 0)
            {
                return true;
            }
            else
            {
                Logs.Add("File is not CEWB format." + String.Join(Environment.NewLine, Errors));
                return false;
            }
        }

        private bool IsExportTC7_5Format(XSSFWorkbook wb)
        {
            var sheetName = "MOTORS_BOM";
            List<string> Errors = new List<string>();

            var sheet = wb.GetSheet(sheetName);

            if (sheet == null)
            {
                Errors.Add("Sheet MOTORS_BOM not found. It's not Team Center 7.5 export format.");
            }
            else
            {
                var partNumberText = sheet.GetRow(4).GetCell(0).StringCellValue;
                if (!partNumberText.Equals("Part Number"))
                {
                    Errors.Add("Cell A,5 do not contain 'Part Number' text");
                }
                var partText = sheet.GetRow(0).GetCell(6).StringCellValue;
                if (!partText.Equals("Part"))
                {
                    Errors.Add("Cell G,1 do not contain 'Part' text");
                }
            }

            if (Errors.Count == 0)
            {
                return true;
            }
            else
            {
                Logs.Add("File is not export from TC 7.5." + String.Join(Environment.NewLine, Errors));
                return false;
            }
        }

        private bool IsFileExtensionCorrect(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath);
            if (!File.Exists(filePath))
            {
                Logs.Add("File Path is incorrect. File doesn't exist.");
                return false;
            }
            else if (fileExtension != ".xlsx" && fileExtension != ".xls")
            {
                Logs.Add("File must be type of '.xlsx' or '.xls'.");
                return false;
            }

            return true;
        }

        private XSSFWorkbook LoadWorkbook(string _filePath)
        {
            XSSFWorkbook xssfwb;
            using (FileStream file = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                xssfwb = new XSSFWorkbook(file);
            }
            return xssfwb;
        }
    }
}
