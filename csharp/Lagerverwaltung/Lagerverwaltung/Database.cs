using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WerhausCore;
using Newtonsoft.Json;
namespace Lagerverwaltung
{
    public class Database 
    {
        
        public static HttpClient client { get; set; }
        public static string connString { get; set; }
        public static OracleConnection conn { get; set; }
        public static void connect(string _connString)
        {
            connString = _connString;
            client = new HttpClient();
            conn = new OracleConnection(connString);
            conn.Open();
        }
        public static async Task<string> getWebserviceInformation()
        {
            string warehouse = "warehouse";
            string path = "https://simple-warehouse-api.herokuapp.com";
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                warehouse = JsonConvert.DeserializeObject(result).ToString();
            }
            return warehouse;
        }
        public static async Task<List<Warehouse>> getWarehousesAsync(int ownerId)
        {
            List<Warehouse> warehouses = null;
            string path = " https://simple-warehouse-api.herokuapp.com/warehouses/" + ownerId;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                warehouses = JsonConvert.DeserializeObject<List<Warehouse>>(response.Content.ReadAsStringAsync().Result);
            }
            return warehouses;

        }
        public static async Task<Warehouse> getWarehouseAsync(int warehouseId)
        {
            Warehouse warehouse = null;
            string path = " https://simple-warehouse-api.herokuapp.com/warehouses/" + warehouseId;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                warehouse = JsonConvert.DeserializeObject<Warehouse>(response.Content.ReadAsStringAsync().Result);
            }
            return warehouse;
        }
        public static async void loginAsync(string username, string password)
        {

        }
        public static async void registerAsync(string username, string password)
        {

        }
        public static void login(string username, string password)
        {
            try
            {
                string select = "select * from sw_owner where name like '" + username + "' and password like '" + password + "'";
                OracleCommand cmd = new OracleCommand(select, conn);
                OracleDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    throw new Exception("Wrong username or password!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void register(string username, string password)
        {
            try
            {
                string select = "insert into sw_owner values(seq_owner.nextval, '" + username + "', '" + password + "')";
                OracleCommand cmd = new OracleCommand(select, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void closeConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                conn.Close();
        }
    }
}
