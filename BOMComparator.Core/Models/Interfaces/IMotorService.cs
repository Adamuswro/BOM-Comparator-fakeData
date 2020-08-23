using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BOMComparator.Core.Models.MotorService;

namespace BOMComparator.Core.Models
{
    public interface IMotorService
    {
        IEnumerable<Motor> AllMotors { get; }
        IEnumerable<Part> AllParts { get; }
        event Notify DatabaseUpdatedEventHandler;
        Motor GetMotorByMotorNumber(string motorNumber);
        Part GetPartByPartNumber(string partNumber);
        void LoadFileAndUpdateDatabase(string filePath);
        List<string> GetAllPartsDesignations();
        List<uint> GetAllPartPositionNumbers(Part part, IEnumerable<Motor> usedInMotors);
        List<string> GetAllMotorsDisplacements();
        List<string> GetAllMotorsFamilies();
        List<string> GetAllMotorsfeatures();
        IEnumerable<Part> GetAllParts(IEnumerable<Motor> selectedMotors);
        void LoadFilesAndUpdateDatabase(string[] filePath);
        Task<IEnumerable<Motor>> LoadFileAndReturnResultAsync(string path);
        IEnumerable<Motor> LoadFileAndReturnResult(string path);
        IEnumerable<Part> GetPartsUsedWith(Part part);
        IEnumerable<Motor> LoadMotorsFromFiles(string[] paths);
        List<Motor> MotorsFilterBy(IEnumerable<Motor> input, IEnumerable<MotorFamily> motorFamilies, IEnumerable<uint?> displacements, IEnumerable<string> features);
        IEnumerable<Part> PartsFilterBy(IEnumerable<Part> input, IEnumerable<string> designations, IEnumerable<string> descriptions);
        IEnumerable<Motor> GetMotorsUsedInBothParts(Part partUsedWith, Part part);
        IEnumerable<Motor> WhereUsed(Part part);
        IEnumerable<Motor> WhereUsed(Part part, IEnumerable<Motor> motors);
        void ClearLoadedData();
        IEnumerable<Motor> GetMotorsUsingBothParts(Part part1, Part part2);
        List<KeyValuePair<int, Motor>> FindSimilar(Motor compared, List<Motor> allMotors);
    }
}
