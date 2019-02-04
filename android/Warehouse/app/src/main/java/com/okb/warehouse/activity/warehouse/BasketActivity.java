package com.okb.warehouse.activity.warehouse;

import android.content.Intent;
import android.support.design.widget.FloatingActionButton;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.activity.adapter.RVA_OrderProduct;
import com.okb.warehouse.activity.base.BaseActivity;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.OrderProduct;
import com.okb.warehouse.businesslogic.data.Product;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class BasketActivity extends BaseActivity {
    private int wId;
    private List<Product> l_products;

    private TextView tv_orderHeading;
    private FloatingActionButton fab_addProduct;
    private RecyclerView rv_products;
    private Button btn_create;
    private Button btn_cancel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_create_order);

        wId = getIntent().getIntExtra("wid", 0);
        l_products = new ArrayList<>();

        initUIReferences();
        initEventHandlers();
        fillRecyclerView();
    }

    private void initUIReferences(){
        this.tv_orderHeading = findViewById(R.id.aco_tv_orderHeading);
        this.rv_products = findViewById(R.id.aco_rv_products);
        this.btn_create = findViewById(R.id.aco_btn_create);
        this.btn_cancel = findViewById(R.id.aco_btn_cancel);
        this.fab_addProduct = findViewById(R.id.aco_fab_addProduct);

        this.tv_orderHeading.setText(R.string.h_Buy);
    }

    private void initEventHandlers(){
        BasketActivity ba = this;
        this.fab_addProduct.setOnClickListener(v ->{
            //TODO: fab_Add Product
        });

        this.btn_create.setOnClickListener(v -> {
            ApiUtils.getService().createOrder(sp.getString("token", null), wId, this.convertProductsToOrderP()).enqueue(new Callback<Void>(){
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    if (response.isSuccessful()) {
                        Toast.makeText(ba, "Order successful", Toast.LENGTH_LONG).show();
                        l_products.clear();
                    }else{
                        Toast.makeText(ba, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                    }
                }

                @Override
                public void onFailure(Call<Void> call, Throwable t) {
                    Toast.makeText(ba, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
                }
            });
        });

        this.btn_cancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startActivity(new Intent(getApplicationContext(), WarehouseDetailsActivity.class).putExtra("wId", warehouseID));
            }
        });


    }

    private void fillRecyclerView(){
        this.rv_products.setAdapter(new RVA_OrderProduct(getApplicationContext(), l_products));
        this.rv_products.setLayoutManager(new LinearLayoutManager(this));
    }

    private List<OrderProduct> convertProductsToOrderP(){
        List<OrderProduct> op = new ArrayList<OrderProduct>();
        for(Product p : l_products){
            op.add(new OrderProduct(p));
        }
        return op;
    }

}
