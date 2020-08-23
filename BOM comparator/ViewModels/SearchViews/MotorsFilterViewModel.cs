using BOMComparator.Core.Models;
using BOMComparator.ViewModels.Helpers;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BOMComparator.ViewModels
{
    public class MotorsFilterViewModel : Screen
    {
        private IMotorService _motorService;

        public IList<PropertyViewModel> MotorFamilies { get; set; } = new ObservableCollection<PropertyViewModel>();
        public List<MotorFamily> SelectedMotorFamilies
        {
            get => MotorFamilies.Where(d => d.IsSelected == true).Select(p => (MotorFamily)Enum.Parse(typeof(MotorFamily), p.Property)).ToList();
        }

        public IList<PropertyViewModel> Displacements { get; set; } = new ObservableCollection<PropertyViewModel>();
        public List<uint?> SelectedDisplacements
        {
            get => Displacements.Where(d => d.IsSelected == true && !String.IsNullOrEmpty(d.Property)).Select(p => (uint?)Convert.ToUInt32(p.Property)).ToList();
        }

        public IList<PropertyViewModel> Features { get; set; } = new ObservableCollection<PropertyViewModel>();
        public List<string> SelectedFeatures
        {
            get => Features.Where(d => d.IsSelected == true).Select(p => p.Property).ToList();
        }

        public string Description { get; set; }
        public IEnumerable<string> Descriptions
        {
            get => UserInputConverter.SplitText(Description);
        }

        public MotorsFilterViewModel(IMotorService motorService)
        {
            this._motorService = motorService;
            this._motorService.DatabaseUpdatedEventHandler += UpdateProperties;
            UpdateProperties();
        }

        public void UpdateProperties()
        {
            FilterHelper.CreatePropertyViewModel(MotorFamilies, _motorService.GetAllMotorsFamilies());
            FilterHelper.CreatePropertyViewModel(Displacements, _motorService.GetAllMotorsDisplacements());
            FilterHelper.CreatePropertyViewModel(Features, _motorService.GetAllMotorsfeatures());
        }

        public void SelectAllMotorFamilies()
        {
            foreach (var motorFamily in MotorFamilies)
            {
                motorFamily.IsSelected = true;
            }
        }

        public void ClearAllMotorFamilies()
        {
            foreach (var motorFamily in MotorFamilies)
            {
                motorFamily.IsSelected = false;
            }
        }

        public void SelectAllDisplacements()
        {
            foreach (var displacement in Displacements)
            {
                displacement.IsSelected = true;
            }
        }

        public void ClearAllDisplacements()
        {
            foreach (var displacement in Displacements)
            {
                displacement.IsSelected = false;
            }
        }

        public void SelectAllFeatures()
        {
            foreach (var feature in Features)
            {
                feature.IsSelected = true;
            }
        }

        public void ClearAllFeatures()
        {
            foreach (var feature in Features)
            {
                feature.IsSelected = false;
            }
        }
    }
}