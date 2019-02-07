package com.okb.warehouse.activity.warehouse;

import android.content.Intent;
import android.support.design.widget.FloatingActionButton;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.activity.adapter.RVA_awd_Product;
import com.okb.warehouse.activity.base.BaseActivity;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Product;
import com.okb.warehouse.businesslogic.data.Warehouse;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class WarehouseDetailsActivity extends BaseActivity {
    private TextView tv_warehouseDescription;
    private ProgressBar pbar_capacity;
    private RecyclerView rv_products;
    private Button btn_back;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContent(R.layout.activity_warehouse_details);

        warehouseID = getIntent().getIntExtra("warehouseId", 0);
        initUIReferences();
        fillViews();
        initEventHandlers();
    }

    private void initUIReferences(){
        this.tv_warehouseDescription = findViewById(R.id.awd_tv_description);
        this.pbar_capacity = findViewById(R.id.awd_pbar_capacity);
        this.rv_products = findViewById(R.id.awd_rv_products);
        this.btn_back = findViewById(R.id.awd_btn_back);
    }

    private int calculateProgress(List<Product> Capacities){
        int toReturn = 0;
        for (Product p : Capacities){
            toReturn = toReturn + (int)p.getAmount();
        }
        return toReturn;
    }

    private void fillViews(){
        //region Products Service
        WarehouseDetailsActivity wdActivity = this;
        ApiUtils.getService().getOneWarehouse(sp.getString("token", null), this.warehouseID).enqueue(new Callback<Warehouse>(){
            @Override
            public void onResponse(Call<Warehouse> call, Response<Warehouse> response) {
                if (response.isSuccessful()){
                    wdActivity.toolbar.setTitle(response.body().getName());
                    wdActivity.tv_warehouseDescription.setText(response.body().getDescription());
                    wdActivity.pbar_capacity.setMax(response.body().getCapacity());
                    wdActivity.pbar_capacity.setProgress(wdActivity.calculateProgress(response.body().getProducts()));
                    wdActivity.rv_products.setAdapter(new RVA_awd_Product(wdActivity, response.body().getProducts()));
                    wdActivity.rv_products.setLayoutManager(new LinearLayoutManager(wdActivity));
                }else{
                    Toast.makeText(wdActivity, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<Warehouse> call, Throwable t) {
                Toast.makeText(wdActivity, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
            }
        });
        //endregion
    }

    private void initEventHandlers(){
        this.btn_back.setOnClickListener(v -> {
            this.startActivity(new Intent(this, UserWarehousesActivity.class));
            this.finish();
        });
    }


}
