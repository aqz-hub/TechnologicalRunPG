using System.Windows;
using Updater.ViewModel;

namespace Updater.View
{
    /// <summary>
    /// Логика взаимодействия для UpdateView.xaml
    /// </summary>
    public partial class UpdateView : Window
    {
        public UpdateView()
        {
            InitializeComponent();
            DataContext = new UpdateViewModel();
        }
    }
}
