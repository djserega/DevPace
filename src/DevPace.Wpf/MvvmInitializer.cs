using Microsoft.Extensions.DependencyInjection;
using System;

namespace DevPace.Wpf
{
    internal class MvvmInitializer
    {
        private static readonly IServiceProvider _provider;

        static MvvmInitializer()
        {
            ServiceCollection services = new();

            services.AddSingleton<ViewModels.MainWindowViewModel>();

            services.AddTransient<ViewModels.CustomersListViewModel>();
            services.AddTransient<ViewModels.CustomerViewModel>();

            _provider = services.BuildServiceProvider();
        }

#pragma warning disable CS8714 
        public static T? Resolve<T>() => _provider.GetRequiredService<T>();
#pragma warning restore CS8714 

        public ViewModels.MainWindowViewModel? MainWindow { get => Resolve<ViewModels.MainWindowViewModel>(); }
        public ViewModels.CustomersListViewModel? Customers { get => Resolve<ViewModels.CustomersListViewModel>(); }
        public ViewModels.CustomerViewModel? Customer { get => Resolve<ViewModels.CustomerViewModel>(); }
    }
}
