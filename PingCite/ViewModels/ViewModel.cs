using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;

namespace PingSite
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Constructor
        public ObservableCollection<Site> Sites { get; set; }

        public ViewModel(IDBRepo dBRepo)
        {
            Sites = new ObservableCollection<Site>();
            _dBRepo = dBRepo;
            var list = _dBRepo.GetSite();
            foreach (Site _site in list.ToList())
                Sites.Add(_site);

            var listInterval = _dBRepo.GetInterval();
            Interval = listInterval.Intervalsite;

            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.IsBackground = true;
            t.Start();
        }

        #endregion

        #region Fields
        private IDBRepo _dBRepo;
        private Site _site;
        private string _url;
        private int _interval;
        private string _status;
        private int _id;
        #endregion

        #region Command
        public RelayCommand DeleteCmd => new RelayCommand(Delete, CanDelete);

        public RelayCommand AddCmd => new RelayCommand(Add, CanAdd);

        public RelayCommand IntervalRequest => new RelayCommand(IntervalSiteRequest, CanSiteRequest);

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Properties
        private bool CanDelete(object arg)
        {
            if (SelectedSite != null)
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
            _dBRepo.Delete(SelectedSite);
            Sites.Remove(SelectedSite);
        }

        private void OnPC(string field)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(field));
        }

        private void Add(object obj)
        {
           var list = _dBRepo.AddSite(Url);
            Sites.Add(list);
        }

        private void IntervalSiteRequest(object obj)
        {
            _dBRepo.IntervalSiteRequest(Interval);
        }

        #endregion

        #region Public Properties
        public void ThreadProc()
        {
            try
            {
                for (int i = 0; i < Sites.Count; i++)
                {
                    using (var webClient = new WebClient())
                    {
                        var tempList = Sites.ToList();

                        foreach (Site _qwe in tempList)
                        {
                            _qwe.Status = "Подключение";
                            string url = _qwe.Url;
                            try
                            {
                                var response = webClient.DownloadString(url);
                                if (response != null)
                                {
                                    _qwe.Status = "Доступен";
                                }
                            }
                            catch
                            {
                                _qwe.Status = "Не доступен";
                            }
                        }
                    }
                    Thread.Sleep(Interval * 1000);
                }
            }
            catch (Exception ex) { throw; }
        }

        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                OnPC("Status");
            }
        }

        public Site SelectedSite
        {
            get
            {
                return _site;
            }
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

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
                OnPC("Id");
            }
        }

        #endregion
    }
}