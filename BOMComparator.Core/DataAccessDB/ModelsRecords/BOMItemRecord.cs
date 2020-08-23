using BOMComparator.Core.Models;

namespace BOMComparator.Core.DataAccessDB.ModelsRecords
{
    internal class BOMItemRecord
    {
        public uint PositionNumber { get; set; }
        public uint Quantity { get; set; }
        public Part PartItem { get; set; }
    }
}
