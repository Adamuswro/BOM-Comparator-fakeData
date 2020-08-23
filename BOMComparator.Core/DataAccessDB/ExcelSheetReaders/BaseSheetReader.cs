using BOMComparator.Core.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Text.RegularExpressions;

namespace BOMComparator.Core.DataAccessDB
{
    internal class BaseSheetReader
    {
        private XSSFFormulaEvaluator formula;

        public BaseSheetReader(IWorkbook wb)
        {
            formula = new XSSFFormulaEvaluator(wb);
        }

        public BaseSheetReader()
        {
            formula = new XSSFFormulaEvaluator(new XSSFWorkbook());
        }

        private uint? LoadCellAsUint(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }
            uint result;
            formula.EvaluateInCell(cell);

            switch (cell.CellType)
            {
                case NPOI.SS.UserModel.CellType.Numeric:
                    if (cell.NumericCellValue > 0)
                        result = Convert.ToUInt32(cell.NumericCellValue);
                    else
                        return null;
                    break;
                case NPOI.SS.UserModel.CellType.String:
                    if (UInt32.TryParse(cell.StringCellValue, out result))
                        result = Convert.ToUInt32(cell.StringCellValue);
                    else
                        return null;
                    break;
                default:
                    return null;
            }

            return result;
        }

        private string LoadCellAsString(ICell cell)
        {
            if (cell == null)
            {
                return String.Empty;
            }
            string result = String.Empty;
            formula.EvaluateInCell(cell);

            switch (cell.CellType)
            {
                case NPOI.SS.UserModel.CellType.Numeric:
                    result = cell.NumericCellValue.ToString();
                    break;
                case NPOI.SS.UserModel.CellType.String:
                    result = cell.StringCellValue;
                    break;
            }

            return result;
        }

        // Input example: BMW 1499BluePerformance
        // Output: 1499
        internal uint? LoadDisplacement(string cellText)
        {
            string regexPattern = @"\s(\d+)";
            MatchCollection matches = Regex.Matches(cellText, regexPattern);
            switch (matches.Count)
            {
                case 1:
                    return (uint?)UInt32.Parse(matches[0].Groups[0].Value);
                default:
                    return null;
            }
        }

        // Input example: BMW 1499 BluePerformance
        // Output: BluePerformance
        internal string LoadMotorType(string cellText)
        {
            if (String.IsNullOrEmpty(cellText))
                return String.Empty;

            string regexPattern = @"([a-zA-Z]+)?\s?[a-zA-Z]+$";
            MatchCollection matches = Regex.Matches(cellText, regexPattern);

            switch (matches.Count)
            {
                case 0:
                    return "";
                case 1:
                    var result = matches[0].Groups[0].Value.Trim();
                    return result;
                default:
                    return "unknown";
            }
        }

        // Input example: BMW 1499BluePerformance
        // Output: BMW
        public MotorFamily LoadMotorFamily(string cellText)
        {
            string regexPattern = @"^([a-zA-Z]{2,}\s?[X]?)\s";
            MatchCollection matches = Regex.Matches(cellText, regexPattern);

            if (matches.Count != 1)
                return MotorFamily.Unknow;

            //Replace all white spaces from result (OMP X == OMPX)
            string motorFamilyName = Regex.Replace(matches[0].Groups[0].Value, @"\s+", "");

            if (Enum.TryParse(motorFamilyName, out MotorFamily result) == false)
            {
                return MotorFamily.Unknow;
            }

            return result;
        }

        protected string LoadDescription(ICell cell)
        {
            return LoadCellAsString(cell);
        }

        protected string LoadPartNumber(ICell cell)
        {
            var partNumberText = LoadCellAsString(cell);

            if (!MotorValidator.IsPartNumber(partNumberText))
                return null;
            else
                return partNumberText;
        }

        protected string LoadPositionNumber(ICell cell)
        {
            var positionNumber = LoadCellAsString(cell);

            if (positionNumber == null)
                return null;
            else
                return positionNumber;
        }

        protected uint LoadQuantity(ICell cell)
        {
            var quantity = LoadCellAsUint(cell);

            if (quantity == null)
                return 0;
            else
                return Convert.ToUInt32(quantity);
        }

        protected string LoadPartDesignation(ICell cell)
        {
            return LoadCellAsString(cell);
        }
    }
}