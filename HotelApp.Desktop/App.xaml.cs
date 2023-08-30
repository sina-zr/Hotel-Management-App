using DataAccessLibrary.Data;
using DataAccessLibrary.DataBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider serviceProvider;

        // Overriding the OnStartup method from Application.cs
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Adding MainWindow to our Dependency Injection
            // Adding some DataAccessLibrary instances
            // gives you an instance each time
            var services = new ServiceCollection();
            services.AddTransient<MainWindow>();
            services.AddTransient<ISqlDataAccess, SqlDataAccess>();
            services.AddSingleton<IDataBaseData, SqlData>();
            services.AddTransient<ISQLiteDataAccess, SQLiteDataAccess>();


            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = builder.Build();

            // putting config into Dependency Injection
            services.AddSingleton(config);

            // Talking to our serviceProvider
            serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetService<MainWindow>();

            mainWindow.Show();
        }
    }
}
