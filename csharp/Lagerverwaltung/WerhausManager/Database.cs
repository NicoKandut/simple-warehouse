using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WerhausCore;

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
                    owner = JsonConvert.DeserializeObject<Owner>(response.Content.ReadAsStringAsync().Result);
                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden || response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new Exception(JsonConvert.DeserializeObject<ErrorObject>(response.Content.ReadAsStringAsync().Result).ToString());
                if (owner != null)
                    return owner;
                else
                    throw new Exception("No owner found");
            }
            catch (Exception ex)
            {
                throw new Exception("Error while trying to get owner!", ex);
            }
        }
        public static async Task<List<Warehouse>> getWarehousesOfOwnerAsync()
        {
            try
            {
                List<Warehouse> warehouses = null;
                HttpResponseMessage response = await Client.GetAsync(Path + "/user/warehouses");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    warehouses = JsonConvert.DeserializeObject<List<Warehouse>>(response.Content.ReadAsStringAsync().Result);
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception("Not allowed to get warehouses!");
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    throw new Exception("Wrong user credentials");
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new Exception("Error happened on server!");
                return warehouses;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while trying to get warehouses!", ex);
            }
        }
        public static async Task<Warehouse> getWarehouseAsync(int warehouseId)
        {
            Warehouse warehouse = null;
            string path = " https://simple-warehouse-api.herokuapp.com/user/warehouses/" + warehouseId;
            HttpResponseMessage response = await Client.GetAsync(path);
            if (response.IsSuccessStatusCode)
                warehouse = JsonConvert.DeserializeObject<Warehouse>(response.Content.ReadAsStringAsync().Result);
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new Exception("Not allowed to get warehouse!");
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new Exception("Wrong user credentials");
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new Exception("Error happended on server!");
            return warehouse;
        }
        public static async Task<bool> registerAsync(string username, string password)
        {
            try
            {
                string content = JsonConvert.SerializeObject(new OwnerHelp(username, password));
                HttpResponseMessage response = await Client.PostAsync(Path + "/auth/register", new StringContent(content, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return true;
                else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    throw new Exception(JsonConvert.DeserializeObject<ErrorObject>(response.Content.ReadAsStringAsync().Result).ToString());
                else if(response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new Exception(JsonConvert.DeserializeObject<ErrorObject>(response.Content.ReadAsStringAsync().Result).ToString());
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
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new Exception(JsonConvert.DeserializeObject<ErrorObject>(response.Content.ReadAsStringAsync().Result).ToString());
            if (Token != null)
                return true;
            return false;
        }
        public static async Task<bool> logoutAsync()
        {
            try
            {
                HttpResponseMessage response = await Client.GetAsync(Path + "/auth/logout");
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception("No token sent!");
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    throw new Exception(JsonConvert.DeserializeObject<ErrorObject>(response.Content.ReadAsStringAsync().Result).ToString());
                return false;
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
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception(JsonConvert.DeserializeObject<ErrorObject>(response.Content.ReadAsStringAsync().Result).ToString());
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    throw new Exception(JsonConvert.DeserializeObject<ErrorObject>(response.Content.ReadAsStringAsync().Result).ToString());
                else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new Exception(JsonConvert.DeserializeObject<ErrorObject>(response.Content.ReadAsStringAsync().Result).ToString());
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
                string content = JsonConvert.SerializeObject(new WarehouseHelp(warehouse.Name, warehouse.Description, warehouse.Capacity));
                HttpResponseMessage response = await Client.PostAsync(Path + "/user/warehouses", new StringContent(content, Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                    return true;
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception("Not allowed to add warehouse!");
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    throw new Exception("Wrong user credentials");
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new Exception("Error happended on server!");
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
                return true;
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new Exception("Not allowed to delete warehouse!");
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                throw new Exception("Wrong user credentials");
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new Exception("Error happended on server!");
            return false;
        }
        public static async Task<bool> updateOwnerAsync(string newName, string newPassword)
        {
            try
            {
                HttpResponseMessage response = await Client.PutAsync(Path + "/user", new StringContent(JsonConvert.SerializeObject(new OwnerHelp(newName, newPassword)), Encoding.UTF8, "application/json"));
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception("Not allowed to change user!");
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    throw new Exception("Wrong user credentials");
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while trying to update user!", ex);
            }
        }
        public static async Task<List<ProductBase>> getAllProductsOfCatalogAsync()
        {
            try
            {
                List<ProductBase> products = null;
                HttpResponseMessage response = await Client.GetAsync(Path + "/catalog/products");
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    products = JsonConvert.DeserializeObject<List<ProductBase>>(response.Content.ReadAsStringAsync().Result);
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
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new Exception("Warehouse is full!");
                else
                    throw new Exception("Error while trying to add order!");
            }
            catch(Exception ex)
            {
                throw new Exception("Error while trying to add order!", ex);
            }
        }
        public static async Task<List<Product>> getProductsOfWarehouseAsync(int warehouseId)
        {
            try
            {
                List<Product> products = null;
                HttpResponseMessage response = await Client.GetAsync(Path + "/user/warehouses/" + warehouseId + "/products");
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                    products = JsonConvert.DeserializeObject<List<Product>>(response.Content.ReadAsStringAsync().Result);
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception("Not allowed to delete warehouse!");
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    throw new Exception("Wrong user credentials");
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new Exception("Error happended on server!");
                return products;
            }
            catch(Exception ex)
            {
                throw new Exception("Error happended while trying to delete warehouse!", ex);
            }
        }
        public static async Task<List<Order>> getAllOrdersOfWarehouse(int warehouseId)
        {
            try
            {
                List<Order> orders = null;
                HttpResponseMessage response = await Client.GetAsync(Path + "/user/warehouses/" + warehouseId + "/orders");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    orders = JsonConvert.DeserializeObject<List<Order>>(response.Content.ReadAsStringAsync().Result);
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception("Not allowed to delete warehouse!");
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    throw new Exception("Wrong user credentials");
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new Exception("Error happended on server!");
                return orders;
            }
            catch(Exception ex)
            {
                throw new Exception("Error happened while trying to get orders!", ex);
            }
        }
    }
}
class ErrorObject
{
    public string message { get; set; }
    public string details { get; set; }
    public ErrorObject(string message, string details)
    {
        this.message = message;
        this.details = details;
    }
    public override string ToString()
    {
        return message + "\n" + details;
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