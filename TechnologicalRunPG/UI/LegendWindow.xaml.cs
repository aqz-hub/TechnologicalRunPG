using System.Windows;
using System.Windows.Input;
using TechnologicalRunPG.HW;

namespace TechnologicalRunPG.UI
{
    /// <summary>
    /// Логика взаимодействия для LegendWindow.xaml
    /// </summary>
    public partial class LegendWindow : Window
    {
        public LegendWindow()
        {
            InitializeComponent();

            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.closebutton);
            conImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.waiticon);
            notokImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.notOkay);
            okImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.okay);
            disImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.disconnect);
            errorImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.warning);
            outrangeImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.outrange);
            wronggImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.wrongversion);

            taskNotFindedImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.taskNotFinded);
            taskNotClosedImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.taskNotClosed);
            taskFindedImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.taskFinded);
            taskClosedImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.taskClosed);

            protocolNotCreatedImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.protocolNotCreated);
            protocolCreatedImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.protocolCreated);
        }

        #region Обработчики
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void closebutton_MouseEnter(object sender, MouseEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.greyclosebutton);
        }
        private void closebutton_MouseLeave(object sender, MouseEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.closebutton);
        }
        private void closebutton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }
        #endregion
    }
}
