using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Dependencies.Graph.Models;
using Dependencies.Graph.Services;

namespace QueryTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private GraphDriverService service;

        public MainWindow()
        {
            InitializeComponent();

            service = new GraphDriverService("bolt://localhost", "", "");

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string searchText;
        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchText)));
            }
        }

        private string result;
        public string Result
        {
            get => result;
            set
            {
                result = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Result)));
            }
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            var assemblies = new[] {
                new Assembly { Name = "Xce.Test.exe", Version = "1.0"},
                new Assembly { Name = "Xce.Test2.dll", Version = "2.0"},
                new Assembly { Name = "Xce.Test3.dll", Version = "2.0"},
                new Assembly { Name = "Xce.Test4.dll", Version = "2.0", IsPartial = true},
                new Assembly { Name = "Xce.Test5.dll", Version = "2.0"},
                new Assembly { Name = "Xce.Test6.dll", Version = "2.0"},
                new Assembly { Name = "Xce.Test7.dll", Version = "2.0"},
                new Assembly { Name = "Xce.Test8.dll", Version = "2.0"},

            };

            assemblies[0].AssembliesReferenced.AddRange(new[] {
                assemblies[1].Name,
                assemblies[2].Name,
                assemblies[3].Name,
                assemblies[5].Name,
                assemblies[7].Name,
            });

            assemblies[5].AssembliesReferenced.Add(assemblies[6].Name);
            assemblies[6].AssembliesReferenced.Add(assemblies[7].Name);

            var assemblyService = new AssemblyService(service, null);

            Task.Run(async () =>
            {
                try
                {
                    await assemblyService.AddAsync(assemblies);
                }
                catch
                {
                    ;
                }
            });
        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            var assemblyService = new AssemblyService(service, null);

            Task.Run(async () =>
            {
                try
                {
                    var result = await assemblyService.SearchAsync(searchText);

                    Result = result.Select(x => x.Name).DefaultIfEmpty().Aggregate((x, y) => $"{x}{Environment.NewLine}{y}");
                }
                catch
                {
                    ;
                }
            });
        }
    }
}
