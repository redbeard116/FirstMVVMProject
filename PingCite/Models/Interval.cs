using System.ComponentModel;

namespace PingSite
{
    public class Interval : INotifyPropertyChanged
    {
        #region Fields
        private int _interval;
        private int id;
        #endregion
        #region Public Properties
        public int Intervalsite
        {
            get
            {
                return _interval;
            }
            set
            {
                _interval = value;
                OnPC("IntervalSite");
            }
        }
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
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