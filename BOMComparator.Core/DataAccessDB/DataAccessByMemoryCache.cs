using BOMComparator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.Core.DataAccessDB
{
    public class DataAccessByMemoryCache : IDataAccessDB
    {
        private List<Motor> _allMotors = new List<Motor>();

        public IEnumerable<Motor> GetAllMotors() => _allMotors;

        public Motor GetMotorByMotorNumber(string motorNumber) => _allMotors.SingleOrDefault(m => m.MotorNumber.Equals(motorNumber));

        public void InsertMotor(Motor motorToInsert)
        {
            if (motorToInsert == null)
            {
                return;
            }
            _allMotors.Add(motorToInsert);
        }

        public void InsertMotors(IEnumerable<Motor> motorsToInsert)
        {
            if (motorsToInsert == null)
            {
                return;
            }
            foreach (var motor in motorsToInsert)
            {
                InsertMotor(motor);
            }
        }

        public void UpdateMotors(IEnumerable<Motor> motorsToUpdate)
        {
            if (motorsToUpdate == null || motorsToUpdate.Count() == 0)
            {
                return;
            }
            foreach (var motor in motorsToUpdate)
            {
                UpdateMotor(motor);
            }
        }

        public void UpdateMotor(Motor motorToUpdate)
        {
            if (motorToUpdate == null)
            {
                return;
            }
            var source = _allMotors.FirstOrDefault(m => m.MotorNumber.Equals(motorToUpdate.MotorNumber));
            if (source == null)
            {
                throw new NullReferenceException($"Motor {motorToUpdate.MotorNumber} not found in cache. There is not possible to update it.");
            }
            int index = _allMotors.IndexOf(source);
            _allMotors[index] = motorToUpdate;
        }
    }
}
