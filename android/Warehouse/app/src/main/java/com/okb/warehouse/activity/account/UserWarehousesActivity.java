package com.okb.warehouse.activity.account;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.FloatingActionButton;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.ViewGroup;
import android.widget.Toast;

import com.google.gson.JsonObject;
import com.okb.warehouse.R;
import com.okb.warehouse.activity.additional.RecyclerViewAdapter_Warehouses;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Warehouse;

import java.security.Key;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class UserWarehousesActivity extends AppCompatActivity {
    private RecyclerView rv_warehouses;
    private FloatingActionButton fab_addWarehouse;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_user_warehouses);

        initUIReferences();
        initEventHandlers();
        fillRecyclerView();
    }

    private void initUIReferences(){
        this.rv_warehouses = findViewById(R.id.rv_warehouses);
        this.fab_addWarehouse = findViewById(R.id.fab_addWarehouse);
    }

    private void initEventHandlers(){

    }

    private void fillRecyclerView(){
        //region Werhouse Service
        UserWarehousesActivity uwActivity = this;
        //TODO: change response to list of warehouses
        ApiUtils.getService().getWarehousesFromUser(getSharedPreferences("Userdata", MODE_PRIVATE).getString("token", null)).enqueue(new Callback<Warehouse>() { //asynchronous request
            @Override
            public void onResponse(Call<Warehouse> call, Response<Warehouse> response) {
                if (response.isSuccessful()){
                    //TODO: response of warehouses after key in list

                }else{  // error response, no access to resource
                    if (response.code() == 403){
                        Toast.makeText(uwActivity, "Error: ", Toast.LENGTH_LONG).show();
                    }else{
                        Toast.makeText(uwActivity, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                    }
                }
            }
            @Override
            public void onFailure(Call<Warehouse> call, Throwable t) {  //something went completely wrong (eg. no internet connection)
                Toast.makeText(uwActivity, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
            }
        });
        //endregion
    }
}
