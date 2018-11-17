using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using System;
using Teste03.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Teste03
{
    public partial class App : Application
	{
       // public int idCliente = 8;
        
        public App ()
		{
            // Models.Session.Instance.IdCliente = 8;
            // Models.Session.Instance.Cnome = "Teste OK!";

            InitializeComponent();

            MainPage = new Views.LgHome();

            /*
            if(Models.Session.Instance.IdCliente == 0)
            {
                MainPage = new Views.Login();
            }
            else
            {
                MainPage = new Views.LgHome();
            } */

        }
        
        protected override void OnStart ()
		{

        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
            // Handle when your app resumes
        }
    }
}
