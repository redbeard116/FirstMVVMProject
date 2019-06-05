using System.Collections.Generic;

namespace PingSite
{
    public interface IDBRepo
    {
        Site AddSite(string url);

        bool Delete(Site _selectedSite);

        bool IntervalSiteRequest(int _interval);

        List<Site> GetSite();

        Interval GetInterval();
    }
}
