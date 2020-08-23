using BOMComparator.Core.Models;
using BOMComparator.ViewModels.Helpers;
using Caliburn.Micro;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BOMComparator.ViewModels
{
    public class PartsFilterViewModel : Screen
    {
        private IMotorService _motorService;

        public IList<PropertyViewModel> Designations { get; set; } = new ObservableCollection<PropertyViewModel>();
        public List<string> SelectedDesignations
        {
            get => Designations.Where(d => d.IsSelected == true).Select(d => d.Property).ToList();
        }
        //TODO: Implement this feature, for now it's just wire up to motorService and doesn't make nothing
        public List<string> SelectedDescriptions
        {
            get => new List<string>();
        }
        public string Description { get; set; }
        public IEnumerable<string> Descriptions
        {
            get => UserInputConverter.SplitText(Description);
        }

        public PartsFilterViewModel(IMotorService motorService)
        {
            this._motorService = motorService;
            this._motorService.DatabaseUpdatedEventHandler += UpdateProperties;
            UpdateProperties();
        }

        private void UpdateProperties()
        {
            FilterHelper.CreatePropertyViewModel(Designations, _motorService.GetAllPartsDesignations());
        }

        public void SelectAllDesignations()
        {
            foreach (var designation in Designations)
            {
                designation.IsSelected = true;
            }
        }

        public void ClearAllDesignations()
        {
            foreach (var designation in Designations)
            {
                designation.IsSelected = false;
            }
        }
    }
}