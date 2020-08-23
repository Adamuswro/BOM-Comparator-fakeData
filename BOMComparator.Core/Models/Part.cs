using System;

namespace BOMComparator.Core.Models
{
    public class Part
    {
        private string _partNumber;
        public string PartNumber { get => _partNumber; set => _partNumber = value.Trim(); }
        public string Designation { get; }
        public string Description { get; private set; }
        public string FullDescription
        {
            get => $"{PartNumber} {Designation} {Description}";
        }
        public Part()
        {
        }
        public Part(string partNumber)
        {
            if (partNumber == null)
                throw new ArgumentNullException($"Part creation not possible. Part number not passed.");
            if (!MotorValidator.IsPartNumber(partNumber))
                throw new ArgumentException($"Part creation not possible. Part number {partNumber} is not correct.");
            PartNumber = partNumber;
        }
        public Part(string partNumber, string designation, string description)
        {
            PartNumber = partNumber;
            Designation = designation;
            Description = description;
        }

        public Part(string partNumber, string designation) : this(partNumber)
        {
            Designation = designation;
            Description = String.Empty;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Part;

            if (item == null)
            {
                return false;
            }

            return PartNumber.Equals(item.PartNumber);
        }

        public void ChangeDescription(string newDescription) => Description = newDescription;

        public override int GetHashCode() => this.PartNumber.GetHashCode();

        public override string ToString() => $"{PartNumber} {Designation}";
    }
}
