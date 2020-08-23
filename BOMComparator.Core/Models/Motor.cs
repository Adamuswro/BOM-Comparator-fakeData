using System;
using System.Collections.Generic;

namespace BOMComparator.Core.Models
{
    public class Motor
    {
        public string MotorNumber { get; private set; }
        public MotorFamily MotorType { get; private set; }
        public uint? Displacement { get; private set; }
        public string Feature { get; private set; }
        public string Description { get; private set; }
        public List<BOMItem> BOM { get; } = new List<BOMItem>();
        public string FullDescription
        {
            get
            {
                return $"{MotorNumber} {MotorType.ToString()} {Displacement.ToString()}";
            }
        }
        public string MotorTypeTxt => MotorType.ToString();

        public Motor(string motorNumber)
        {
            if (motorNumber == null)
                throw new ArgumentNullException($"Motor creation not possible. Motor number not passed.");
            if (!MotorValidator.IsPartNumber(motorNumber))
                throw new ArgumentException($"Motor creation not possible. Motor number {motorNumber} is not correct.");
            MotorNumber = motorNumber;
        }

        public Motor(string motorNumber, MotorFamily motorType, uint? displacement, string feature, string description)
        {
            MotorNumber = motorNumber;
            MotorType = motorType;
            Displacement = displacement;
            Feature = feature;
            Description = description;
        }

        public void AddPart(Part newPart, uint positionNumber, uint quantity)
        {
            BOM.Add(new BOMItem(newPart, positionNumber, quantity));
        }

        public void RemoveAllParts()
        {
            for (int i = 0; i < BOM.Count; i++)
            {
                var partToRemove = BOM[i].PartItem;
                BOM.RemoveAt(0);
            }
            BOM.Clear();
        }
    }
}
