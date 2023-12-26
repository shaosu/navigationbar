using Prism.Commands;

namespace AvaDemoApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";
         
        public DelegateCommand<string> GoToCommand => new DelegateCommand<string>(GoToCommand_Sub);
        private void GoToCommand_Sub(string par)
        {
            System.Diagnostics.Debug.WriteLine(par);
        }
    }
}
