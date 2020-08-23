using BOMComparator.Core.Models;
using BOMComparator.Core.TestsTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.Core.DataAccessDB
{
    internal class RandomDataForTests : IDataAccessDB
    {
        public uint NumberOfMotors { get; set; } = 120;
        public uint NumberOfParts { get; set; } = 100;
        public uint QuantityFactorMotorsToParts { get; set; } = 5;

        private List<Motor> _allMotors;

        public IEnumerable<Motor> GetAllMotors()
        {
            if (_allMotors != null)
            {
                return _allMotors;
            }

            var allParts = new List<Part>();

            for (int i = 0; i < NumberOfParts; i++)
            {
                allParts.Add(MotorRandomizer.RandomPart());
            }

            _allMotors = new List<Motor>();
            for (int i = 0; i < NumberOfMotors; i++)
            {
                _allMotors.Add(MotorRandomizer.CreateMotorWithPredefinedParts(allParts, allParts.Count / 3));
            }
            var duplicates = _allMotors.GroupBy(m => m.MotorNumber).Where(n => n.Count() > 1).Select(m => m.Key);
            _allMotors.RemoveAll(m => duplicates.Any(d => d == m.MotorNumber));

            return _allMotors;
        }

        public Motor GetMotorByMotorNumber(string motorNumber) => _allMotors.SingleOrDefault(m => m.MotorNumber == "motorNumber");

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
            var toInsert = BOMAnalizeHelper.GetOnlyUniqeMotors(_allMotors, motorsToInsert);
            _allMotors.AddRange(toInsert);
        }

        public void UpdateMotors(IEnumerable<Motor> motorsToUpdate)
        {
            var result = BOMAnalizeHelper.GetMotorsToUpdate(_allMotors, motorsToUpdate);
            throw new NotImplementedException("UpdateMotors in RandomDataForTests");
        }

        public void UpdateMotor(Motor motorToUpdate)
        {
            throw new NotImplementedException("UpdateMotors in RandomDataForTests");
        }
    }
}
