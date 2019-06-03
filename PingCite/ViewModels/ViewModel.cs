using Npgsql;
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

        public string connectionString = "Server=127.0.0.1;User Id=postgres;Password=nagimullin;Database=SiteStatus;";

        public ViewModel()
        {
            Sites = new ObservableCollection<Site>();
            GetInterval();
            GetSite();
            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.IsBackground = true;
            t.Start();
        }
        #endregion

        #region Fields
        private Site _site;
        private string _url;
        private int _interval;
        private string _status;
        private int _id;

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
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            npgsqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand("DELETE FROM urlsite WHERE idsite = "+SelectedSite.Id+";", npgsqlConnection);
            command.ExecuteReader();
            npgsqlConnection.Close();
            Sites.Remove(SelectedSite);
        }

        private void OnPC(string field)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(field));
        }

        private void Add(object obj)
        {
            Site site = new Site();
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            npgsqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO urlsite (url) values ('" + Url + "') RETURNING idsite;", npgsqlConnection);
            command.ExecuteReader();
            npgsqlConnection.Close();
            site.Url = Url;
            Sites.Add(site);
            SelectedSite = site;
        }

        private void GetSite()
        {
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            npgsqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT idsite, url FROM urlsite", npgsqlConnection);
            NpgsqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string result = reader.GetString(1);
                        int _id = reader.GetInt32(0);
                        Site site = new Site();
                        site.Url = result;
                        site.Id = _id;
                        Sites.Add(site);
                    }
                }
                reader.Close();
                npgsqlConnection.Close();
            }
            catch (Exception ex)
            {
            }
        }

        private void GetInterval()
        {
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            npgsqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT interval FROM intervalsite", npgsqlConnection);
            NpgsqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    int result = reader.GetInt32(0);
                    Interval = result;
                }
                reader.Close();
                npgsqlConnection.Close();
            }
            catch (Exception ex)
            {
            }
        }

        private void IntervalSiteRequest(object obj)
        {
            Interval interReq = new Interval();
            interReq.Intervalsite = Interval;
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            npgsqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand("UPDATE intervalsite SET interval ='" + interReq.Intervalsite + "';", npgsqlConnection);
            command.ExecuteNonQuery();
            npgsqlConnection.Close();
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
            catch(Exception ex) { }
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

        public ObservableCollection<Site> Sites { get; set; }
        #endregion
    }
}