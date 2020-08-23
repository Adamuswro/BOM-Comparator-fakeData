using BOMComparator.Core.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;

namespace BOMComparator.ViewModels
{
    internal class WhereUsedViewModel : Screen
    {
        public BindableCollection<Motor> Motors { get; }
        public string ViewTitle { get; }

        public WhereUsedViewModel(IEnumerable<Motor> motors, string description = "Motors result", string title = "Where used")
        {
            if (motors == null)
            {
                motors = new BindableCollection<Motor>();
                return;
            }

            ViewTitle = description;
            Motors = new BindableCollection<Motor>(motors);
            DisplayName = title;
        }
        public void RowSelect(Motor selectedMotor)
        {
            var windowManager = IoC.Get<IWindowManager>();
            windowManager.ShowWindow(new MotorBOMViewModel(selectedMotor, $"{selectedMotor.FullDescription}{Environment.NewLine}Bill Of Material."), null, null);
        }

    }
}