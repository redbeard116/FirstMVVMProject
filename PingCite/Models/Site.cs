using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PingSite
{
    public class Site:INotifyPropertyChanged
    {
        #region Fields
        private string _url;
        private string _status;
        private int _id;
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
        [NotMapped]
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
