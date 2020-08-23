using BOMComparator.Core.Models;
using BOMComparator.ViewModels.Helpers;
using Caliburn.Micro;
using System;
using System.Collections.Generic;

namespace BOMComparator.ViewModels
{
    internal class PartsResultViewModel : Screen
    {
        private string _searchContent;
        private IMotorService motorService = IoC.Get<IMotorService>();

        public string SearchDescription
        {
            get => _searchContent;
            private set
            {
                _searchContent = value ?? throw new ArgumentNullException(nameof(value));
                NotifyOfPropertyChange(() => SearchDescription);
            }
        }
        public BindableCollection<PartUsedWithModel> UsedWithResults { get; }

        public PartsResultViewModel(IEnumerable<Part> parts, string description, string title = "Parts result")
        {
            SearchDescription = description;
            UsedWithResults = new BindableCollection<PartUsedWithModel>();
            foreach (var part in parts)
            {
                UsedWithResults.Add(new PartUsedWithModel(part, motorService.WhereUsed(part)));
            }
            DisplayName = title;
        }

        public PartsResultViewModel(IEnumerable<Part> parts, IEnumerable<Motor> motorsWhereUsed, string description, string title = "Parts result")
        {
            SearchDescription = description;
            UsedWithResults = new BindableCollection<PartUsedWithModel>();
            foreach (var part in parts)
            {
                UsedWithResults.Add(new PartUsedWithModel(part, motorService.WhereUsed(part, motorsWhereUsed)));
            }
            DisplayName = title;
        }

        public PartsResultViewModel(List<PartUsedWithModel> usedWithResults, string description = "Parts result")
        {
            SearchDescription = description;
            UsedWithResults = new BindableCollection<PartUsedWithModel>(usedWithResults);
        }

        public void RowSelect(PartUsedWithModel selectedPart)
        {
            var manager = IoC.Get<IWindowManager>();
            manager.ShowWindow(new WhereUsedViewModel(motorService.WhereUsed(selectedPart.PartUsedWith), $"Motors with {selectedPart.PartUsedWith}"), null, null);
        }
    }
}
