package com.okb.warehouse.activity.warehouse;

import android.content.Intent;
import android.support.design.widget.FloatingActionButton;
import android.support.v7.app.AppCompatActivity;
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

public class ExportActivity extends BaseActivity {
    private List<Product> l_products;

    private FloatingActionButton fab_addProduct;
    private RecyclerView rv_products;
    private Button btn_create;
    private Button btn_cancel;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContent(R.layout.activity_create_order);

        warehouseID = getIntent().getIntExtra("wid", 0);
        l_products = new ArrayList<>();

        getSupportActionBar().setTitle(R.string.h_Sell);

        initUIReferences();
        initEventHandlers();
        fillRecyclerView();
        this.setToolbarTitle();
    }

    private void initUIReferences(){
        this.rv_products = findViewById(R.id.aco_rv_products);
        this.btn_create = findViewById(R.id.aco_btn_create);
        this.btn_cancel = findViewById(R.id.aco_btn_cancel);
        this.fab_addProduct = findViewById(R.id.aco_fab_addProduct);

    }

    private void setToolbarTitle(){
        ExportActivity ea = this;
        ea.toolbar.setTitle(R.string.h_Sell);
    }

    private void initEventHandlers(){
        ExportActivity ea = this;
        this.fab_addProduct.setOnClickListener(v ->{
            startActivityForResult(new Intent(this, AddProductToOrder.class).putExtra("isImport", false).putExtra("wid", warehouseID), 2);
        });

        this.btn_create.setOnClickListener(v -> {
            ApiUtils.getService().createOrder(sp.getString("token", null), warehouseID, this.convertProductsToOrderP()).enqueue(new Callback<Void>(){
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    if (response.isSuccessful()) {
                        Toast.makeText(ea, "Order successful", Toast.LENGTH_LONG).show();
                        l_products.clear();
                        ea.startActivity(new Intent(getApplicationContext(), WarehouseDetailsActivity.class).putExtra("warehouseId", ea.warehouseID));
                        ea.finish();
                    }else{
                        Toast.makeText(ea, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                    }
                }

                @Override
                public void onFailure(Call<Void> call, Throwable t) {
                    Toast.makeText(ea, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
                }
            });
        });

        this.btn_cancel.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                ea.startActivity(new Intent(ea, WarehouseDetailsActivity.class).putExtra("warehouseId", ea.warehouseID));
                ea.finish();
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
            p.setAmount(p.getAmount() * (-1));
            op.add(new OrderProduct(p));
        }
        return op;
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data){
        if (requestCode == 2 && resultCode == RESULT_OK){
            int productId = data.getIntExtra("product",0);
            int amount = data.getIntExtra("amount", 0);

            getAllProducts(productId, amount);
        }
    }

    private void getAllProducts(int productId, int amount){
        ExportActivity ea = this;
        ApiUtils.getService().getProducts().enqueue(new Callback<List<Product>>(){
            @Override
            public void onResponse(Call<List<Product>> call, Response<List<Product>> response) {
                if (response.isSuccessful()){
                    for (Product p : response.body()){
                        if (p.getId() == productId){
                            p.setAmount(amount);
                            ea.l_products.add(p);
                            ea.fillRecyclerView();
                        }
                    }

                }else{  // error response, no access to resource
                    Toast.makeText(ea, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                }
            }
            @Override
            public void onFailure(Call<List<Product>> call, Throwable t) {  //something went completely wrong (eg. no internet connection)
                Toast.makeText(ea, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
            }
        });
    }
}
