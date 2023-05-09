using DevExpress.Mvvm;
using System;
using System.Windows;
using System.Windows.Input;

namespace DevPace.Wpf.ViewModels
{
    public class CustomerViewModel : BindableBase
    {
        public CustomerViewModel()
        {
            Models.EventsHub.GetInstance().OpenCustomerEvent += (object? sender, Models.Customer? e) =>
            {
                Data = e;
            };
        }

        public Models.Customer? Data { get; set; }


        #region Commands

        public ICommand BackCommand
        {
            get => new DelegateCommand(() =>
            {
                Models.EventsHub.GetInstance().OpenCustomer(this, null);
            });
        }
        public ICommand SaveCommand
        {
            get => new DelegateCommand(async () =>
            {
                if (Data != default)
                {
                    ApiConnector.Connector connector = new();
                    try
                    {
                        Data = await connector.AddCustomer(Data);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }, () => Data != default);
        }
        public ICommand RemoveCommand
        {
            get => new DelegateCommand(async () =>
            {
                if (Data != default)
                {
                    ApiConnector.Connector connector = new();
                    try
                    {
                        await connector.RemoveCustomer(Data);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }, () => !(Data == default || Data.Id == default));
        }

        #endregion

    }
}
