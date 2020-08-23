using Caliburn.Micro;

namespace BOMComparator.ViewModels.Helpers
{
    public class PropertyViewModel : Screen
    {
        public string Property { get; set; }
        public bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }

        public override string ToString()
        {
            return Property;
        }
    }
}