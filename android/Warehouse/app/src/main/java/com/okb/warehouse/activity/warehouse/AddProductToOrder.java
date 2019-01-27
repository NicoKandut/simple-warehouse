package com.okb.warehouse.activity.warehouse;

import android.content.Intent;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.activity.base.BaseActivity;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Product;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class AddProductToOrder extends BaseActivity {
    private boolean isImport;

    private Spinner sp_products;
    private EditText et_amount;
    private Button btn_cancel;
    private Button btn_add;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_product_to_order);

        DisplayMetrics dm = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getMetrics(dm);

        int width = dm.widthPixels;
        int height = dm.heightPixels;

        getWindow().setLayout((int)(width*.8), (int)(height*.5));

        isImport = getIntent().getBooleanExtra("isImport", true);
        if (!isImport){
            warehouseID = getIntent().getIntExtra("wid", 0);
        }

        initUIReferences();
        handleFillSpinner();
        initEventHandlers();
    }

    private void initUIReferences(){
        sp_products = findViewById(R.id.aapto_sp_products);
        et_amount = findViewById(R.id.aapto_et_amount);
        btn_add = findViewById(R.id.aapto_btn_add);
        btn_cancel = findViewById(R.id.aapto_btn_cancel);
    }

    private void initEventHandlers(){
        this.btn_add.setOnClickListener(v->{
            try {
                if (Integer.parseInt(et_amount.getText().toString()) != 0) {
                    Product selectedProd = (Product) this.sp_products.getSelectedItem();
                    Intent i = new Intent();
                    i.putExtra("product", selectedProd.getId());
                    i.putExtra("amount", Integer.parseInt(et_amount.getText().toString()));
                    setResult(RESULT_OK, i);
                    finish();
                } else {
                    Toast.makeText(this, "Need an amount to finish process.", Toast.LENGTH_LONG).show();
                }
            }catch(Exception ex){
                Toast.makeText(this, "The data is wrong", Toast.LENGTH_LONG).show();
            }
        });

        this.btn_cancel.setOnClickListener(v -> {
           this.finish();
        });
    }

    private void handleFillSpinner(){
        if (isImport){
            this.getAllProducts();
        }
        else{
            this.getAllProductsInWarehouse();
        }
    }

    private void fillSpinner(List<Product> products){
        if (products != null){
            ArrayAdapter<Product> adapterProduct = new ArrayAdapter<Product>(
                    this,
                    android.R.layout.simple_spinner_item,
                    products
            );
            this.sp_products.setAdapter(adapterProduct);
        }else{
            Toast.makeText(this, "Error: Can't load Products." , Toast.LENGTH_LONG).show();
        }
    }

    private void getAllProducts(){
        AddProductToOrder aptoActivity = this;
        ApiUtils.getService().getProducts().enqueue(new Callback<List<Product>>(){
            @Override
            public void onResponse(Call<List<Product>> call, Response<List<Product>> response) {
                if (response.isSuccessful()){
                    aptoActivity.fillSpinner(response.body());
                }else{  // error response, no access to resource
                    Toast.makeText(aptoActivity, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                }
            }
            @Override
            public void onFailure(Call<List<Product>> call, Throwable t) {  //something went completely wrong (eg. no internet connection)
                Toast.makeText(aptoActivity, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
            }
        });
    }

    private void getAllProductsInWarehouse(){
        AddProductToOrder aptoActivity = this;
        ApiUtils.getService().getProductsFromWarehouse(sp.getString("token", null), warehouseID).enqueue(new Callback<List<Product>>() {
            @Override
            public void onResponse(Call<List<Product>> call, Response<List<Product>> response) {
                if (response.isSuccessful()) {
                    aptoActivity.fillSpinner(response.body());
                }else{
                    Toast.makeText(aptoActivity, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                }
            }

            @Override
            public void onFailure(Call<List<Product>> call, Throwable t) {
                Toast.makeText(aptoActivity, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
            }
        });
    }
}
