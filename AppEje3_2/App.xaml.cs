using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppEje3_2
{
    public partial class App : Application
    {
        static Controllers.DBProc dBProc;

        public static Controllers.DBProc Instancia
        {
            get
            {
                if (dBProc == null)
                {
                    String dbname = "Proc.db3";
                    String dbpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    String dbfulp = Path.Combine(dbpath, dbname);
                    dBProc = new Controllers.DBProc(dbfulp);
                }

                return dBProc;
            }
        }

        public static string DbFullPath
        {
            get
            {
                String dbname = "Proc.db3";
                String dbpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(dbpath, dbname);
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Views.PageAlumnos());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
