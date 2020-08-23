using BOMComparator.Core.Models;
using BOMComparator.ViewModels.Helpers;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.ViewModels
{
    internal class UsedWithSearchViewModel : Screen, ISearchViewModel
    {
        private IMotorService _motorService;

        public PartsFilterViewModel Filter { get; set; }

        private string _partNumber;
        public string PartNumber
        {
            get { return _partNumber; }
            set
            {
                _partNumber = value;
                NotifyOfPropertyChange(() => PartNumber);
                NotifyOfPropertyChange(() => CanExecuteSearch);
            }
        }

        public bool CanExecuteSearch
        {
            get => MotorValidator.IsPartNumber(PartNumber);
        }

        public UsedWithSearchViewModel(IMotorService motorService)
        {
            Filter = new PartsFilterViewModel(motorService);
            this._motorService = motorService;
            DisplayName = "Used With";
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

            Part part = _motorService.GetPartByPartNumber(PartNumber);
            if (part == null)
            {
                ShellViewModel.Log($"{PartNumber} not found. Try again.");
                return;
            }

            var partsUsedWith = _motorService.GetPartsUsedWith(part);
            if (partsUsedWith == null || partsUsedWith.Count() == 0)
            {
                ShellViewModel.Log($"No used parts found with {PartNumber}");
                return;
            }

            partsUsedWith = _motorService.PartsFilterBy(
                input: partsUsedWith,
                designations: Filter.SelectedDesignations,
                descriptions: Filter.Descriptions);
            if (partsUsedWith == null || partsUsedWith.Count() == 0)
            {
                ShellViewModel.Log($"No parts found with current filtering criteria. To see other results select other filters.");
                return;
            }

            var result = new List<PartUsedWithModel>();

            foreach (var partUsedWith in partsUsedWith)
            {
                var commonMotors = _motorService.GetMotorsUsedInBothParts(partUsedWith, part);
                result.Add(new PartUsedWithModel(partUsedWith, commonMotors));
            }

            IWindowManager manager = new WindowManager();
            manager.ShowWindow(new PartsResultViewModel(result, $"Parts used with {part}"));
        }
    }
}
