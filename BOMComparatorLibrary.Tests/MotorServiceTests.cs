using BOMComparator.Core.DataAccessDB;
using BOMComparator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BOMComparatorLibrary.Tests
{
    public class MotorServiceTests
    {
        [Fact]
        public void FilterBy_ShouldReturnMotorsFilteredByFamilies()
        {
            var motorFamilies = new List<MotorFamily>() { MotorFamily.Toyota };
            var expectedMotors = new List<Motor>() { motorsExamples1[4] };

            var motorService = new MotorService(new DataAccessByMemoryCache());
            var result = motorService.MotorsFilterBy(motorsExamples1, motorFamilies, GetAllDisplacements(), GetAllFeatures());

            Assert.Equal(expectedMotors.OrderBy(m => m.MotorNumber), result.OrderBy(m => m.MotorNumber));
        }

        [Fact]
        public void FilterBy_ShouldReturnMotorsFilteredByDisplacements()
        {
            var displacements = new List<uint?>() { 200, 250 };
            var expectedMotors = new List<Motor>() { motorsExamples1[0], motorsExamples1[1], motorsExamples1[4], motorsExamples1[6], motorsExamples1[7] };

            var motorService = new MotorService(new DataAccessByMemoryCache());
            var result = motorService.MotorsFilterBy(motorsExamples1, GetAllFamilies(), displacements, GetAllFeatures());

            Assert.Equal(expectedMotors.OrderBy(m => m.MotorNumber), result.OrderBy(m => m.MotorNumber));
        }

        [Fact]
        public void FilterBy_ShouldReturnMotorsFilteredByFeatures()
        {
            var features = new List<string>() { "F", "" };
            var expectedMotors = new List<Motor>() { motorsExamples1[1], motorsExamples1[2] };

            var motorService = new MotorService(new DataAccessByMemoryCache());
            var result = motorService.MotorsFilterBy(motorsExamples1, GetAllFamilies(), GetAllDisplacements(), features);

            Assert.Equal(expectedMotors.OrderBy(m => m.MotorNumber), result.OrderBy(m => m.MotorNumber));
        }

        [Fact]
        public void PartsFilterBy_ShouldReturnPartsFilteredByDescriptionsAndDesignations()
        {
            var designations = new List<string>() { "SHAFT" };
            var descriptions = new List<string>() { "Conical" };
            var expectedParts = new List<Part>() { partsExamples[0], partsExamples[5] };

            var motorService = new MotorService(new DataAccessByMemoryCache());
            var result = motorService.PartsFilterBy(partsExamples, designations, descriptions);

            Assert.Equal(expectedParts.OrderBy(m => m.PartNumber), result.OrderBy(m => m.PartNumber));
        }

        [Fact]
        public void PartsFilterByDesignations_ShouldReturnCorrectResult()
        {
            var designations = new List<string>() { "NUT" };
            var expectedParts = new List<Part>() { partsExamples[2], partsExamples[3] };

            var motorService = new MotorService(new DataAccessByMemoryCache());
            var result = motorService.PartsFilterByDesignations(partsExamples, designations);

            Assert.Equal(expectedParts.OrderBy(m => m.PartNumber), result.OrderBy(m => m.PartNumber));
        }

        [Fact]
        public void PartsFilterByDesignations_ShouldReturnAllRecordsIfDesignationIsNullO()
        {
            var expectedParts = partsExamples;

            var motorService = new MotorService(new DataAccessByMemoryCache());
            var expected = motorService.PartsFilterByDesignations(partsExamples, null);

            Assert.Equal(expectedParts.OrderBy(m => m.PartNumber), expected.OrderBy(m => m.PartNumber));
        }

        [Fact]
        public void FilterBy_ShouldReturnEmptyCollectionIfNothingFound()
        {
            var features = new List<string>() { "Not existing feature" };
            var displacements = new List<uint?>() { 404 };
            var families = new List<MotorFamily>() { MotorFamily.VOLVO };

            var motorService = new MotorService(new DataAccessByMemoryCache());
            var result = motorService.MotorsFilterBy(motorsExamples1, families, displacements, features);

            Assert.Empty(result);
            Assert.NotNull(result);
        }

        [Fact]
        public void FilterBy_ShouldReturnEmptyCollectionIfParametersAreEmptyCollections()
        {
            List<MotorFamily> families = new List<MotorFamily>();
            List<uint?> displacements = new List<uint?>();
            List<string> features = new List<string>();

            var motorService = new MotorService(new DataAccessByMemoryCache());
            var result = motorService.MotorsFilterBy(motorsExamples1, families, displacements, features);

            Assert.Empty(result);
        }

        [Fact]
        public void FilterBy_ShouldThrowExceptionIfParametersAreNulls()
        {
            IEnumerable<MotorFamily> families = null;
            IEnumerable<uint?> displacements = null;
            IEnumerable<string> features = null;

            var motorService = new MotorService(new DataAccessByMemoryCache());

            Assert.Throws<ArgumentNullException>(() => motorService.MotorsFilterBy(motorsExamples1, families, displacements, features));
        }

        [Fact]
        public void FindSimilar_ShouldFindTwoResults()
        {
            List<Motor> allMotors = new List<Motor>() { motorsExamples1[0], motorsExamples1[1], motorsExamples1[2] };
            Motor compared = motorsExamples2[0];

            //TODO: Refactor the data initiation - should be common list of motors with BOM and second list should be it's depp-copy with changed bom afterwards 
            allMotors[0].AddPart(partsExamples[0], 1, 1);
            allMotors[0].AddPart(partsExamples[1], 2, 1);
            allMotors[0].AddPart(partsExamples[2], 2, 1);
            allMotors[1].AddPart(partsExamples[3], 3, 1);
            allMotors[1].AddPart(partsExamples[4], 4, 1);
            allMotors[1].AddPart(partsExamples[5], 5, 1);
            allMotors[2].AddPart(partsExamples[0], 3, 1);
            allMotors[2].AddPart(partsExamples[2], 4, 1);
            allMotors[2].AddPart(partsExamples[4], 5, 1);

            compared.AddPart(partsExamples[0], 1, 1);
            compared.AddPart(partsExamples[1], 2, 1);
            compared.AddPart(partsExamples[3], 2, 1);

            List<KeyValuePair<int, Motor>> expected = new List<KeyValuePair<int, Motor>>();
            expected.Add(new KeyValuePair<int, Motor>(1, allMotors[0]));
            expected.Add(new KeyValuePair<int, Motor>(2, allMotors[1]));
            expected.Add(new KeyValuePair<int, Motor>(2, allMotors[2]));

            MotorService motorService = new MotorService(new DataAccessByMemoryCache());

            List<KeyValuePair<int, Motor>> actual = motorService.FindSimilar(compared, allMotors);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FilterBy_ShouldReturnMotorsFilteredByAllParameters()
        {
            var motorFamilies = new List<MotorFamily>() { MotorFamily.VOLVO, MotorFamily.Ford };
            var features = new List<string>() { "F", "", "H", "EMD" };
            var displacements = new List<uint?>() { 120, 200, 250 };

            var expectedMotors = new List<Motor>() { motorsExamples1[0], motorsExamples1[3], motorsExamples1[7] };

            var motorService = new MotorService(new DataAccessByMemoryCache());
            var result = motorService.MotorsFilterBy(motorsExamples1, motorFamilies, GetAllDisplacements(), GetAllFeatures());

            Assert.Equal(expectedMotors.OrderBy(m => m.MotorNumber), result.OrderBy(m => m.MotorNumber));
        }

        #region TestData
        private IEnumerable<MotorFamily> GetAllFamilies() => motorsExamples1.Select(m => m.MotorType);
        private IEnumerable<uint?> GetAllDisplacements() => motorsExamples1.Select(m => m.Displacement);
        private IEnumerable<string> GetAllFeatures() => motorsExamples1.Select(m => m.Feature);

        private List<Motor> motorsExamples1 = new List<Motor>()
            {
                new Motor("12345678",MotorFamily.VOLVO,200,"EMD",""),
                new Motor("12345679",MotorFamily.BMW,250,"",""),
                new Motor("12345676",MotorFamily.BMW,300,"F",""),
                new Motor("12345675",MotorFamily.VOLVO,120,"EMD",""),
                new Motor("12345674",MotorFamily.Toyota,200,"H",""),
                new Motor("12345674",MotorFamily.Opel,120,"H",""),
                new Motor("12345674",MotorFamily.Opel,200,"EMD",""),
                new Motor("12345674",MotorFamily.Ford,200,"H",""),
            };

        private List<Motor> motorsExamples2 = new List<Motor>()
            {
                new Motor("12345611",MotorFamily.VOLVO,200,"EMD",""),
                new Motor("12345612",MotorFamily.Ford,250,"",""),
                new Motor("12345613",MotorFamily.VOLVO,300,"F",""),
                new Motor("12345614",MotorFamily.VOLVO,120,"EMD",""),
                new Motor("12345615",MotorFamily.Ford,200,"H",""),
                new Motor("12345616",MotorFamily.BMW,120,"H",""),
                new Motor("12345617",MotorFamily.BMW,200,"EMD",""),
                new Motor("12345618",MotorFamily.Ford,200,"H",""),
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
