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
            string path = " https://simple-warehouse-api.herokuapp.com/user/warehouses/" + warehouseId;
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
                HttpResponseMessage response = await Client.DeleteAsync(Path + "/auth/delete");
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
        public static async Task<bool> deleteWarehouseAsnyc(int warehouseId)
        {
            HttpResponseMessage response = await Client.DeleteAsync(Path + "/user/warehouses/" + warehouseId);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
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
        public static async Task<List<ProductBase>> getAllProductsOfCatalogAsync()
        {
            try
            {
                List<ProductBase> products = new List<ProductBase>();
                HttpResponseMessage response = await Client.GetAsync(Path + "/catalog/products");
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    products = JsonConvert.DeserializeObject<List<ProductBase>>(response.Content.ReadAsStringAsync().Result);
                }
                return products;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<bool> addOrderAsync(Order _order)
        {
            try
            {
                string content = JsonConvert.SerializeObject(new OrderHelp(_order.Amounts.ElementAt(0).Key.Id, _order.Amounts[_order.Amounts.ElementAt(0).Key]));
                HttpResponseMessage response = await Client.PostAsync(Path + "/user/warehouses/" + _order.IdWarehouse + "/orders", new StringContent(content, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return true;
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;
                else
                    throw new Exception("Error while trying to add order!");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<List<Product>> getProductsOfWarehouseAsync(int warehouseId)
        {
            try
            {
                List<Product> products = new List<Product>();
                HttpResponseMessage response = await Client.GetAsync(Path + "/user/warehouses/" + warehouseId + "/products");
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    products = JsonConvert.DeserializeObject<List<Product>>(response.Content.ReadAsStringAsync().Result);
                }
                return products;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
class OrderHelp
{
    public int id_product { get; set; }
    public int amount { get; set; }

    public OrderHelp(int id_product, int amount)
    {
        this.id_product = id_product;
        this.amount = amount;
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