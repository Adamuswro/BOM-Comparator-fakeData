using BOMComparator.Core.Models;
using System.Collections.Generic;
using Xunit;

namespace BOMComparatorLibrary.Tests
{
    public class BOMAnalizeHelperTests
    {
        [Fact]
        public void GetAllPartPositionNumbers_ShouldReturnEmptyListIfInputIsNull()
        {
            var actual1 = BOMAnalizeHelper.GetAllPartPositionNumbers(null, motorsExamples);
            var actual2 = BOMAnalizeHelper.GetAllPartPositionNumbers(partsExamples[0], null);

            Assert.Empty(actual1);
            Assert.Empty(actual2);
        }

        [Fact]
        public void GetAllPartPositionNumbers_ShouldReturnEmptyListIfThereIsNoRelation()
        {
            var motors = new List<Motor>(motorsExamples);
            Part part = new Part("11111111");

            var actual = BOMAnalizeHelper.GetAllPartPositionNumbers(part, motors);

            Assert.Empty(actual);
        }

        [Fact]
        public void GetAllPartPositionNumbers_ShouldReturnCorrectResult()
        {
            List<uint> expected = new List<uint>() { 1, 2, 12 };

            var motors = new List<Motor>(motorsExamples);
            Part part = new Part("11111111");
            motors[0].AddPart(part, expected[0], 1);
            motors[2].AddPart(part, expected[1], 2);
            motors[3].AddPart(part, expected[2], 2);

            var actual = BOMAnalizeHelper.GetAllPartPositionNumbers(part, motors);

            Assert.Equal(actual, expected);
        }

        #region TestData
        private List<Motor> motorsExamples = new List<Motor>()
            {
                new Motor("12345678",MotorFamily.VOLVO,200,"EMD",""),
                new Motor("12345679",MotorFamily.VOLVO,250,"",""),
                new Motor("12345676",MotorFamily.VOLVO,300,"F",""),
                new Motor("12345675",MotorFamily.VOLVO,120,"EMD",""),
                new Motor("12345674",MotorFamily.VOLVO,200,"H",""),
                new Motor("12345674",MotorFamily.VOLVO,120,"H",""),
                new Motor("12345674",MotorFamily.VOLVO,200,"EMD",""),
                new Motor("12345674",MotorFamily.VOLVO,200,"H",""),
            };

        private List<Part> partsExamples = new List<Part>()
        {
            new Part("87654321", "SHAFT", "Conical with Keyway"),
            new Part("87654322", "SHAFT", "Spline T13"),
            new Part("87654323", "NUT", "M12x1,5 "),
            new Part("87654324", "NUT", "M10"),
            new Part("87654325", "SPRING", "L=120"),
            new Part("87654326", "SHAFT", "Conical")
        };
        #endregion
    }
}
