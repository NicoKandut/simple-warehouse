using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WerhausCore;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;

namespace Lagerverwaltung
{
    public class Database
    {
        public static string Path { get; set; }
        public static string Token { get; set; }
        public static HttpClient Client { get; set; }
        public static void init()
        {
            Path = "https://simple-warehouse-api.herokuapp.com";
            Client = new HttpClient();
        }
        public static async Task<Owner> getOwnerAsync()
        {
            try
            {
                Owner owner = null;
                HttpResponseMessage response = await Client.GetAsync(Path + "/user");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    owner = JsonConvert.DeserializeObject<Owner>(response.Content.ReadAsStringAsync().Result);
                }
                if (owner != null)
                    return owner;
                else
                    throw new Exception("Error when trying to get owner!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<Warehouse>> getWarehousesOfOwnerAsync()
        {
            try
            {
                List<Warehouse> warehosues = null;
                HttpResponseMessage response = await Client.GetAsync(Path + "/user/warehouses");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    warehosues = JsonConvert.DeserializeObject<List<Warehouse>>(response.Content.ReadAsStringAsync().Result);
                }
                return warehosues;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while trying to get warehouses!\n" + ex.Message);
            }
        }
        public static async Task<Warehouse> getWarehouseAsync(int warehouseId)
        {
            Warehouse warehouse = null;
            string path = " https://simple-warehouse-api.herokuapp.com/warehouses/" + warehouseId;
            HttpResponseMessage response = await Client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                warehouse = JsonConvert.DeserializeObject<Warehouse>(response.Content.ReadAsStringAsync().Result);
            }
            return warehouse;
        }
        public static async Task<bool> registerAsync(string username, string password)
        {
            try
            {
                string content = JsonConvert.SerializeObject(new OwnerHelp(username, password));
                HttpResponseMessage response = await Client.PostAsync(Path + "/auth/register", new StringContent(content, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<bool> loginAsync(string username, string password)
        {
            string content = JsonConvert.SerializeObject(new OwnerHelp(username, password));
            HttpResponseMessage response = await Client.PostAsync(Path + "/auth/login", new StringContent(content, Encoding.UTF8, "application/json"));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Token = JsonConvert.DeserializeObject<TokenString>(response.Content.ReadAsStringAsync().Result).token;
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Add("Token", Token);
            }
            if (Token != null)
                return true;
            return false;
        }
        public static async Task<bool> logoutAsync()
        {
            try
            {
                //HttpResponseMessage response = await Client.GetAsync(Path + "/auth/logout");
                //if (response.StatusCode == System.Net.HttpStatusCode.OK)
                //    return true;
                //return false;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while logging out!\n" + ex.Message);
            }
        }
        public static async Task<bool> deleteAccountAsync()
        {
            try
            {
                HttpResponseMessage response = await Client.DeleteAsync(Path + "/auth/logout");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting account!\n" + ex.Message);
            }
        }
        public static async Task<bool> addWarehouseAsync(Warehouse warehouse)
        {
            try
            {
                WarehouseHelp helpWarehouse = new WarehouseHelp(warehouse.Name, warehouse.Description, warehouse.Capacity);
                string content = JsonConvert.SerializeObject(helpWarehouse);
                HttpResponseMessage response = await Client.PostAsync(Path + "/user/warehouses", new StringContent(content, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<bool> deleteWarehouseAsnyc()
        {
            throw new NotImplementedException();
        }
        public static async Task<bool> updateOwnerAsync(string newName, string newPassword)
        {
            try
            {
                string content = JsonConvert.SerializeObject(new OwnerHelp(newName, newPassword));
                HttpResponseMessage response = await Client.PutAsync(Path + "/user", new StringContent(content, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<Warehouse>> getWarehousesOfOwnerAsync(int ownerId)
        {
            throw new NotImplementedException();
        }
        public static async Task<List<Product>> getProductsOfWArehouseAsync(int warehouseId)
        {
            throw new NotImplementedException();
        }
    }
}
class OwnerHelp
{
    public string name { get; set; }
    public string password { get; set; }
    public OwnerHelp(string username, string password)
    {
        this.name = username;
        this.password = password;
    }
}
class TokenString
{
    public string token { get; set; }
    public TokenString(string token)
    {
        this.token = token;
    }
}
class WarehouseHelp
{
    public string name { get; set; }
    public string description { get; set; }
    public int capacity { get; set; }

    public WarehouseHelp(string name, string description, int capacity)
    {
        this.name = name;
        this.description = description;
        this.capacity = capacity;
    }
}