using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using _0131MVCTest3.Models;
using Microsoft.Extensions.Configuration;

namespace _0131MVCTest3.DAL
{
    public class DALCity
    {
        private IConfiguration _configuration;

        public DALCity(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int addCity(City city)
        {
            string connStr = _configuration.GetConnectionString("MyConnString");
            SqlConnection conn= new SqlConnection(connStr);
            conn.Open();

            
            string query = "INSERT INTO [dbo].[CityTable2]([Name],[Date],[ConfirmedNum],[Death],[Recovery]) VALUES (@cName,@cDate,@cConfirmedNum,@cDeath,@cRecovery) select @@IDENTITY";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@cName", city.Name);
            cmd.Parameters.AddWithValue("@cDate", city.Date);
            cmd.Parameters.AddWithValue("@cConfirmedNum", city.ConfirmedNum);
            cmd.Parameters.AddWithValue("@cDeath", city.Death);
            cmd.Parameters.AddWithValue("@cRecovery", city.Recovery);

            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int uID = Convert.ToInt32(reader[0].ToString());

            conn.Close();
                  

            return uID;
        }

        internal List<City> getCityByDate(DateTime cDate)
        {
            //Step 1
            string connStr = _configuration.GetConnectionString("MyConnString");
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd;
            conn.Open();

            //Step 2
            string query;
            string sDate = cDate.ToShortDateString();

            if (cDate == null)
            {
                query = "SELECT * FROM [dbo].[CityTable2] order by date";
                cmd = new SqlCommand(query, conn);

            }
            else
            {
                query = "SELECT * FROM [dbo].[CityTable2] where Date=@sDate";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sDate", sDate);
            }



            //Step 3
            SqlDataReader reader = cmd.ExecuteReader();
            List<City> lc = new List<City>();

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    City temp = new City();
                    temp.Name = reader["Name"].ToString();
                    temp.Date = cDate;
                    temp.ConfirmedNum = Convert.ToInt32(reader["ConfirmedNum"]);
                    temp.Death = Convert.ToInt32(reader["Death"]);
                    temp.UID = Convert.ToInt32(reader["ID"]);
                    try
                    {
                        temp.Recovery = Convert.ToInt32(reader["Recovery"]);
                    }
                    catch
                    {
                        temp.Recovery = 0;
                    }

                    lc.Add(temp);
                }
                //Step 4
                conn.Close();

                return lc;

            }
            else
            {

                conn.Close();
                return null;

            }
        }

        public City getCity(int uID)
        {
            City city = new City();

            //Step 1
            string connStr = _configuration.GetConnectionString("MyConnString");
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            //Step 2
            string query = "SELECT * FROM [dbo].[CityTable2] where ID=@uID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@uID", uID);

            //Step 3
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();

            

            city.Name = reader["Name"].ToString();
            city.Date = Convert.ToDateTime(reader["Date"]);
            //city.Date = city.Date.Date;
            string ddd = reader["Date"].ToString();
            city.ConfirmedNum = Convert.ToInt32(reader["ConfirmedNum"]);
            city.Death = Convert.ToInt32(reader["Death"]);
            city.Recovery = Convert.ToInt32(reader["Recovery"]);
            city.UID = uID;
            
            
            //Step 4
            conn.Close();


            
            return city;
        }

        public List<City> getCityByName(string cName)
        {

            //Step 1
            string connStr = _configuration.GetConnectionString("MyConnString");
            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd;
            conn.Open();

            //Step 2
            string query;
            if (cName == "1")
            {
                query = "SELECT * FROM [dbo].[CityTable2] order by date";
                cmd = new SqlCommand(query, conn);

            }
            else
            {
                query = "SELECT * FROM [dbo].[CityTable2] where Name=@cName order by Date";
                cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@cName", cName);
            }
            
            

            //Step 3
            SqlDataReader reader = cmd.ExecuteReader();
            List<City> lc = new List<City>();

            if (reader.HasRows)
            {
                
                while (reader.Read())
                {
                    City temp = new City();
                    temp.Name = reader["Name"].ToString();
                    temp.Date = Convert.ToDateTime(reader["Date"]);
                    temp.Date = temp.Date.Date;
                    temp.ConfirmedNum = Convert.ToInt32(reader["ConfirmedNum"]);
                    temp.Death = Convert.ToInt32(reader["Death"]);
                    temp.UID = Convert.ToInt32(reader["ID"]);
                    try
                    {
                        temp.Recovery = Convert.ToInt32(reader["Recovery"]);
                    } catch
                    {
                        temp.Recovery = 0;
                    }
                    
                    lc.Add(temp);
                }
                //Step 4
                conn.Close();
                
                return lc;

            } else
            {

                conn.Close();
                return null;

            }
            
            
        }

        internal void deleteCity(int uID)
        {
            //Step 1
            string connStr = _configuration.GetConnectionString("MyConnString");
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();


            //Step 2
            string query = "DELETE FROM [dbo].[CityTable2] WHERE id=@uID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@uID", uID);

            //Step 3
            cmd.ExecuteNonQuery();

            //Step 4
            conn.Close();

        }

        public void updateCity(City city)
        {
                 
            //Step 1
            string connStr = _configuration.GetConnectionString("MyConnString");
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            //Step 2
            string query = "UPDATE [dbo].[CityTable2]   SET [Name] = @cName,[ConfirmedNum] =@cConfirmedNum,[Death] = @cDeath,[Recovery] =@cRecovery WHERE id=@uID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@cName", city.Name);
            cmd.Parameters.AddWithValue("@cConfirmedNum", city.ConfirmedNum);
            cmd.Parameters.AddWithValue("@cDeath", city.Death);
            cmd.Parameters.AddWithValue("@cRecovery", city.Recovery);
            cmd.Parameters.AddWithValue("@uID", city.UID);

            //Step 3
            cmd.ExecuteNonQuery();


            //Step 4
            conn.Close();

        }
    }
}
