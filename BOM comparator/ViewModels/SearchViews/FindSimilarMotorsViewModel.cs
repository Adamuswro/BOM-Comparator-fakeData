using BOMComparator.Core.Models;
using Caliburn.Micro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.ViewModels
{
    public class FindSimilarMotorsViewModel : Screen, ISearchViewModel
    {
        public List<int> SimilarityLevel { get => new List<int>() { 0, 1, 2, 3, 4, 5 }; }
        public int SelectedSimilarityLevel { get; set; }
        public MotorsFilterViewModel Filter { get; set; }

        private string partNumber;
        public string PartNumber
        {
            get { return partNumber; }
            set
            {
                partNumber = value;
                NotifyOfPropertyChange(() => PartNumber);
                NotifyOfPropertyChange(() => CanExecuteSearch);
            }
        }

        private IMotorService motorService;

        public FindSimilarMotorsViewModel(IMotorService motorService)
        {
            Filter = new MotorsFilterViewModel(motorService);
            this.motorService = motorService;
            DisplayName = "Find similar motors";
        }

        public bool CanExecuteSearch
        {
            get => MotorValidator.IsPartNumber(PartNumber);
        }

        public void ExecuteSearch()
        {
            Motor motor = motorService.GetMotorByMotorNumber(PartNumber);
            if (motor == null)
            {
                ShellViewModel.Log($"Motor {PartNumber} not found.");
                return;
            }

            var motorFamilies = Filter.SelectedMotorFamilies;
            var features = Filter.SelectedFeatures;
            var displacements = Filter.SelectedDisplacements;

            var filteredMotors = motorService.MotorsFilterBy(input: motorService.AllMotors, motorFamilies: motorFamilies, displacements: displacements, features: features);

            if (filteredMotors == null)
            {
                ShellViewModel.Log($"No motors found with current filtering criteria. To see other results select other filters.");
                return;
            }

            var allComparedMotors = motorService.FindSimilar(motor, filteredMotors);
            var result = allComparedMotors.Where(k=>k.Key<= SelectedSimilarityLevel).Select(m=>m.Value);

            if (result==null)
            {
                ShellViewModel.Log($"No motors found with selected level of similarity. The most similar motor has got {allComparedMotors.Select(k=>k.Key).Min()} differences in BOM.");
                return;
            }

            string queryDescription = CreateQueryDescription(result);

            IWindowManager manager = new WindowManager();
            manager.ShowWindow(new WhereUsedViewModel(result, queryDescription));
        }

        private string CreateQueryDescription(IEnumerable<Motor> motors)
        {
            string result = $"{motors.Count()} motors similar to {motorService.GetMotorByMotorNumber(PartNumber).FullDescription} found.{Environment.NewLine}Differences in BOM: <= {SelectedSimilarityLevel}.{Environment.NewLine}Query criteria:{Environment.NewLine}Motor families: ";

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
