using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace DevPace.Wpf.ViewModels
{
    public class CustomersListViewModel : BindableBase
    {
        public CustomersListViewModel()
        {
            Task.Run(async () => await GetCustomerList(default));

            Models.EventsHub.GetInstance().OpenCustomerEvent += async (object? sender, Models.Customer? e) =>
            {
                await GetCustomerList();
            };

        }


        #region Commands

        public ICommand AddCustomerCommand
        {
            get => new DelegateCommand(() =>
            {
                Models.EventsHub.GetInstance().OpenCustomer(this, new Models.Customer());
            }, () => !IsUpdatingList);
        }
        public ICommand OpenCustomerCommand
        {
            get => new DelegateCommand(() =>
            {
                Models.EventsHub.GetInstance().OpenCustomer(this, CurrentCustomer);
            }, () => CurrentCustomer != null && !IsUpdatingList);
        }
        public ICommand RemoveCustomerCommand
        {
            get => new DelegateCommand(async () =>
            {
                if (CurrentCustomer != default)
                {
                    InvokeAction(async () => await RemoveCustomer(CurrentCustomer));
                }
            }, () => CurrentCustomer != null && !IsUpdatingList);
        }
        public ICommand UpdateListCustomerCommand
        {
            get => new DelegateCommand(async () =>
            {
                await GetCustomerList();
            }, () => !IsUpdatingList);
        }

        #endregion

        public bool IsUpdatingList { get; set; } = true;
        public Visibility VisibilityUpdating { get; set; } = Visibility.Collapsed;


        #region Customers

        private string? _textToFilterCustomers;
        public string? TextToFilterCustomers
        {
            get => _textToFilterCustomers;
            set
            {
                _textToFilterCustomers = value;
                InitFilterCustomers();
            }
        }

        public Models.Customer? CurrentCustomer { get; set; }
        public ICollectionView? CustomersView { get; private set; }
        public ObservableCollection<Models.Customer> Customers { get; set; } = new();

        private void InitFilterCustomers()
        {
            if (CustomersView != null)
                CustomersView.Filter = CustomersFilter;
        }
        private bool CustomersFilter(object obj)
        {
            bool result = true;

            if (obj is Models.Customer row)
            {
                if (result && !string.IsNullOrWhiteSpace(TextToFilterCustomers))
                {
                    result = TextToFilterContainsInFields(row.Name)
                        || TextToFilterContainsInFields(row.CompanyName)
                        || TextToFilterContainsInFields(row.Phone)
                        || TextToFilterContainsInFields(row.Email);
                }
            }

            return result;
        }
        private bool TextToFilterContainsInFields(string? value)
            => value?.Contains(TextToFilterCustomers!, StringComparison.OrdinalIgnoreCase) ?? false;


        #endregion


        private async Task GetCustomerList(ApiConnector.Connector? connector = default)
        {
            IsUpdatingList = true;
            VisibilityUpdating = Visibility.Visible;

            if (connector == default)
                connector = new();

            IEnumerable<Models.Customer>? customers = default;

            customers = await InvokeActionAsync(connector.GetCustomerList);

            if (customers == default)
                Customers = new();
            else
                Customers = new(customers);

            CustomersView = CollectionViewSource.GetDefaultView(Customers);

            IsUpdatingList = false;
            VisibilityUpdating = Visibility.Collapsed;
        }

        private async Task RemoveCustomer(Models.Customer customer)
        {
            IsUpdatingList = true;
            VisibilityUpdating = Visibility.Visible;

            ApiConnector.Connector connector = new();

            await connector.RemoveCustomer(customer);
            await GetCustomerList(connector);

            IsUpdatingList = false;
            VisibilityUpdating = Visibility.Collapsed;
        }

        private void InvokeAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task<T?> InvokeActionAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return default;
            }
        }

    }
}
