using System.Windows;

namespace TechnologicalRunPG.UI
{
    /// <summary>
    /// Логика взаимодействия для DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        public DataWindow()
        {
            InitializeComponent();
        }

        #region Обработчики
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.Left = System.Windows.Forms.Screen.AllScreens[0].WorkingArea.Location.X;
            this.Top = System.Windows.Forms.Screen.AllScreens[0].WorkingArea.Location.Y;
            this.WindowState = WindowState.Maximized;
        }
        #endregion
    }
}
