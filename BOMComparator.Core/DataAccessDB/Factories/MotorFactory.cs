using BOMComparator.Core.DataAccessDB.ModelsRecords;
using BOMComparator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.Core.DataAccessDB.Factories
{
    internal abstract class MotorFactory
    {
        internal static Motor BulidMotor(MotorRecord motor, IEnumerable<BOMItemRecord> bomItemRecords, IEnumerable<Part> parts)
        {
            if (motor == null)
                return null;

            var newMotor = new Motor(motor.MotorNumber, motor.MotorType, Convert.ToUInt32(motor.Displacement), motor.Feature, motor.Description);
            foreach (var partItem in bomItemRecords)
            {
                var partToAdd = parts.First(p => p.PartNumber == partItem.PartItem.PartNumber);
                newMotor.AddPart(partToAdd, Convert.ToUInt32(partItem.PositionNumber), Convert.ToUInt32(partItem.Quantity));
            }
            return newMotor;
        }
        internal static IEnumerable<Motor> BulidMotor(IEnumerable<MotorRecord> motors)
        {
            if (motors == null)
                return null;
            var result = new List<Motor>();
            foreach (var motor in motors)
            {
                var newMotor = new Motor(
                    motorNumber: motor.MotorNumber,
                    motorType: motor.MotorType,
                    displacement: Convert.ToUInt32(motor.Displacement),
                    feature: motor.Feature,
                    description: motor.Description);
                for (int i = 0; i < motor.BOM.Count(); i++)
                {
                    var bomItem = motor.BOM[i];
                    newMotor.AddPart(bomItem.PartItem, bomItem.PositionNumber, bomItem.Quantity);
                }
                result.Add(newMotor);
            }
            return result;
        }
        internal static IEnumerable<MotorRecord> BulidMotorRecordList(IEnumerable<Motor> motorList)
        {
            var result = new List<MotorRecord>();
            foreach (var motor in motorList)
            {
                result.Add(BulidMotorRecord(motor));
            }

            return result;
        }

        internal static MotorRecord BulidMotorRecord(Motor motor)
        {
            var result = new MotorRecord
            {
                MotorNumber = motor.MotorNumber,
                Description = motor.Description,
                Displacement = Convert.ToInt32(motor.Displacement),
                Feature = motor.Feature,
                MotorType = motor.MotorType
            };
            return result;
        }
    }
}
