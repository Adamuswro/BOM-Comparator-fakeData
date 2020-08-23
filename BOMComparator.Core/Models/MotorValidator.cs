using System;

namespace BOMComparator.Core.Models
{
    abstract public class MotorValidator
    {
        static public bool IsPartNumber(string partNumber)
        {
            if (String.IsNullOrEmpty(partNumber) || String.IsNullOrWhiteSpace(partNumber))
            {
                return false;
            }
            if (partNumber.Length != 8 && partNumber.Length != 9)
            {
                return false;
            }
            if (Char.IsLetter(partNumber[0]))
            {
                return false;
            }

            return true;
        }
        static public bool HasPart(Motor motor, string partNumber)
        {
            if (motor == null || partNumber == null)
                throw new ArgumentNullException();
            else if (!IsPartNumber(partNumber))
                throw new ArgumentException();

            return motor.BOM.Exists(p => p.PartItem.PartNumber == partNumber);
        }
    }
}
