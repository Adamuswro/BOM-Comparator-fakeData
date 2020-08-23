using BOMComparator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.Core.TestsTools
{
    static public class MotorRandomizer
    {
        private static readonly Random random = new Random();
        private static List<Part> _allParts = null;
        public static List<Part> AllParts
        {
            get
            {
                if (_allParts == null)
                {
                    _allParts = new List<Part>();
                    int numberOfParts = 400;
                    for (int i = 0; i < numberOfParts; i++)
                        _allParts.Add(RandomPart());
                }
                return _allParts;
            }
        }
        public static string[] designations = new string[] { "SCREW", "HOUSING", "NUT", "SHAFT", "PLUG", "END COVER", "PILOT FLANGE", "WASHER", "CARDAN SHAFT", "CASTE NUT",
        "SPRING", "WASHER", "SPOOL", "FLANGE", "SENSOR", "PLASTIC PLUG", "CABLE", "GEAR SET", "GEAR", "VALVE", "CHECK VALVE", "SHOCK VALVE", "SPOOL SHAFT", "SCREW"};
        public static string[] descriptions = new string[] { "L=15", "Old design", "New design", "Prototype", "test", "blah blah", "32231235123", "Something" };

        static public Motor CreateMotor(MotorFamily family, int numberOfParts)
        {
            Motor result = new Motor
                (RandomPartNumber(),
                family,
                RandomDisplacement(),
                RandomFeature(),
                RandomDescription()
                );

            for (int i = 0; i < numberOfParts; i++)
                result.AddPart(AllParts[random.Next(0, AllParts.Count - 1)], RandomPositionNumber(), RandomQuantity());

            return result;
        }

        private static uint RandomPositionNumber() => (uint)random.Next(1, 80);
        private static uint RandomQuantity() => (uint)random.Next(1, 10);

        static public Motor CreateMotor(MotorFamily family)
        {
            return CreateMotor(family, RandomNumberOfParts());
        }
        static public Motor CreateMotor(int numberOfParts)
        {
            return CreateMotor(RandomMotorFamily(), numberOfParts);
        }
        static public Motor CreateMotor()
        {
            return CreateMotor(RandomMotorFamily(), RandomNumberOfParts());
        }
        static public Motor CreateMotorWithPredefinedParts(List<Part> parts, int maxNumberOfPartsInBOM, int minNumberOfPartsInBOM = 1)
        {
            if (parts == null)
            {
                parts = new List<Part>();
            }

            Motor result = new Motor
                (RandomPartNumber(),
                RandomMotorFamily(),
                RandomDisplacement(),
                RandomFeature(),
                RandomDescription()
                );

            var numberOfPartsInBom = random.Next(minNumberOfPartsInBOM, maxNumberOfPartsInBOM);
            if (numberOfPartsInBom == 0 && parts.Count() != 0)
            {
                numberOfPartsInBom = 1;
            }
            for (int i = 0; i < numberOfPartsInBom; i++)
            {
                var index = random.Next(0, parts.Count());
                result.AddPart(
                    parts.ElementAt(index),
                    RandomPositionNumber(),
                    RandomQuantity());
            }

            return result;
        }

        private static MotorFamily RandomMotorFamily()
        {
            int familiesCount = Enum.GetNames(typeof(MotorFamily)).Length;
            return (MotorFamily)random.Next(0, familiesCount - 1);
        }

        static private string RandomFeature()
        {
            return "Random feature #" + random.Next();
        }
        static private string RandomDescription()
        {
            return "Random description #" + random.Next();
        }
        static private int RandomNumberOfParts()
        {
            return random.Next(5, 15);
        }

        static public Part RandomPart()
        {
            Part result = new Part(
                partNumber: RandomPartNumber(),
                designation: designations[random.Next(0, designations.Length - 1)],
                description: descriptions[random.Next(0, descriptions.Length - 1)]);
            return result;
        }
        static public string RandomDesignation()
        {
            return designations[random.Next(designations.Length - 1)];
        }

        static public uint RandomDisplacement()
        {
            List<uint> allDisplacements = new List<uint>()
            {
                32,80,100,160,250,315,500,615
            };
            return allDisplacements[random.Next(allDisplacements.Count - 1)];
        }
        static public string RandomPartNumber()
        {
            return "1" + RandomDigits(7);
        }
        static private string RandomDigits(int length)
        {
            string s = String.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}
