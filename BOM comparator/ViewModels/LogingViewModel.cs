using Caliburn.Micro;
using System.Windows.Input;

namespace BOMComparator.ViewModels
{
    internal class LogingViewModel : Screen
    {
        public delegate void Notify();
        public event Notify LoginSucceedEventHandler;

        public void PerformLogin(ShellViewModel shellView)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            //TODO: User validation here
            OnLoginSucceed();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public void OnLoginSucceed()
        {
            LoginSucceedEventHandler?.Invoke();
        }
    }
}
