using BOMComparator.Core.Models;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.ViewModels.Helpers
{
    public class UsedWithQueryManager
    {
        public Part SearchedPart { get; private set; }
        public BindableCollection<PartUsedWithModel> Results { get; set; }

        public UsedWithQueryManager(Part searchedPart, IEnumerable<Motor> motorDatabase)
        {
            SearchedPart = searchedPart;
            Results = new BindableCollection<PartUsedWithModel>();
            ExecuteSearch(motorDatabase);
        }

        private void ExecuteSearch(IEnumerable<Motor> motors)
        {
            var results = new List<Part>();

            //Gets lists of all parts used with SearchedPart in motors, removes duplicates and removes SearchedPart
            foreach (var motor in motors)
            {
                if (motor.BOM.Any(b => b.PartItem.Equals(SearchedPart)))
                {
                    results.AddRange(motor.BOM.Select(p => p.PartItem).ToList());
                    results.Remove(SearchedPart);
                    results = results.Distinct().ToList();
                }
            }

            results = results.OrderBy(o => o.Designation).ToList();
            //Create UsedWithModel --> store part used with SearchedPart and list of motors where parts are used together
            foreach (var result in results)
            {
                this.Results.Add(new PartUsedWithModel(result, IoC.Get<IMotorService>().GetMotorsUsingBothParts(SearchedPart, result)));
            }
        }
    }


}
