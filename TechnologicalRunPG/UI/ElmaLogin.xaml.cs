using System;
using System.Windows;
using System.Windows.Input;
using TechnologicalRunPG.HW;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TechnologicalRunPG.UI
{
    /// <summary>
    /// Логика взаимодействия для ElmaLogin.xaml
    /// </summary>
    public partial class ElmaLogin : Window
    {
        /// <summary>
        /// Триггер повторного логина
        /// </summary>
        public static bool secondLogin { get; set; } = false;
        /// <summary>
        /// Триггер закрытия
        /// </summary>
        bool userClosing { get; set; } = true;
        /// <summary>
        /// Триггер удачного входа
        /// </summary>
        public bool success { get; set; } = false;
        /// <summary>
        /// Логин
        /// </summary>
        public static string login { get; set; } = "";
        /// <summary>
        /// Пароль
        /// </summary>
        public static string password { get; set; } = "";
        /// <summary>
        /// Пользователь
        /// </summary>
        public static ElmaUser elmaUser = new ElmaUser();
        /// <summary>
        /// Обработка входа в Элму.
        /// </summary>
        Elma elmaConnect = new Elma();
        /// <summary>
        /// Сервис для авторизации
        /// </summary>
        WsdlAuthorizationService service = new WsdlAuthorizationService();
        /// <summary>
        /// Ссылка на главное окно
        /// </summary>
        MainWindow target_form = null;

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        public ElmaLogin(MainWindow _target_form)
        {
            InitializeComponent();
            target_form = _target_form;
            elmaLogoImage.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.elmalogo);
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.closebutton);
            DisableCapsLock();
            loginText.Focus();
        }
        /// <summary>
        /// Выключить CapsLock
        /// </summary>
        private void DisableCapsLock()
        {
            if (System.Windows.Forms.Control.IsKeyLocked(Keys.CapsLock))
            {
                const int KEYEVENTF_EXTENDEDKEY = 0x1;
                const int KEYEVENTF_KEYUP = 0x2;
                keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,
                (UIntPtr)0);
            }
        }
        /// <summary>
        /// Войти в Элму.
        /// </summary>
        /// <param name="_login"></param>
        /// <param name="_password"></param>
        private void EnterToElma(string _login, string _password)
        {
            // Устанавливаем ЛОГИН
            service.userName = _login;
            // Устанавливаем ПАРОЛЬ
            service.userPwd = _password;
            // Проверяем подключение
            target_form.picture.SetImage(service.TestConnection());
            // Проверяем существует ли такой пользователь в базе
            elmaUser = elmaConnect.CheckElmaUser(service.userId, _login);
            if(elmaUser != null)
            {
                string user = elmaUser.ElmaUserLastName.ToString() + " " + elmaUser.ElmaUserFirstName[0] + ". " + elmaUser.ElmaUserMiddleName[0] + ". ";
                if (elmaUser != null)
                {
                    if (CheckTime())
                    {
                        if (elmaUser.competences == null)
                        {
                            System.Windows.MessageBox.Show(user + ", у вас нет доступа к технологическому прогону ПГ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            elmaUser.competences.CheckAttestation();
                            if (elmaUser.competences.access)
                            {
                                if (elmaUser.competences.warningMessage != "")
                                {
                                    System.Windows.MessageBox.Show(elmaUser.competences.warningMessage, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                                target_form.nameLabel.Content = user;
                                login = _login;
                                password = _password;
                                userClosing = false;
                                success = true;
                                this.Close();
                            }
                            else
                            {
                                System.Windows.MessageBox.Show(elmaUser.competences.warningMessage, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
                else
                {
                    loginText.Text = "";
                    passwordText.Password = "";
                    System.Windows.MessageBox.Show(this, "Неправильно введена комбинация Логин/пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    loginText.Focus();
                }
            }
            else
            {
                loginText.Text = "";
                passwordText.Password = "";
                System.Windows.MessageBox.Show(this, "Неправильно введена комбинация Логин/пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                loginText.Focus();
            }
        }
        /// <summary>
        /// Проверить авторизацию пользователя.
        /// </summary>
        /// <returns></returns>
        public bool CheckTime()
        {
            string SQLQuery = "select TOP 1 TimeIn, TimeOut, id from PersonalTimeTracking" +
                             $" where Usr like '{service.userId}'" +
                              " ORDER BY ID DESC";
            List<object> result = ElmaConnect.SqlQuery(SQLQuery);
            if (result != null)
            {
                if (result.Count != 0)
                {
                    if (result[1].ToString() == "")
                    {
                        if (Convert.ToDateTime(result[0]) < DateTime.Today.AddDays(1) && Convert.ToDateTime(result[0]) > DateTime.Today)
                        {
                            //Бинго-бонго
                            return true;
                        }
                        else
                        {
                            //Если пользователь забыл закрыть смену.
                            return false;
                        }
                    }
                    else
                    {
                        //Если пользователь не вошел утром.
                        return false;
                    }
                }
                else
                {
                    //Если пользователь вообще не пользовался системой.
                    return false;
                }
            }
            else
            {
                //Если система не ответила.
                return false;
            }
        }

        #region Обработчики
        #region Кнопка закрытия программы
        private void closebutton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.greyclosebutton);
        }

        private void closebutton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            closebutton.Source = BitmapWork.Bitmap2BitmapImage(Properties.Resources.closebutton);
        }
        private void closebutton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        #endregion

        #region При закрытии окна
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!secondLogin)
            {
                if (userClosing) //Если пользователь хочет закрыть с кнопки.
                {
                    Environment.Exit(0);
                    e.Cancel = true;
                }
                else             //Если форму закрывает программа, чтоб перейти дальше.
                {
                    e.Cancel = false;
                    target_form.Show();
                }
            }
            else
            {
                e.Cancel = false;
                target_form.Show();
            }
        }
        #endregion

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            EnterToElma(loginText.Text, passwordText.Password);
        }
        #endregion
    }
}
