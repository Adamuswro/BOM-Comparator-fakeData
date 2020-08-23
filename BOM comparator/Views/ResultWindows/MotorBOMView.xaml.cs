using System.Windows;
using System.Windows.Input;

namespace BOMComparator.Views
{
    /// <summary>
    /// Interaction logic for MotorBOMView.xaml
    /// </summary>
    public partial class MotorBOMView : Window
    {
        public MotorBOMView()
        {
            InitializeComponent();
            PreviewKeyDown += (s, e) => { if (e.Key == Key.Escape) Close(); };
        }
    }
}
