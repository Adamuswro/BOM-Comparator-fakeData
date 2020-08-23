using BOMComparator.Core.Models;
using Caliburn.Micro;
using System;

namespace BOMComparator.ViewModels
{
    internal class ShowBOMViewModel : Screen, ISearchViewModel
    {
        private IMotorService _motorService;
        public MotorsFilterViewModel Filter { get; set; }

        public ShowBOMViewModel(IMotorService motorService)
        {
            Filter = new MotorsFilterViewModel(motorService);
            _motorService = motorService;
            DisplayName = "Show BOM";
        }

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
            var motor = _motorService.GetMotorByMotorNumber(PartNumber);

            if (motor == null)
            {
                ShellViewModel.Log($"Motor {PartNumber} not found.");
                return;
            }

            IWindowManager manager = new WindowManager();
            manager.ShowWindow(new
                MotorBOMViewModel(motor, $"{motor.FullDescription}{Environment.NewLine}Bill Of Material."), null, null);
        }

    }
}