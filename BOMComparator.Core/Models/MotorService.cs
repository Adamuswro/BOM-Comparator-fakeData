using BOMComparator.Core.DataAccessDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BOMComparator.Core.Models
{
    public class MotorService : IMotorService
    {
        private IDataAccessDB _dataAccessDB;
        private IDataAccessFile _dataAccessFile = new DataAccessFileNPOI();

        public delegate void Notify();
        public event Notify DatabaseUpdatedEventHandler;

        public IEnumerable<Motor> AllMotors
        {
            get
            {
                return _dataAccessDB.GetAllMotors();
            }
        }
        public IEnumerable<Part> AllParts { get => AllMotors.SelectMany(p => p.BOM).Select(p => p.PartItem).Distinct(); }
        private void OnDatabaseUpdated()
        {
            DatabaseUpdatedEventHandler?.Invoke();
        }
        public MotorService(IDataAccessDB databaseRepository)
        {
            if (databaseRepository == null)
            {
                throw new ArgumentNullException("databaseRepository equals null.");
            }
            _dataAccessDB = databaseRepository;
        }

        public Motor GetMotorByMotorNumber(string motorNumber)
        {
            if (motorNumber == null || !MotorValidator.IsPartNumber(motorNumber))
                return null;

            return AllMotors.SingleOrDefault(m => m.MotorNumber == motorNumber);
        }

        public Part GetPartByPartNumber(string partNumber)
        {
            if (partNumber == null || !MotorValidator.IsPartNumber(partNumber))
                return null;

            return AllParts.SingleOrDefault(p => p.PartNumber == partNumber);
        }

        public List<Motor> MotorsFilterBy(IEnumerable<Motor> input, IEnumerable<MotorFamily> motorFamilies, IEnumerable<uint?> displacements, IEnumerable<string> features)
        {
            if (input == null)
                throw new ArgumentNullException("No motors given for filter.");
            else if (motorFamilies == null)
                throw new ArgumentNullException("No motor families given as filter parameter.");
            else if (displacements == null)
                throw new ArgumentNullException("No motor displacements given as filter parameter.");
            else if (features == null)
                throw new ArgumentNullException("No motor features given as filter parameter.");

            var result = input.Where(m => motorFamilies.Any(f => f.Equals(m.MotorType)));
            result = result.Where(m => displacements.Any(f => f.Equals(m.Displacement)));
            result = result.Where(m => features.Any(f => f.Equals(m.Feature)));

            return result.ToList();
        }

        private void UpdateDatabase(IEnumerable<Motor> motors)
        {
            if (motors == null)
            {
                return;
            }
            var commonMotors = AllMotors.Intersect(motors).ToList();
            var motorsToInsert = motors.Except(commonMotors);

            _dataAccessDB.InsertMotors(motorsToInsert);
            _dataAccessDB.UpdateMotors(commonMotors);

            OnDatabaseUpdated();
        }

        public void LoadFileAndUpdateDatabase(string filePath)
        {
            var motorsToUpdate = _dataAccessFile.GetAllMotors(filePath).ToList();

            UpdateDatabase(motorsToUpdate);
        }

        public void LoadFilesAndUpdateDatabase(string[] filePaths)
        {
            var motorsToUpdate = LoadMotorsFromFiles(filePaths);

            UpdateDatabase(motorsToUpdate);
        }

        public async Task<IEnumerable<Motor>> LoadFileAndReturnResultAsync(string filePath)
        {
            List<Motor> result = new List<Motor>();
            await Task.Run(() =>
            {
                result = _dataAccessFile.GetAllMotors(filePath).ToList();
            });
            return result;
        }

        public IEnumerable<Motor> LoadFileAndReturnResult(string path)
        {
            return _dataAccessFile.GetAllMotors(path);
        }

        public IEnumerable<Motor> LoadMotorsFromFiles(string[] paths)
        {
            var result = new List<Motor>();
            var motorsFromFile = new List<Motor>();
            foreach (var path in paths)
            {
                try
                {
                    motorsFromFile = LoadFileAndReturnResult(path).ToList();
                }
                catch (Exception ex)
                {
                    motorsFromFile.Clear();

                    throw new Exception($"Loading file {Path.GetFileName(path)} failed. Error: {Environment.NewLine}{ex.Message}", ex);
                }

                if (motorsFromFile.Count != 0)
                {
                    result.AddRange(motorsFromFile);
                }
            }

            return result;
        }

        public List<string> GetAllPartsDesignations() =>
            AllParts.OrderBy(m => m.Designation).Select(p => p.Designation).Distinct().ToList();

        public List<string> GetAllMotorsDisplacements() =>
            AllMotors.OrderBy(m => m.Displacement).Select(p => p.Displacement.ToString()).Distinct().ToList();

        public List<string> GetAllMotorsFamilies() =>
            AllMotors.OrderBy(m => m.MotorType).Select(p => p.MotorType.ToString()).Distinct().ToList();

        public List<string> GetAllMotorsfeatures() =>
            AllMotors.Select(p => p.Feature).OrderBy(f => f).Distinct().ToList();

        public IEnumerable<Part> GetPartsUsedWith(Part part)
        {
            var motorsUsedWithPart = WhereUsed(part);
            var partsUsedWith = motorsUsedWithPart.
                SelectMany(bi => bi.BOM).
                Select(p => p.PartItem).
                Distinct()
                .Where(p => !p.Equals(part));

            return partsUsedWith;
        }

        public IEnumerable<Part> PartsFilterBy(IEnumerable<Part> input, IEnumerable<string> designations, IEnumerable<string> descriptions)
        {
            var filteredByDesignations = PartsFilterByDesignations(input, designations);
            return PartsFilterByDescriptions(filteredByDesignations, descriptions);
        }

        public List<KeyValuePair<int, Motor>> FindSimilar(Motor compared, List<Motor> allMotors)
        {
            if (compared == null || allMotors == null)
            {
                return new List<KeyValuePair<int, Motor>>();
            }

            var motorsWithoutCompared = allMotors.Except(new List<Motor>() { compared });

            var result = new List<KeyValuePair<int, Motor>>();

            foreach (var motor in motorsWithoutCompared)
            {
                result.Add(new KeyValuePair<int, Motor>(CountDifferencesInBOM(compared, motor), motor));
            }

            return result;
        }

        private int CountDifferencesInBOM(Motor compared, Motor comparedTo)
        {
            int differences = 0;

            differences += Math.Max(0, comparedTo.BOM.Count - compared.BOM.Count);

            foreach (var BOMItem in compared.BOM)
            {
                if (!NotContainsBOMItem(comparedTo, BOMItem))
                {
                    differences++;
                }
            }

            return differences;
        }

        private bool NotContainsBOMItem(Motor comparedTo, BOMItem bOMItem)
        {
            bool result = comparedTo.BOM.Any(b => b.PartItem == bOMItem.PartItem);

            return result;
        }

        public IEnumerable<Part> PartsFilterByDescriptions(IEnumerable<Part> input, IEnumerable<string> descriptions)
        {
            if (descriptions == null || descriptions.Count() == 0)
            {
                return input;
            }
            var result = input.Where(p1 => descriptions.Any(p2 => p1.Description.Contains(p2)));
            return result;
        }

        public IEnumerable<Part> PartsFilterByDesignations(IEnumerable<Part> input, IEnumerable<string> designations)
        {
            if (designations == null)
            {
                return input;
            }
            var result = input.Where(p => designations.Contains(p.Designation));
            return result;
        }

        public IEnumerable<Motor> GetMotorsUsingBothParts(Part part1, Part part2)
        {
            if (part1 == null || part2 == null)
                return null;

            List<Motor> result = new List<Motor>();

            foreach (Motor motor in WhereUsed(part1))
            {
                if (motor.BOM.Select(p => p.PartItem).Contains(part2))
                    result.Add(motor);
            }

            return result;
        }

        public IEnumerable<Motor> GetMotorsUsedInBothParts(Part partUsedWith, Part part)
        {
            var result = WhereUsed(partUsedWith).Intersect(WhereUsed(part));
            return result;
        }

        public List<uint> GetAllPartPositionNumbers(Part part, IEnumerable<Motor> usedInMotors)
        {
            if (part == null || usedInMotors == null)
            {
                return new List<uint>();
            }

            var result = usedInMotors
                .SelectMany(b => b.BOM)
                .Where(p => p.PartItem.PartNumber == part.PartNumber)
                .Select(p => p.PositionNumber)
                .ToList();

            return result;
        }

        public IEnumerable<Motor> WhereUsed(Part part)
        {
            return WhereUsed(part, AllMotors);
        }

        public IEnumerable<Part> GetAllParts(IEnumerable<Motor> selectedMotors)
        {
            if (selectedMotors == null)
            {
                throw new ArgumentException(nameof(selectedMotors));
            }

            var result = selectedMotors.SelectMany(b => b.BOM).Select(p => p.PartItem).Distinct();
            return result;
        }

        public IEnumerable<Motor> WhereUsed(Part part, IEnumerable<Motor> motors)
        {
            if (part == null || motors == null)
            {
                return new List<Motor>();
            }

            var result = motors.Where(p => p.BOM.Any(b => b.PartItem.PartNumber.Equals(part.PartNumber)));

            return result;
        }

        public void ClearLoadedData()
        {
            _dataAccessDB = new DataAccessByMemoryCache();
            OnDatabaseUpdated();
        }
    }
}

