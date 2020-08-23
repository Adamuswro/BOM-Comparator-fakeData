using BOMComparator.Core.Models;
using Caliburn.Micro;
using System;

namespace BOMComparator.ViewModels
{
    internal class WhereUsedSearchViewModel : Screen, ISearchViewModel
    {
        private IMotorService motorService;

        public MotorsFilterViewModel Filter { get; set; }

        public WhereUsedSearchViewModel(IMotorService motorService)
        {
            Filter = new MotorsFilterViewModel(motorService);
            this.motorService = motorService;
            DisplayName = "Where Used";
        }

        private string partNumber;
        public string PartNumber
        {
            get => partNumber;
            set
            {
                partNumber = value;
                NotifyOfPropertyChange(() => PartNumber);
                NotifyOfPropertyChange(() => CanExecuteSearch);
            }
        }

        public bool CanExecuteSearch
        {
            get => MotorValidator.IsPartNumber(PartNumber);
        }

        public void ExecuteSearch()
        {
            if (String.IsNullOrEmpty(PartNumber))
            {
                ShellViewModel.Log($"You must provide P/N.");
                return;
            }

            if (!MotorValidator.IsPartNumber(PartNumber))
            {
                ShellViewModel.Log($"{PartNumber} is not valid P/N. Try again.");
                return;
            }

            Part part = motorService.GetPartByPartNumber(PartNumber);
            if (part == null)
            {
                ShellViewModel.Log($"{PartNumber} not found. Try again.");
                return;
            }

            var allMotorsWhereUsed = motorService.WhereUsed(part);

            var motorFamilies = Filter.SelectedMotorFamilies;
            var features = Filter.SelectedFeatures;
            var displacements = Filter.SelectedDisplacements;

            var result = motorService.MotorsFilterBy(input: allMotorsWhereUsed, motorFamilies: motorFamilies, displacements: displacements, features: features);

            if (result == null)
            {
                ShellViewModel.Log($"No motors found with current filtering criteria. To see other results select other filters.");
                return;
            }

            IWindowManager manager = new WindowManager();
            manager.ShowWindow(new WhereUsedViewModel(result, $"{result.Count} motors are using part:{Environment.NewLine}{part}"));
        }
    }
}