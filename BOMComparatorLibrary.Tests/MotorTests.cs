using BOMComparator.Core.Models;
using System.Collections.Generic;
using Xunit;

namespace BOMComparatorLibrary.Tests
{
    public class MotorTests
    {
        [Fact]
        public void RemoveAllParts_ShouldRemoveAllBOMItemsAndNotRemoveParts()
        {
            int expectedBOMItemCount = 0;
            int expectedPartsCount = 2;
            var motor = new Motor(motorNumber: "12345678");
            var parts = new List<Part>() { new Part(partNumber: "11111111"), new Part(partNumber: "22222222") };

            motor.RemoveAllParts();

            Assert.Equal(motor.BOM.Count, expectedBOMItemCount);
            Assert.Equal(parts.Count, expectedPartsCount);
        }

        [Fact]
        public void AddPart_shouldCreate2BOMItems()
        {
            int expectedBOMItemCount = 2;
            var motor = new Motor(motorNumber: "12345678");
            var parts = new List<Part>() { new Part(partNumber: "11111111"), new Part(partNumber: "22222222") };

            foreach (var part in parts)
            {
                motor.AddPart(part, 1, 1);
            }

            Assert.Equal(motor.BOM.Count, expectedBOMItemCount);
        }


    }
}
