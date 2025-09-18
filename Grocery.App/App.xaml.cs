using Grocery.App.ViewModels;
using Grocery.App.Views;

namespace Grocery.App
{
    public partial class App : Application
    {
        public App(LoginViewModel viewModel)// LoginViewModel viewModel wordt beschikbaar gemaakt.
        {
            InitializeComponent();
            //AppShell wordt uitgeschakeld.
            MainPage = new LoginView(viewModel);
        }
    }
}