using BOMComparator.Core.Models;
using Caliburn.Micro;

namespace BOMComparator.ViewModels
{
    internal class MotorBOMViewModel : Screen
    {
        public string ViewTitle { get; }
        public BindableCollection<BOMItem> BOM { get; }

        public MotorBOMViewModel(Motor motor, string description = "Motor Bill Of Material.")
        {
            BOM = new BindableCollection<BOMItem>(motor.BOM);
            ViewTitle = description;
            DisplayName = $"{motor.FullDescription}";
        }

        public void RowSelect(BOMItem selectedBOMItem)
        {
            var selectedPart = selectedBOMItem.PartItem;
            var motorService = IoC.Get<IMotorService>();
            var windowManager = IoC.Get<IWindowManager>();
            windowManager.ShowWindow(new WhereUsedViewModel(motorService.WhereUsed(selectedPart), $"Motors with {selectedPart}"), null, null);
        }
    }
}
