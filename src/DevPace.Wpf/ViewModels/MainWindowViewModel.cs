using DevExpress.Mvvm;
using System.Windows;
using System.Windows.Controls;

namespace DevPace.Wpf.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            Models.EventsHub.GetInstance().OpenCustomerEvent += (object? sender, Models.Customer? e) =>
            {
                VisibilityCustomer = e == default ? Visibility.Collapsed : Visibility.Visible;
                VisibilityCustomerList = e == default ? Visibility.Visible : Visibility.Collapsed;
            };
        }

        public Page CustomersList { get; set; } = new Views.CustomersList();
        public Page Customer { get; set; } = new Views.CustomerView();

        public Visibility VisibilityCustomerList { get; set; } = Visibility.Visible;
        public Visibility VisibilityCustomer { get; set; } = Visibility.Collapsed;
    }
}
