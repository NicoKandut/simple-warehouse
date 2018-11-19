package com.okb.warehouse.activity.warehouse;

import android.support.design.widget.FloatingActionButton;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.activity.adapter.RVA_Product;
import com.okb.warehouse.activity.adapter.RVA_Warehouse;
import com.okb.warehouse.activity.base.BaseActivity;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Product;
import com.okb.warehouse.businesslogic.data.Warehouse;

import org.w3c.dom.Text;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class WarehouseDetailsActivity extends BaseActivity {
    private int warehouseID;
    private TextView tv_warehouseName;
    private TextView tv_warehouseDescription;
    private ProgressBar pbar_capacity;
    private RecyclerView rv_products;
    private FloatingActionButton fab_addAuftrag;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_warehouse_details);

        warehouseID = getIntent().getIntExtra("warehouseId", 0);

        initUIReferences();
        initEventHandlers();
        fillViews();
    }

    private void initUIReferences(){
        this.tv_warehouseName = findViewById(R.id.awd_tv_warehouseName);
        this.tv_warehouseDescription = findViewById(R.id.awd_tv_description);
        this.pbar_capacity = findViewById(R.id.awd_pbar_capacity);
        this.rv_products = findViewById(R.id.awd_rv_products);
        this.fab_addAuftrag = findViewById(R.id.awd_fab_addAuftrag);
    }

    private void initEventHandlers(){
        //this.fab_addAuftrag.setOnClickListener(this);
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
                    wdActivity.tv_warehouseName.setText(response.body().getName());
                    wdActivity.tv_warehouseDescription.setText(response.body().getDescription());
                    wdActivity.pbar_capacity.setMax(response.body().getCapacity());
                    wdActivity.pbar_capacity.setProgress(wdActivity.calculateProgress(response.body().getProducts()));
                    wdActivity.rv_products.setAdapter(new RVA_Product(wdActivity, response.body().getProducts()));
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
}
