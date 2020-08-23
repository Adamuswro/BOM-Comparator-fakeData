using BOMComparator.Core.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.Core.DataAccessDB
{
    //TODO: This class is really messy. It should be Method Template Pattern implemented for base class. All validation logic should be moved to MotorValidator class or it should be created PartValidator class as well.
    internal class ExportTC7_5Reader : BaseSheetReader, ISheetReader
    {
        public string FormatDescription { get; } =
            "Files exported in Team Center 7.5. Motors P/N are stored in first column. Parts P/Ns are in first row.";

        #region Options
        private const int motorsColumn = 0;
        private const int motorsRow = 5;
        private const int motorsDesignationColumn = 1;
        private const int motorsDescriptionColumn = 2;
        private const int partsRow = 0;
        private const int partsColumn = 8; // 8 == I column
        private const int partsPositionNoRow = 1;
        private const int partsDesignationRow = 2;
        private const int partsDescriptionRow = 3;
        #endregion

        public string SheetName { get; } = "MOTORS_BOM";

        public XSSFWorkbook Workbook { get; }
        public XSSFFormulaEvaluator Formula { get; }

        public ExportTC7_5Reader(XSSFWorkbook workbook)
            : base(workbook)
        {
            Workbook = workbook;
            Formula = new XSSFFormulaEvaluator(Workbook);
        }

        public IEnumerable<Motor> ReadSheetAndConvertForMotors()
        {
            ISheet sheet = Workbook.GetSheet(SheetName);

            List<Motor> motors = new List<Motor>();
            List<Part> parts = new List<Part>();

            for (int i = motorsRow; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                string designationCellText = LoadCellAsString(row.GetCell(motorsDesignationColumn));
                string motorNumber = LoadPartNumber(row.GetCell(motorsColumn));
                if (motorNumber == null)
                {
                    continue;
                }
                uint? displacement = LoadDisplacement(designationCellText);
                string MotorType = LoadMotorType(designationCellText);
                MotorFamily motorFamily = LoadMotorFamily(designationCellText);
                string motorDescription = LoadCellAsString(row.GetCell(motorsDescriptionColumn));

                Motor newMotor = new Motor(motorNumber, motorFamily, displacement, MotorType, motorDescription);

                motors.Add(newMotor);

                for (int j = partsColumn; j <= row.LastCellNum; j++)
                {
                    var cell = row.GetCell(j);
                    if (cell == null)
                    {
                        continue;
                    }
                    else if (String.IsNullOrWhiteSpace(cell.ToString()))
                    {
                        continue;
                    }
                    string partNumber = LoadPartNumber(sheet.GetRow(partsRow).GetCell(j));
                    string positionNumberTxt = LoadPositionNumber(sheet.GetRow(partsPositionNoRow).GetCell(j));
                    string designation = LoadPartDesignation(sheet.GetRow(partsDesignationRow).GetCell(j));
                    string description = LoadDescription(sheet.GetRow(partsDescriptionRow).GetCell(j));
                    uint quantity = LoadQuantity(row.GetCell(j));

                    uint positionNumber = 0;

                    if (IsLawrencePart(positionNumberTxt))
                    {
                        continue;
                    }
                    else
                    {
                        UInt32.TryParse(positionNumberTxt, out positionNumber);
                    }
                    if (!IsRecordCorrect(partNumber, positionNumber))
                    {
                        newMotor.RemoveAllParts();
                        break;
                    }
                    else
                    {
                        //TODO: Instead of looking to the list it should be Dictionary
                        Part partToAdd = parts.FirstOrDefault(p => p.PartNumber.Equals(partNumber));
                        if (partToAdd == null)
                        {
                            parts.Add(partToAdd = new Part(partNumber, designation, description));
                        }
                        newMotor.AddPart(partToAdd, positionNumber, quantity);
                    }
                }
            }

            return motors;
        }

        private string LoadCellAsString(ICell cell)
        {
            if (cell == null)
            {
                return String.Empty;
            }
            string result = String.Empty;
            Formula.EvaluateInCell(cell);

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

        private bool IsLawrencePart(string positionNumberTxt)
        {
            //Lawrance Part will have got letter after position number: 21A, 22B, etc.
            if (String.IsNullOrWhiteSpace(positionNumberTxt) || positionNumberTxt.Length == 0)
            {
                return false;
            }

            char lastChar = positionNumberTxt[positionNumberTxt.Length - 1];
            string beforeLastChar = positionNumberTxt.Substring(0, positionNumberTxt.Length - 1);

            if (Char.IsLetter(lastChar) && Int32.TryParse(beforeLastChar, out int empty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsRecordCorrect(string partNumber, uint positionNumber)
        {
            if (partNumber != null && positionNumber != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
