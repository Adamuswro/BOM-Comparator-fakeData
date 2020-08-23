using BOMComparator.Core.Models;
using Caliburn.Micro;
using System.Collections.Generic;

namespace BOMComparator.ViewModels
{
    internal class SearchManagerViewModel : Screen
    {
        private IMotorService _motorService;

        private ISearchViewModel _currentSearchOption;
        public ISearchViewModel CurrentSearchOption
        {
            get
            {
                return _currentSearchOption;
            }
            set
            {
                if (_currentSearchOption != value)
                {
                    _currentSearchOption = value;
                    NotifyOfPropertyChange(() => CurrentSearchOption);
                }
            }
        }
        private List<ISearchViewModel> _searchOptions;
        public List<ISearchViewModel> SearchOptions
        {
            get
            {
                if (_searchOptions == null)
                {
                    _searchOptions = new List<ISearchViewModel>();
                }
                return _searchOptions;
            }
        }

        public SearchManagerViewModel(IMotorService motorService)
        {
            _motorService = motorService;
            //_motorService = new MotorService(new RandomDataForTests(120, 100));

            //Add all search options
            SearchOptions.Add(new ShowMotorsViewModel(_motorService));
            SearchOptions.Add(new ShowPartsViewModel(_motorService));
            SearchOptions.Add(new ShowBOMViewModel(_motorService));
            SearchOptions.Add(new WhereUsedSearchViewModel(_motorService));
            SearchOptions.Add(new UsedWithSearchViewModel(_motorService));
            SearchOptions.Add(new FindSimilarMotorsViewModel(_motorService));

            //Set default search option
            CurrentSearchOption = SearchOptions[0];
        }
    }
}
