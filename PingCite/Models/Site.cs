using System.ComponentModel;

namespace PingSite
{
    public class Site:INotifyPropertyChanged
    {
        #region Fields
        private string _url;
        #endregion

        #region Public Properties
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPC("Url");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Private Properties
        private void OnPC(string field)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(field));
        }
        #endregion
    }
}
