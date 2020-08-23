using BOMComparator.Core.Models;
using System.Collections.Generic;

namespace BOMComparator.Core.DataAccessDB
{
    public interface IDataAccessDB
    {
        IEnumerable<Motor> GetAllMotors();
        Motor GetMotorByMotorNumber(string motorNumber);
        void InsertMotors(IEnumerable<Motor> motorsToInsert);
        void InsertMotor(Motor motorToInsert);
        void UpdateMotors(IEnumerable<Motor> motorsToUpdate);
        void UpdateMotor(Motor motorToUpdate);

    }
}
