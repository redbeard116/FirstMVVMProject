using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PingSite
{
    public class DBEntity : IDBRepo
    {
        private ApplicationContext db = new ApplicationContext();

        public Site AddSite(string _url)
        {
            Site site = new Site();
            site.Url = _url;
            db.urlsite.Add(site);
            db.SaveChanges();
            return site;
        }

        public bool Delete(Site _selectedSite)
        {
            Site site = db.urlsite.Find(_selectedSite.Id);
            db.urlsite.Remove(site);
            db.SaveChanges();
            return true;
        }

        public Interval GetInterval()
        {
            Interval interval = new Interval();
            var _interval = db.intervalsite;
            foreach (Interval inter in _interval)
                interval.Intervalsite = inter.Intervalsite;
            return interval;
            
        }

        public List<Site> GetSite()
        {
            List<Site> list = new List<Site>();
            var sites = db.urlsite;
            foreach (Site site in sites)
                list.Add(site);
            return list;
        }

        public bool IntervalSiteRequest(int _interval)
        {
            Interval interval = new Interval();
            interval = db.intervalsite.FirstOrDefault();
            interval.Intervalsite = _interval;
            db.SaveChanges();
            return true;
        }
    }
}
