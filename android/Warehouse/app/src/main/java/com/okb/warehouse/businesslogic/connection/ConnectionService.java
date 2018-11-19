package com.okb.warehouse.businesslogic.connection;

import com.google.gson.JsonObject;
import com.okb.warehouse.businesslogic.data.Credentials;
import com.okb.warehouse.businesslogic.data.Warehouse;

import org.json.JSONObject;

import java.lang.String;
import java.sql.Connection;
import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.Field;
import retrofit2.http.FormUrlEncoded;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.Headers;
import retrofit2.http.POST;
import retrofit2.http.Path;

/**
 * provides all paths for the webservice
 */
public interface ConnectionService {
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

    //Logout
    @DELETE("/auth/delete")
    Call<Void> deleteUser(@Header("Token") String token);

    //userinfo whit warehouses
    @GET("/user")
    Call<JsonObject> getUser(@Header("Token") String token);

    //warehouses from user
    @GET("/user/warehouses")
    Call<List<Warehouse>> getWarehousesFromUser(@Header("Token") String token);

    //get one Warehouse
    @GET("/user/warehouses/{w_id}")
    Call<Warehouse> getOneWarehouse(@Header ("Token") String token, @Path("w_id") int id);

}
