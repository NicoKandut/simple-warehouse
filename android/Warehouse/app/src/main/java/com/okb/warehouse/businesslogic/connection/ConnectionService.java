package com.okb.warehouse.businesslogic.connection;

import com.google.gson.JsonObject;
import com.okb.warehouse.businesslogic.data.Credentials;
import com.okb.warehouse.businesslogic.data.Order;
import com.okb.warehouse.businesslogic.data.OrderProduct;
import com.okb.warehouse.businesslogic.data.Product;
import com.okb.warehouse.businesslogic.data.Warehouse;


import java.lang.String;
import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.Headers;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

/**
 * provides all paths for the webservice
 */
public interface ConnectionService {
    //region Authentification
    //Login
    @POST("/auth/login")
    @Headers("Content-Type:application/json")
    Call<JsonObject> loginUser(@Body Credentials credentials);

    //Logout
    @GET("/auth/logout")
    Call<Void> logoutUser(@Header("Token") String token);

    //Register
    @POST("/auth/register")
    @Headers("Content-Type:application/json")
    Call<Void> registerUser(@Body Credentials credentials);

    //Delete
    @DELETE("/auth/delete")
    Call<Void> deleteUser(@Header("Token") String token);
    //endregion

    //region user
    //Change User Credentials
    @PUT("/user")
    Call<Void> changeCredentials(@Header("Token") String token, @Body Credentials credentials);

    //get User Details
    @GET("/user?cred=true")
    Call<Credentials> getCredentials(@Header("Token") String token);
    //endregion

    //region warehouses
    //warehouses from user
    @GET("/user/warehouses")
    Call<List<Warehouse>> getWarehousesFromUser(@Header("Token") String token);

    //get one Warehouse
    @GET("/user/warehouses/{w_id}")
    Call<Warehouse> getOneWarehouse(@Header ("Token") String token, @Path("w_id") int id);

    //create one Warehouse
    @POST("/user/warehouses")
    Call<Void> createWarehouse(@Header ("Token") String token, @Body Warehouse warehouse);

    //delete one warehouse
    @DELETE("/user/warehouses/{w_id}")
    Call<Void> deleteWarehouse(@Header ("Token") String token, @Path("w_id") int id);
    //endregion


    //region products
    //get all Products
    @GET("/catalog/products")
    Call<List<Product>> getProducts();

    //get Products from Warehouse
    @GET("/user/warehouses/{w_id}/products")
    Call<List<Product>> getProductsFromWarehouse(@Header("Token") String token, @Path("w_id") int id);
    //endregion


    //region orders
    //handle Orders
    @POST("/user/warehouses/{w_id}/orders")
    Call<Void> createOrder(@Header("Token") String token, @Path("w_id") int id, @Body List<OrderProduct> products);

    @GET("/user/warehouses/{w_id}/orders")
    Call<List<Order>> getOrdersFromWarehouse(@Header("Token") String token, @Path("w_id") int id);
    //endregion

}
