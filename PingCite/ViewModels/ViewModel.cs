using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PingSite
{
    public class ViewModel : INotifyPropertyChanged
    {

        #region Fields
        private Site _site;
        private string _url;
        private int _interval;

        public RelayCommand DeleteCmd => new RelayCommand(Delete, CanDelete);

        public RelayCommand AddCmd => new RelayCommand(Add, CanAdd);

        public RelayCommand IntervalRequest => new RelayCommand(IntervalSiteRequest, CanSiteRequest);

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Properties
        private bool CanDelete(object arg)
        {
            if (SelectedSite !=null)
                return true;
            return false;
        }
        
        private bool CanAdd(object arg)
        {   
           
             return !string.IsNullOrWhiteSpace(Url);
        }

        private bool CanSiteRequest(object arg)
        {
            return true;
        }

        private void Delete(object obj)
        {
                Sites.Remove(SelectedSite);
        }

        private void OnPC(string field)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(field));
        }

        private void Add(object obj)
        {
            Site site = new Site();
            site.Url = Url;
            Sites.Add(site);
            SelectedSite = site;
        }

        private void IntervalSiteRequest(object obj)
        {
            Interval interReq = new Interval();
            interReq.Intervalsite = Interval;
        }

        #endregion

        #region Public Properties
        public Site SelectedSite
        {
            get { return _site; }
            set
            {
                _site = value;
                OnPC("selectedSite");
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                OnPC("Url");
            }
        }

        public int Interval
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
                OnPC("Interval");
            }
        }

        public ViewModel()
        {
            Sites = new ObservableCollection<Site>
            {
                new Site{Url="google"},
                new Site{Url="bing"},
                new Site{Url="yahoo"},
                new Site{Url="yandex"},
            };

        }

        public void RequestSite()
        {
            string url = "https://www.google.com/";
            using (var webClient = new WebClient())
            {
                var response = webClient.DownloadString(url);
            }
        }

        public ObservableCollection<Site> Sites { get; set; }
        #endregion
    }
}