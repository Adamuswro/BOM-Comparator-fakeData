using BOMComparator.Core.DataAccessDB.Factories;
using BOMComparator.Core.DataAccessDB.ModelsRecords;
using BOMComparator.Core.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BOMComparator.Core.DataAccessDB
{
    public class DataAccessByDatabase : IDataAccessDB
    {
        public IEnumerable<Motor> GetAllMotors()
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionStrHelper.CnnVal("TestDB")))
            {
                var lookupMotors = new Dictionary<string, MotorRecord>();
                var lookupParts = new Dictionary<string, Part>();
                var allMotorsWithBomItems = connection.Query<MotorRecord, BOMItemRecord, Part, MotorRecord>
                    (@"select m.*, bi.*, p.* from Motors m
                    inner join BOMItems bi on m.MotorNumber = bi.MotorNumber
                    inner join Parts p on bi.PartNumber = p.PartNumber",
                    (m, bi, p) =>
                    {
                        MotorRecord motor;
                        Part part;
                        if (!lookupMotors.TryGetValue(m.MotorNumber, out motor))
                        {
                            lookupMotors.Add(m.MotorNumber, motor = m);
                        }
                        if (!lookupParts.TryGetValue(p.PartNumber, out part))
                        {
                            lookupParts.Add(p.PartNumber, part = p);
                        }
                        if (motor.BOM == null)
                        {
                            motor.BOM = new List<BOMItemRecord>();
                        }
                        bi.PartItem = part;
                        motor.BOM.Add(bi);
                        return motor;
                    }, splitOn: "MotorNumber,PartNumber").AsQueryable();
                var resultInRecords = lookupMotors.Values;

                var result = MotorFactory.BulidMotor(resultInRecords);

                return result;
            }

        }
        public void InsertMotors(IEnumerable<Motor> motors)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionStrHelper.CnnVal("TestDB")))
            {
                string motorSql = $@"insert into dbo.Motors (MotorNumber, MotorType, Displacement, Feature, Description) 
                                    values (@MotorNumber, @MotorType, @Displacement, @Feature, @Description)";
                string partsSql = $@"insert into dbo.Parts (PartNumber, Designation, Description) 
                                    values (@PartNumber, @Designation, @Description)";
                string BomItemSql = $@"insert into dbo.BOMItems(MotorNumber, PartNumber, PositionNumber, Quantity) 
                                    values (@MotorNumber, @PartNumber, @PositionNumber, @Quantity)";

                connection.Execute(motorSql, MotorFactory.BulidMotorRecordList(motors));

                var partsToInsert = motors.SelectMany(m => m.BOM).Select(b => b.PartItem).Distinct();
                connection.Execute(partsSql, partsToInsert);

                var bomItemsToInsert = CreateDynItemsListFromBomItemsInMotor(motors.ToList());
                connection.Execute(BomItemSql, bomItemsToInsert);
            }
        }
        private List<DynamicParameters> CreateDynItemsListFromBomItemsInMotor(IList<Motor> motors)
        {
            if (motors == null || motors.Count() == 0)
                return null;

            var bomItems = new List<DynamicParameters>();
            for (var i = 0; i < motors.Count(); i++)
            {
                var motor = motors[i];
                for (var j = 0; j < motor.BOM.Count(); j++)
                {
                    var p = new DynamicParameters();
                    p.Add("@MotorNumber", motor.MotorNumber, DbType.String, ParameterDirection.Input);
                    p.Add("@PartNumber", motor.BOM[j].PartItem.PartNumber, DbType.String, ParameterDirection.Input);
                    p.Add("@PositionNumber", Convert.ToInt32(motor.BOM[j].PositionNumber), DbType.Int32, ParameterDirection.Input);
                    p.Add("@Quantity", Convert.ToInt32(motor.BOM[j].Quantity), DbType.Int32, ParameterDirection.Input);

                    bomItems.Add(p);
                }
            }
            return bomItems;
        }
        public void InsertMotor(Motor motor)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionStrHelper.CnnVal("TestDB")))
            {
                string motorSql = $@"insert into dbo.Motors (MotorNumber, MotorType, Displacement, Feature, Description) 
                                    values (@MotorNumber, @MotorType, @Displacement, @Feature, @Description)";
                string partsSql = $@"insert into dbo.Parts (PartNumber, Designation, Description) 
                                    values (@PartNumber, @Designation, @Description)";
                string BomItemSql = $@"insert into dbo.BOMItems(MotorNumber, PartNumber, PositionNumber, Quantity) 
                                    values (@MotorNumber, @PartNumber, @PositionNumber, @Quantity)";

                connection.Execute(motorSql, MotorFactory.BulidMotorRecord(motor));

                var partsToInsert = motor.BOM.Select(b => b.PartItem).Distinct();
                connection.Execute(partsSql, partsToInsert);

                var bomItems = CreateDynItemsListFromBomItemsInMotor(new List<Motor> { motor });
                connection.Execute(BomItemSql, bomItems);
            }
        }

        public Motor GetMotorByMotorNumber(string motorNumber)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionStrHelper.CnnVal("TestDB")))
            {
                var queryResult = connection.Query<MotorRecord, BOMItemRecord, Part, MotorRecord>
                    (@"select m.*, bi.*, p.* from Motors m
                    inner join BOMItems bi on m.MotorNumber = bi.MotorNumber
                    inner join Parts p on bi.PartNumber = p.PartNumber
                    where m.MotorNumber = @motorNumber",
                    (m, bi, p) =>
                    {
                        if (m.BOM == null)
                        {
                            m.BOM = new List<BOMItemRecord>();
                        }
                        m.BOM.Add(bi);
                        return m;
                    }, splitOn: "MotorNumber,PartNumber").AsQueryable();

                var result = MotorFactory.BulidMotor(queryResult.FirstOrDefault(), queryResult.FirstOrDefault().BOM, queryResult.FirstOrDefault().BOM.Select(bi => bi.PartItem));

                return result;
            }
        }

        public void UpdateMotors(IEnumerable<Motor> motorsToUpdate)
        {
            throw new NotImplementedException("UpdateMotors in DataAccessByDatabase");
        }

        public void UpdateMotor(Motor motorToUpdate)
        {
            throw new NotImplementedException("UpdateMotor in DataAccessByDatabase");
        }
    }
}