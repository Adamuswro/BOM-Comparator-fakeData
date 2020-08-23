using BOMComparator.Core.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.Core.DataAccessDB
{
    internal class CEWBReader : BaseSheetReader, ISheetReader
    {
        public CEWBReader(XSSFWorkbook Workbook)
            : base(Workbook)
        {
            this.Workbook = Workbook;
        }

        private List<Motor> motors = new List<Motor>();
        private List<Part> parts = new List<Part>();
        private int motorColumn = 0;
        private int partColumn = 1;
        private int partQuantityColumn = 2;
        private int partDesignationColumn = 3;

        public XSSFWorkbook Workbook { get; }

        public string FormatDescription { get; } = "CEWB export from SAP";

        public string SheetName { get; } = "Sheet1";

        public IEnumerable<Motor> ReadSheetAndConvertForMotors()
        {
            XSSFFormulaEvaluator formula = new XSSFFormulaEvaluator(Workbook);
            ISheet sheet = Workbook.GetSheet(SheetName);

            for (int i = 1; i < sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                string motorNumber = LoadPartNumber(row.GetCell(motorColumn));
                if (motorNumber == null)
                {
                    continue;
                }
                var currentMotor = GetMotorFromLoaded(motorNumber);
                if (currentMotor == null)
                {
                    //CEWB format is not containing all informations like motor description - that's why constructor looks like that.
                    currentMotor = new Motor(motorNumber, MotorFamily.Unknow, null, String.Empty, String.Empty);
                    motors.Add(currentMotor);
                }

                string partNumber = LoadPartNumber(row.GetCell(partColumn));
                string designation = LoadPartDesignation(row.GetCell(partDesignationColumn));
                uint quantity = LoadQuantity(row.GetCell(partQuantityColumn));
                if (partNumber == null)
                {
                    continue;
                }
                var partToAdd = GetPartFromLoaded(partNumber);
                if (partToAdd == null)
                {
                    partToAdd = new Part(partNumber, designation);
                    parts.Add(partToAdd);
                }
                currentMotor.AddPart(partToAdd, 0, quantity);
            }
            return motors;
        }

        private Part GetPartFromLoaded(string partNumber)
        {
            return parts.SingleOrDefault(p => p.PartNumber.Equals(partNumber));
        }

        private Motor GetMotorFromLoaded(string motorNumber)
        {
            return motors.SingleOrDefault(m => m.MotorNumber.Equals(motorNumber));
        }
    }
}
