using BOMComparator.ViewModels.Helpers;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BOMComparatorLibrary.Tests
{
    public class UserInputConverterTests
    {
        [Fact]
        public void SplitText_ShouldConvertMultipleUserInput()
        {
            string userInput = "test1,test2;test3\n\ttest4 \ttest5";
            var expectedResult = new List<string>() { "test1", "test2", "test3", "test4", "test5" };

            var result = UserInputConverter.SplitText(userInput).ToList();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void SplitText_ShouldConvertSingleUserInput()
        {
            string userInput = "test1 ";
            var expectedResult = new List<string>() { "test1" };

            var result = UserInputConverter.SplitText(userInput).ToList();

            Assert.Equal(expectedResult, result);
        }
    }
}
