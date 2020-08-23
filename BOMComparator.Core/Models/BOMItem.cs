using System;

namespace BOMComparator.Core.Models
{
    public class BOMItem
    {
        public Part PartItem { get; private set; }
        public uint PositionNumber { get; private set; }
        public uint Quantity { get; private set; }

        internal BOMItem(Part part, uint positionNumber, uint quantity)
        {
            PartItem = part ?? throw new ArgumentNullException(nameof(part));
            PositionNumber = positionNumber;
            Quantity = quantity;
        }

        internal void AddPart(Part part)
        {
            if (PartItem != null)
                PartItem = part;
        }
    }
}
