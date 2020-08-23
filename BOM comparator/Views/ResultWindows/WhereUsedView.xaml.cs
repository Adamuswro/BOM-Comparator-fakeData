using System.Windows;
using System.Windows.Input;

namespace BOMComparator.Views
{
    /// <summary>
    /// Interaction logic for WhereUsedViewModel.xaml
    public partial class WhereUsedView : Window
    {
        public WhereUsedView()
        {
            InitializeComponent();
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }
    }
}
