using BOMComparator.Core.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.ViewModels
{
    internal class ShowPartsViewModel : Screen, ISearchViewModel
    {
        private IMotorService _motorService;

        public PartsFilterViewModel Filter { get; set; }
        public MotorsFilterViewModel MotorsFilter { get; set; }

        public ShowPartsViewModel(IMotorService motorService)
        {
            Filter = new PartsFilterViewModel(motorService);
            MotorsFilter = new MotorsFilterViewModel(motorService);
            this._motorService = motorService;
            DisplayName = "Show parts";
        }

        public void ExecuteSearch()
        {
            var selectedMotors = _motorService.MotorsFilterBy(_motorService.AllMotors, MotorsFilter.SelectedMotorFamilies, MotorsFilter.SelectedDisplacements, MotorsFilter.SelectedFeatures);
            var partsFromSelectedMotors = _motorService.GetAllParts(selectedMotors);

            var result = _motorService.PartsFilterBy(partsFromSelectedMotors, Filter.SelectedDesignations, Filter.SelectedDescriptions);

            if (result == null)
            {
                ShellViewModel.Log($"No parts found with current filtering criteria. To see other results select other filters.");
                return;
            }

            string queryDescription = CreateQueryDescription(result);

            IWindowManager manager = new WindowManager();
            manager.ShowWindow(new PartsResultViewModel(result, selectedMotors, queryDescription));
        }

        private string CreateQueryDescription(IEnumerable<Part> parts)
        {
            string result = $"{parts.Count()} parts found. Query criteria:{Environment.NewLine}Parts designations: ";

            if (Filter.SelectedDesignations.Count == Filter.Designations.Count)
                result += "All";
            else
                result += String.Join(",", Filter.SelectedDesignations);

            result += $"{Environment.NewLine}Descriptions: ";
            if (String.IsNullOrEmpty(Filter.Description))
                result += "All";
            else
                result += String.Join(",", Filter.Designations);

            return result;
        }
    }
}
