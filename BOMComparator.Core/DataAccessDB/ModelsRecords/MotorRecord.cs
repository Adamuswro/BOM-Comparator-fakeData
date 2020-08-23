using System.Collections.Generic;

namespace BOMComparator.Core.DataAccessDB.ModelsRecords
{
    internal class MotorRecord
    {
        public string MotorNumber { get; set; }
        public MotorFamily MotorType { get; set; }
        public int Displacement { get; set; }
        public string Feature { get; set; }
        public string Description { get; set; }
        public List<BOMItemRecord> BOM { get; set; }
    }
}
