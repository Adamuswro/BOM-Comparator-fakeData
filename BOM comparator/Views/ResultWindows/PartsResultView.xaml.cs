using System.Windows;
using System.Windows.Input;

namespace BOMComparator.Views
{
    /// <summary>
    /// Interaction logic for PartResultView.xaml
    /// </summary>
    public partial class PartsResultView : Window
    {
        public PartsResultView()
        {
            InitializeComponent();
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }
    }
}
