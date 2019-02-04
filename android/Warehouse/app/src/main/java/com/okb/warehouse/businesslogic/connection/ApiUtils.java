package com.okb.warehouse.businesslogic.connection;

public class ApiUtils {
    public static final String Base_URL = "https://simple-warehouse-api.herokuapp.com";

    public static ConnectionService getService(){
        return RetrofitClient.getClient(Base_URL).create(ConnectionService.class);
    }
}
