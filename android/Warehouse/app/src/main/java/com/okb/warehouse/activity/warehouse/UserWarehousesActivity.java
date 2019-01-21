package com.okb.warehouse.activity.warehouse;

import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.activity.adapter.RVA_Warehouse;
import com.okb.warehouse.activity.base.BaseActivity;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Warehouse;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class UserWarehousesActivity extends BaseActivity {
    private RecyclerView rv_warehouses;
    private FloatingActionButton fab_addWarehouse;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContent(R.layout.activity_user_warehouses);

        initUIReferences();
        initEventHandlers();
        fillRecyclerView();
    }

    private void initUIReferences(){
        this.rv_warehouses = findViewById(R.id.auw_rv_warehouses);
        this.fab_addWarehouse = findViewById(R.id.auw_fab_addWarehouse);
    }

    private void initEventHandlers(){
        this.fab_addWarehouse.setOnClickListener(view -> startActivity(new Intent(UserWarehousesActivity.this, CreateWarehouseActivity.class)));
    }

    private void fillRecyclerView(){
        //region Warhouse Service
        UserWarehousesActivity uwActivity = this;

        ApiUtils.getService().getWarehousesFromUser(sp.getString("token", null)).enqueue(new Callback<List<Warehouse>>() { //asynchronous request
            @Override
            public void onResponse(Call<List<Warehouse>> call, Response<List<Warehouse>> response) {
                if (response.isSuccessful()){
                    uwActivity.rv_warehouses.setAdapter(new RVA_Warehouse(uwActivity, response.body()));
                    uwActivity.rv_warehouses.setLayoutManager(new LinearLayoutManager(uwActivity));
                }else{  // error response, no access to resource
                    if (response.code() == 403){
                        Toast.makeText(uwActivity, "Error: ", Toast.LENGTH_LONG).show();
                    }else{
                        Log.e("in UserWarehouse", response.errorBody().toString());
                        Toast.makeText(uwActivity, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                    }
                }
            }

            @Override
            public void onFailure(Call<List<Warehouse>> call, Throwable t) {  //something went completely wrong (eg. no internet connection)
                Toast.makeText(uwActivity, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
            }
        });
        //endregion
    }
}
