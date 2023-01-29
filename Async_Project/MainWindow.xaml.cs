using Async_Project;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Async_RestAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ButtonSyncClick(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            LoadDataSync();
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            out_txt_block.Text += $"Time: {time}";
        }
        private async void ButtonAsyncClick(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            await LoadDataAsyncParallel();
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            out_txt_block.Text += $"Time: {time}\n";
        }
        public void LoadDataSync()
        {
            List<String> sites = PrepareLoadSites();
            foreach (var site in sites)
            {
                DataModel data = LoadSite(site);
                PrintInfo(data);
            }

        }
        public async Task LoadDataAsync()
        {
            List<string> sites = PrepareLoadSites();
            foreach (var site in sites)
            {
                DataModel data = await Task.Run(() => LoadSite(site));
                PrintInfo(data);
            }

        }
        public async Task LoadDataAsyncParallel()
        {
            List<string> sites = PrepareLoadSites();
            List<Task<DataModel>> tasks = new List<Task<DataModel>>();
            foreach (var site in sites)
            {
                tasks.Add(Task.Run(() => LoadSite(site)));
            }
            DataModel[] dataModels = await Task.WhenAll(tasks);
            foreach (var item in dataModels)
            {
                PrintInfo(item);
            }

        }
        public void PrintInfo(DataModel dataModel)
        {
            out_txt_block.Text += $"Url: {dataModel.Url}, Length: {dataModel.Data.Length} \n";
        }

        private List<string> PrepareLoadSites()
        {
            List<string> sites = new List<string>()
            {
                "https://google.com",
                "https://my.progtime.net"
            };
            return sites;
        }

        private DataModel LoadSite(string site)
        {
            DataModel dataModel = new DataModel();
            dataModel.Url = site;

            WebClient webClient = new WebClient();
            dataModel.Data = webClient.DownloadString(site);

            return dataModel;
        }

    }
}
