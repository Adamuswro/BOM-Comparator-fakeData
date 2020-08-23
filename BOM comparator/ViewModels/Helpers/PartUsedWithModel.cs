using BOMComparator.Core.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOMComparator.ViewModels.Helpers
{
    public class PartUsedWithModel
    {
        public Part PartUsedWith { get; }
        public string PositionNumbers { get; }
        public BindableCollection<Motor> MotorsUsedWith { get; }

        public PartUsedWithModel(Part partUsedWith, IEnumerable<Motor> motorsUsedWith)
        {
            PartUsedWith = partUsedWith;
            MotorsUsedWith = new BindableCollection<Motor>(motorsUsedWith.OrderBy(o => o.FullDescription).ToList());
            PositionNumbers = String.Join(", ", BOMAnalizeHelper.GetAllPartPositionNumbers(partUsedWith, motorsUsedWith).OrderBy(p => p));
        }
    }
}
