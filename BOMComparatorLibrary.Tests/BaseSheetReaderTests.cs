using Xunit;
using BOMComparator.Core.DataAccessDB;

namespace BOMComparatorLibrary.Tests
{
    public class BaseSheetReaderTests
    {
        [Theory]
        [InlineData("VOLVO X 50 N", "N")]
        [InlineData("VOLVO 100 EMD", "EMD")]
        [InlineData("VOLVO 100", "")]
        [InlineData("VOLVO 160 11 L EMD", "L EMD")]
        [InlineData("VOLVO X 160 LL EMD", "LL EMD")]
        [InlineData("VOLVO 160F", "F")]
        public void LoadMotorType_ShouldReturnCorrectResult(string cellText, string expected)
        {
            var reader = new BaseSheetReader();

            var actual = reader.LoadMotorType(cellText);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("VOLVO X 50 N", 50)]
        [InlineData("VOLVO 100 EMD", 100)]
        [InlineData("VOLVO 100", 100)]
        [InlineData("VOLVO 160 11 L EMD", null)]
        [InlineData("VOLVO -160 EMD", null)]
        [InlineData("VOLVO 160F", 160)]
        [InlineData("VOLVO RDC", null)]
        [InlineData("VOLVO X 160 LL EMD", 160)]
        public void LoadDisplacement_ShouldReturnCorrectResult(string cellText, int? expected)
        {
            var reader = new BaseSheetReader();

            uint? actual = reader.LoadDisplacement(cellText);

            Assert.Equal((uint?)expected, actual);
        }

        [Theory]
        [InlineData("VOLVO 50 N", MotorFamily.VOLVO)]
        [InlineData("VOLVO 100 EMD", MotorFamily.VOLVO)]
        [InlineData("BMW 100", MotorFamily.BMW)]
        [InlineData("BMW 160 11 L EMD", MotorFamily.BMW)]
        [InlineData("Toyota -160 EMD", MotorFamily.Toyota)]
        [InlineData("Toyota 160F", MotorFamily.Toyota)]
        [InlineData("Toyota RDC", MotorFamily.Toyota)]
        public void LoadMotorFamily_ShouldReturnCorrectResult(string cellText, MotorFamily expected)
        {
            var reader = new BaseSheetReader();

            var actual = reader.LoadMotorFamily(cellText);

            Assert.Equal(expected, actual);
        }
    }
}
