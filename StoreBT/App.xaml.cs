using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StoreBT.Repositories;
using StoreBT.Repositories.Interfaces;
using StoreBT.Services;
using StoreBT.Services.Interfaces;
using StoreBT.Views;
using System;
using System.Configuration;
using System.Data;
using System.Windows;

namespace StoreBT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }
        public App()
        {
            var services = new ServiceCollection();

            services.AddDbContext<StoreDbContext>(options =>
                        options.UseSqlite("Data Source=store.db"));

            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IProductService, ProductService>();


            services.AddTransient<CustomerView>();
            services.AddTransient<ProductView>();
            services.AddTransient<CreateOrderView>();

            services.AddTransient<MainWindow>();


            Services = services.BuildServiceProvider();

            UpMigration();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        private void UpMigration()
        {

            using (var scope = Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
                db.Database.Migrate();
            }
        }
    }

}
