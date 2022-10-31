using IWshRuntimeLibrary;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using Updater.HW;
using Updater.UI;

namespace Updater.ViewModel
{
    class UpdateViewModel : INotifyPropertyChanged
    {
        #region Имя программы
        private string _programName;
        /// <summary>
        /// Имя программы.
        /// </summary>
        public string ProgramName
        {
            get
            {
                return _programName;
            }
            set
            {
                _programName = value;
                OnPropertyChanged("ProgramName");
            }
        }
        #endregion

        /// <summary>
        /// Версия программы из файла.
        /// </summary>
        private float ProgramVersion { get; set; } = 0.0f;
        /// <summary>
        /// Версия программы из Элмы.
        /// </summary>
        private float ElmaVersion { get; set; } = 0.0f;
        /// <summary>
        /// Версия программы из Элмы.
        /// </summary>
        private string StringVersion { get; set; } = "";
        /// <summary>
        /// Новая папка с программой.
        /// </summary>
        string NewFolder = "";
        private static WshShellClass WshShell;

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        /// <summary>
        /// ID программы.
        /// </summary>
        private int ProgramID = 0;
        public UpdateViewModel()
        {
            CheckDiskConnection();
            GetProgramVersion();
            Task GetVersion = new Task(GetElmaVersion);
            GetVersion.Start();
            GetVersion.Wait();
            NewFolder = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) + ProgramName + "\\";
            if (ProgramVersion < ElmaVersion)
            {
                GetNewVersion();
            }

            #region Запустить основную программу и закрыть updater
            Process.Start(NewFolder + "\\" + ProgramName + ".exe");
            Environment.Exit(0);
            #endregion
        }
        /// <summary>
        /// Проверить подключение к диску
        /// </summary>
        private void CheckDiskConnection()
        {
            #region Проверить диск
            DiskLogin diskLogin;
            if (!Directory.Exists(@"\\10.59.4.45\Documents_1\"))  //Если диск не подключен, то вызываем окно с авторизацией
            {
                diskLogin = new DiskLogin(DiskLogin.WhereWeGo.Documents);
                diskLogin.ShowDialog();
            }
            #endregion
        }

        /// <summary>
        /// Скачать новую версию
        /// </summary>
        private void GetNewVersion()
        {
            string SourceFile = $@"\\10.59.4.45\Documents_1\Производство\Производство\ManufacturesSoft\{ProgramName} v{StringVersion.Replace(',', '.')}\{ProgramName}\bin\Debug\";

            Directory.CreateDirectory(NewFolder);

            #region Создать идентичное дерево каталогов
            foreach (string dirPath in Directory.GetDirectories(SourceFile, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourceFile, NewFolder));
            #endregion

            #region Скопировать все файлы. И перезаписать(если такие существуют)
            foreach (var b in Directory.GetFiles(SourceFile, "*.*", SearchOption.AllDirectories))
                if (!b.Contains("Updater.exe"))
                {
                    System.IO.File.Copy(b, b.Replace(SourceFile, NewFolder), true);
                }
                else
                {
                    try
                    {
                        System.IO.File.Copy(b, b.Replace(SourceFile, NewFolder), true);
                    }
                    catch { }
                }
            #endregion

            #region Изменить версию в файле
            XmlDocument doc = new XmlDocument();
            doc.Load("Version.xml");
            doc.GetElementsByTagName("version")[0].InnerText = StringVersion;
            doc.Save(NewFolder + "Version.xml");
            #endregion

            #region Создать ярлык на рабочем столе
            CreateLinkOnDesctop(NewFolder + "\\Updater.exe", NewFolder, ProgramName);
            #endregion
        }
        /// <summary>
        /// Создать ссылку на рабочем столе.
        /// </summary>
        public static void CreateLinkOnDesctop(string SourceFile, string WorkingDirectory, string LinkName)
        {
            // Create a new instance of WshShellClass
            WshShell = new WshShellClass();
            // Create the shortcut
            IWshShortcut MyShortcut;
            // Choose the path for the shortcut
            MyShortcut = (IWshShortcut)WshShell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + LinkName + ".lnk");
            // Where the shortcut should point to
            MyShortcut.TargetPath = SourceFile;
            //Установить рабочую папку
            MyShortcut.WorkingDirectory = WorkingDirectory;
            MyShortcut.Save();
        }
        /// <summary>
        /// Получить версию программы из файла.
        /// </summary>
        private void GetProgramVersion()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Version.xml");
            ProgramVersion = Convert.ToSingle(doc.GetElementsByTagName("version")[0].InnerText.Replace('.', ','));
            ProgramID = Convert.ToInt32(doc.GetElementsByTagName("programId")[0].InnerText);
            ProgramName = doc.GetElementsByTagName("programName")[0].InnerText;
        }
        /// <summary>
        /// Получить номер версии из Элмы.
        /// </summary>
        private void GetElmaVersion()
        {
            string SQLQuery = $@"select Versiya from ReglamentirovaniePOOpredelen
                                where id = {ProgramID}";
            var result = Elma.SqlQuery(SQLQuery);
            if (result != null)
            {
                if (result.Count != 0)
                {
                    StringVersion = result[0].ToString();
                    ElmaVersion = float.Parse(result[0].ToString().Replace(".", ","));
                    return;
                }
            }
            MessageBox.Show("Нет подключения к Элме");
            Environment.Exit(0);
        }
    }
}
