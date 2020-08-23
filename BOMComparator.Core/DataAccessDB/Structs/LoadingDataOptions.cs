namespace BOMComparator.Core.DataAccessDB
{
    internal struct LoadingDataOptions
    {
        public readonly uint RowFirstMotor;
        public readonly uint ColumnFirstPart;
        public readonly uint ColumnMotors;
        public readonly uint ColumnMotorsDesignation;
        public readonly uint ColumnMotorsDescription;
        public readonly uint RowParts;
        public readonly uint RowPartDesignationts;
        public readonly uint RowPartDescription;

        public LoadingDataOptions(uint rowWithFirstMotor,
                              uint columnWithFirstPart,
                              uint columnWithMotors, uint rowParts,
                              uint rowPartDesignation, uint rowPartDescription)
        {
            RowFirstMotor = rowWithFirstMotor;
            ColumnFirstPart = columnWithFirstPart;
            ColumnMotors = columnWithMotors;
            ColumnMotorsDesignation = ColumnMotors + 1;
            ColumnMotorsDescription = ColumnMotorsDesignation + 1;
            RowParts = rowParts;
            RowPartDesignationts = rowPartDesignation;
            RowPartDescription = rowPartDescription;
        }
    }
}
