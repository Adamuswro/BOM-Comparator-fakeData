using BOMComparator.Core.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.ViewModels
{
    internal class ShowMotorsViewModel : Screen, ISearchViewModel
    {
        private IMotorService _motorService;

        public MotorsFilterViewModel Filter { get; set; }

        public ShowMotorsViewModel(IMotorService motorService)
        {
            Filter = new MotorsFilterViewModel(motorService);
            this._motorService = motorService;
            DisplayName = "Show motors";
        }

        public void ExecuteSearch()
        {
            var motorFamilies = Filter.SelectedMotorFamilies;
            var features = Filter.SelectedFeatures;
            var displacements = Filter.SelectedDisplacements;

            var result = _motorService.MotorsFilterBy(input: _motorService.AllMotors, motorFamilies: motorFamilies, displacements: displacements, features: features);

            if (result == null)
            {
                ShellViewModel.Log($"No motors found with current filtering criteria. To see other results select other filters.");
                return;
            }

            string queryDescription = CreateQueryDescription(result);

            IWindowManager manager = new WindowManager();
            manager.ShowWindow(new WhereUsedViewModel(result, queryDescription));
        }

        private string CreateQueryDescription(IEnumerable<Motor> motors)
        {
            string result = $"{motors.Count()} motors found. Query criteria:{Environment.NewLine}Motor families: ";

            if (Filter.SelectedMotorFamilies.Count == Filter.MotorFamilies.Count)
                result += "All";
            else
                result += String.Join(",", Filter.SelectedMotorFamilies);

            result += $"{Environment.NewLine}Features: ";
            if (Filter.SelectedFeatures.Count == Filter.Features.Count)
                result += "All";
            else
                result += String.Join(",", Filter.SelectedFeatures);

            result += $"{Environment.NewLine}Displacements: ";
            if (Filter.SelectedDisplacements.Count == Filter.Displacements.Count)
                result += "All";
            else
                result += String.Join(",", Filter.SelectedDisplacements);

            return result;
        }
    }
}
