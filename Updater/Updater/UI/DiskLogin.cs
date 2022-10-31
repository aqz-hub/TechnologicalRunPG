using IWshRuntimeLibrary;
using System;
using System.Windows.Forms;

namespace Updater.UI
{
    public partial class DiskLogin : Form
    {
        public bool success = false;
        WhereWeGo Disk;
        public enum WhereWeGo { Elma, Documents, ElmaAndDocumnets, Nowhere };
        public DiskLogin(WhereWeGo disk)
        {
            InitializeComponent();
            Disk = disk;
        }

        private void buttonLogIn_Click(object sender, EventArgs e)
        {
            EnterToDisk();
        }
        /// <summary>
        /// Вход в диск.
        /// </summary>
        public void EnterToDisk()
        {
            try
            {
                WshNetwork network = new WshNetwork();
                object save = false;
                object user = textBoxLogin.Text, pass = textBoxPassword.Text;
                switch (Disk)
                {
                    case WhereWeGo.Elma:
                        {
                            network.MapNetworkDrive("Y:", @"\\10.59.4.20\Exchange2", ref save, ref user, ref pass);
                            break;
                        }
                    case WhereWeGo.Documents:
                        {
                            network.MapNetworkDrive("P:", @"\\10.59.4.45\Documents_1", ref save, ref user, ref pass);
                            break;
                        }
                    case WhereWeGo.ElmaAndDocumnets:
                        {
                            network.MapNetworkDrive("Y:", @"\\10.59.4.20\Exchange2", ref save, ref user, ref pass);
                            network.MapNetworkDrive("P:", @"\\10.59.4.45\Documents_1", ref save, ref user, ref pass);
                           
                            break;
                        }
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                MessageBox.Show(ex.Message, "Ошибка!");
            }
            if (success)
            {
                this.Close();
            }
        }
        /// <summary>
        /// При нажатии Enter осуществляется вход
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                EnterToDisk();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }
    }
}
