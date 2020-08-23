using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.Core.Models
{
    public abstract class BOMAnalizeHelper
    {
        public static List<uint> GetAllPartPositionNumbers(Part part, IEnumerable<Motor> usedInMotors)
        {
            if (part == null || usedInMotors == null)
            {
                return new List<uint>();
            }

            var result = usedInMotors
                .SelectMany(b => b.BOM)
                .Where(p => p.PartItem.PartNumber == part.PartNumber)
                .Select(p => p.PositionNumber)
                .Distinct()
                .ToList();

            return result;
        }
        public static IEnumerable<Motor> GetOnlyUniqeMotors(IEnumerable<Motor> source, IEnumerable<Motor> toCompare)
        {
            var result = toCompare.Where(m => !source.Any(s => s.MotorNumber.Contains(m.MotorNumber)));
            return result;
        }
        public static IEnumerable<Motor> GetMotorsToUpdate(IEnumerable<Motor> source, IEnumerable<Motor> toCompare)
        {
            throw new NotImplementedException();
        }
    }
}
