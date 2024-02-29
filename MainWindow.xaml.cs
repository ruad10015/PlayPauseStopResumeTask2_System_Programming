using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApp19
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        DispatcherTimer timer = null;

        public Thread checkThread = null;


        private ObservableCollection<string> allDataFrom;
        public ObservableCollection<string> AllDataFrom
        {
            get { return allDataFrom; }
            set { allDataFrom = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> allDataTo;
        public ObservableCollection<string> AllDataTo
        {
            get { return allDataTo; }
            set { allDataTo = value; OnPropertyChanged(); }
        }

        public Thread DownloadDataFromTo { get; set; }


        private string enteredText;
        public string EnteredText
        {
            get { return enteredText; }
            set { enteredText = value; OnPropertyChanged(); }
        }

        public Thread AddThread { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(2000);
            AllDataFrom = new ObservableCollection<string>();
            AllDataTo = new ObservableCollection<string>();

            this.DataContext = this;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DownloadListToOtherList();
        }

        private void txtBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (EnteredText.Trim().Length > 0)
                {
                    AddThread = new Thread(() =>
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            AllDataFrom.Add(EnteredText.Trim());
                            EnteredText = string.Empty;
                        });
                    });
                    AddThread.Start();
                }
                else
                {
                    MessageBox.Show("First need to wright anything !");
                }
            }
        }


        private void DownloadListToOtherList()
        {
            if (AllDataFrom.Count > 0)
            {
                for (int i = 0; i < AllDataFrom.Count; i++)
                {
                    AllDataTo.Add(AllDataFrom[i]);
                    AllDataFrom.RemoveAt(i);
                }
            }
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            DownloadDataFromTo = new Thread(() =>
            {
                timer.Start();
            });
            DownloadDataFromTo.Start();
        }


        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            DownloadDataFromTo.Suspend();
        }

        private void resumeBtn_Click(object sender, RoutedEventArgs e)
        {
            DownloadDataFromTo.Resume();
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            DownloadDataFromTo.Abort();
        }


    }
}
