using Npgsql;
using System;
using System.Collections.Generic;

namespace PingSite
{
    public class DBRepo : IDBRepo
    {
        #region ConnectionDB
        private string connectionString = "Server=127.0.0.1;User Id=postgres;Password=nagimullin;Database=Site;";
        #endregion

        #region Public 
        public Site AddSite(string _url)
        {
            Site site = new Site();
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            npgsqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand("INSERT INTO urlsite (Url) values ('" + _url + "') RETURNING Id;", npgsqlConnection);
            NpgsqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    int result = reader.GetInt32(0);
                    site.Id = result;
                    site.Url = _url;
                }
                reader.Close();
                npgsqlConnection.Close();
                return site;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                reader.Close();
                npgsqlConnection.Close();
            }
        }

        public bool Delete(Site _selectedSite)
        {
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            npgsqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand("DELETE FROM urlsite WHERE Id = " + _selectedSite.Id + ";", npgsqlConnection);
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                command.ExecuteReader();
                npgsqlConnection.Close();
            }
        }

        public bool IntervalSiteRequest(int _interval)
        {
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            npgsqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand("UPDATE intervalsite SET Intervalsite ='" + _interval + "';", npgsqlConnection);
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                command.ExecuteReader();
                npgsqlConnection.Close();
            }
        }
        #endregion

        #region GetInfo
        public List<Site> GetSite()
        {
            List<Site> list= new List<Site>();
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            npgsqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT Id, Url FROM urlsite", npgsqlConnection);
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
                        list.Add(site);
                    }
                }
               
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                reader.Close();
                npgsqlConnection.Close();
            }
        }

        public Interval GetInterval()
        {
            Interval interval = new Interval();
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionString);
            npgsqlConnection.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT Intervalsite FROM intervalsite", npgsqlConnection);
            NpgsqlDataReader reader = command.ExecuteReader();
            try
            {
                if (reader.Read())
                {
                    int result = reader.GetInt32(0);
                    interval.Intervalsite = result;
                }
                return interval;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                reader.Close();
                npgsqlConnection.Close();
            }
        }
        #endregion
    }
}
