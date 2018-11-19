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
        public static Action<HttpResponseMessage> onErrorAction = (HttpResponseMessage response) =>
        {
            string message = response.Content.ReadAsStringAsync().Result.ToString();
            configManager.writeLog(message, WerhausManager.LogType.ERROR);
            throw new Exception(message);
        };
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
                HttpResponseMessage response = await Client.GetAsync(Path + "/user");
                return handleResponse(response, (message) =>{return JsonConvert.DeserializeObject<Owner>(response.Content.ReadAsStringAsync().Result);}, onErrorAction);           
        }
        public static async Task<List<Warehouse>> getWarehousesOfOwnerAsync()
        {
                HttpResponseMessage response = await Client.GetAsync(Path + "/user/warehouses");
                return handleResponse(response, (message) =>{return JsonConvert.DeserializeObject<List<Warehouse>>(message.Content.ReadAsStringAsync().Result);}, onErrorAction);
        }
        public static async Task<Warehouse> getWarehouseAsync(int warehouseId)
        {
            string path = " https://simple-warehouse-api.herokuapp.com/user/warehouses/" + warehouseId;
            HttpResponseMessage response = await Client.GetAsync(path);
            return handleResponse(response, (message) =>{return JsonConvert.DeserializeObject<Warehouse>(response.Content.ReadAsStringAsync().Result);}, onErrorAction);
        }
        public static async Task<bool> registerAsync(string username, string password)
        {
            string content = JsonConvert.SerializeObject(new OwnerHelp(username, password));
            HttpResponseMessage response = await Client.PostAsync(Path + "/auth/register", new StringContent(content, Encoding.UTF8, "application/json"));
            return handleResponse(response, (message) =>{return true;}, onErrorAction);
        }
        public static async Task<bool> loginAsync(string username, string password)
        {
            string content = JsonConvert.SerializeObject(new OwnerHelp(username, password));
            HttpResponseMessage response = await Client.PostAsync(Path + "/auth/login", new StringContent(content, Encoding.UTF8, "application/json"));
            return handleResponse(response, (message) =>
            {
                Token = JsonConvert.DeserializeObject<TokenString>(response.Content.ReadAsStringAsync().Result).token;
                Client.DefaultRequestHeaders.Clear();
                Client.DefaultRequestHeaders.Add("Token", Token);
                return true;
            }, onErrorAction);
        }
        public static async Task<bool> logoutAsync()
        {
            HttpResponseMessage response = await Client.GetAsync(Path + "/auth/logout");
            return handleResponse(response, (message) => { return true; }, onErrorAction);
        }
        public static async Task<bool> deleteAccountAsync()
        {
            HttpResponseMessage response = await Client.DeleteAsync(Path + "/auth/delete");
            return handleResponse(response, (message) => { return true; }, onErrorAction);
        }
        public static async Task<bool> addWarehouseAsync(Warehouse warehouse)
        {
            string content = JsonConvert.SerializeObject(new WarehouseHelp(warehouse.Name, warehouse.Description, warehouse.Capacity));
            HttpResponseMessage response = await Client.PostAsync(Path + "/user/warehouses", new StringContent(content, Encoding.UTF8, "application/json"));
            return handleResponse(response, (message) => { return true; }, onErrorAction);
        }
        public static async Task<bool> deleteWarehouseAsnyc(int warehouseId)
        {
            HttpResponseMessage response = await Client.DeleteAsync(Path + "/user/warehouses/" + warehouseId);
            return handleResponse(response, (message) => { return true; }, onErrorAction);
        }
        public static async Task<bool> updateOwnerAsync(string newName, string newPassword)
        {
            HttpResponseMessage response = await Client.PutAsync(Path + "/user", new StringContent(JsonConvert.SerializeObject(new OwnerHelp(newName, newPassword)), Encoding.UTF8, "application/json"));
            return handleResponse(response, (message) => { return true; }, onErrorAction);
        }
        public static async Task<List<ProductBase>> getAllProductsOfCatalogAsync()
        {
            HttpResponseMessage response = await Client.GetAsync(Path + "/catalog/products");
            return handleResponse(response, (message) => { return JsonConvert.DeserializeObject<List<ProductBase>>(response.Content.ReadAsStringAsync().Result); }, onErrorAction);
        }
        public static async Task<bool> addOrderAsync(Order _order)
        {
            List<OrderHelp> productList = new List<OrderHelp>();
            foreach (KeyValuePair<ProductBase, int> p in _order.Amounts)
                productList.Add(new OrderHelp(p.Key.Id, p.Value));
            string content = JsonConvert.SerializeObject(productList);
            HttpResponseMessage response = await Client.PostAsync(Path + "/user/warehouses/" + _order.IdWarehouse + "/orders", new StringContent(content, Encoding.UTF8, "application/json"));
            return handleResponse(response, (message) => { return true; }, onErrorAction);
        }
        public static async Task<List<Product>> getProductsOfWarehouseAsync(int warehouseId)
        {
            HttpResponseMessage response = await Client.GetAsync(Path + "/user/warehouses/" + warehouseId + "/products");
            return handleResponse(response, (message) => { return JsonConvert.DeserializeObject<List<Product>>(response.Content.ReadAsStringAsync().Result); }, onErrorAction);
        }
        public static async Task<List<Order>> getAllOrdersOfWarehouse(int warehouseId)
        {
            HttpResponseMessage response = await Client.GetAsync(Path + "/user/warehouses/" + warehouseId + "/orders");
            return handleResponse(response, (message) => { return JsonConvert.DeserializeObject<List<Order>>(response.Content.ReadAsStringAsync().Result); }, onErrorAction);
        }
        private static T handleResponse<T>(HttpResponseMessage response, Func<HttpResponseMessage, T> onSuccess, Action<HttpResponseMessage> onError)
        {
            if (response.IsSuccessStatusCode)
                return onSuccess(response);
            else
                onError(response);
            throw new Exception("Unexpedted Error!");
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
    public int id { get; set; }
    public int amount { get; set; }

    public OrderHelp(int id_product, int amount)
    {
        this.id = id_product;
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