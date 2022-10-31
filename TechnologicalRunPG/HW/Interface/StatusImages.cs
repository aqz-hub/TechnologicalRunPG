using System.Windows.Media.Imaging;

namespace TechnologicalRunPG.HW
{
    class StatusImages
    {
        /// <summary>
        /// Картинка для ожидания
        /// </summary>
        public BitmapImage waitImage = BitmapWork.Bitmap2BitmapImage(Properties.Resources.waiticon);
        /// <summary>
        /// Okay
        /// </summary>
        public BitmapImage okImage = BitmapWork.Bitmap2BitmapImage(Properties.Resources.okay);
        /// <summary>
        /// Ne okay
        /// </summary>
        public BitmapImage notOkay = BitmapWork.Bitmap2BitmapImage(Properties.Resources.notOkay);
        /// <summary>
        /// Disconnected
        /// </summary>
        public BitmapImage disconnected = BitmapWork.Bitmap2BitmapImage(Properties.Resources.disconnect);
        /// <summary>
        /// Warning
        /// </summary>
        public BitmapImage warning = BitmapWork.Bitmap2BitmapImage(Properties.Resources.warning);
        /// <summary>
        /// Out range
        /// </summary>
        public BitmapImage outrange = BitmapWork.Bitmap2BitmapImage(Properties.Resources.outrange);
        /// <summary>
        /// Wrong version
        /// </summary>
        public BitmapImage wrongverison = BitmapWork.Bitmap2BitmapImage(Properties.Resources.wrongversion);
        /// <summary>
        /// Task not finded
        /// </summary>
        public BitmapImage tasknotfinded = BitmapWork.Bitmap2BitmapImage(Properties.Resources.taskNotFinded);
        /// <summary>
        /// Task finded
        /// </summary>
        public BitmapImage taskfinded = BitmapWork.Bitmap2BitmapImage(Properties.Resources.taskFinded);
        /// <summary>
        /// Task closed
        /// </summary>
        public BitmapImage taskclosed = BitmapWork.Bitmap2BitmapImage(Properties.Resources.taskClosed);
        /// <summary>
        /// Task not closed
        /// </summary>
        public BitmapImage tasknotclose = BitmapWork.Bitmap2BitmapImage(Properties.Resources.taskNotClosed);
        /// <summary>
        /// Protocol not created
        /// </summary>
        public BitmapImage protocolnotcreated = BitmapWork.Bitmap2BitmapImage(Properties.Resources.protocolNotCreated);
        /// <summary>
        /// Protocol created
        /// </summary>
        public BitmapImage protocolcreated = BitmapWork.Bitmap2BitmapImage(Properties.Resources.protocolCreated);
    }
}
