package com.okb.warehouse.activity.warehouse;

import android.content.Intent;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.activity.base.BaseActivity;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Warehouse;

import java.util.InputMismatchException;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CreateWarehouseActivity extends BaseActivity {
    private TextView tv_wname;
    private TextView tv_wdescription;
    private TextView tv_wcapacity;
    private Button btn_create;
    private Button btn_cancel;
    private Warehouse whouse;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_create_warehouse);

        DisplayMetrics dm = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getMetrics(dm);

        int width = dm.widthPixels;
        int height = dm.heightPixels;

        getWindow().setLayout((int)(width*.8), (int)(height*.6));

        initUIReferences();
        initEventHandlers();
    }

    private void initUIReferences(){
        tv_wname = findViewById(R.id.acw_tv_name);
        tv_wdescription = findViewById(R.id.acw_tv_description);
        tv_wcapacity = findViewById(R.id.acw_tv_capacity);
        btn_create = findViewById(R.id.acw_btn_create);
        btn_cancel = findViewById(R.id.acw_btn_cancel);
    }

    private void createWarehouse(){
        String name = tv_wname.getText().toString();
        String description = tv_wdescription.getText().toString();
        Integer capacity = Integer.parseInt(tv_wcapacity.getText().toString());

        try{
            checkInput(name, description, capacity);
            whouse = new Warehouse(name, description, capacity);
        }catch(Exception ex){
            Toast.makeText(this, "Error: " + ex.getMessage(), Toast.LENGTH_LONG).show();
            clearFields();
        }
    }

    private void checkInput(String name, String description, Integer capacity)throws InputMismatchException {
        if (name == null || name.equals("")){
            throw new InputMismatchException("Enter a warehouse name.");
        }

        if (description == null || description.equals("")){
            throw new InputMismatchException("Enter a description.");
        }

        if (capacity == null || capacity.equals("")){
            throw new InputMismatchException("Enter a capacity.");
        }
    }

    private void clearFields(){
        tv_wname.setText("");
        tv_wdescription.setText("");
        tv_wcapacity.setText("");
    }

    private void initEventHandlers() {
        CreateWarehouseActivity awActivity = this;
        this.btn_create.setOnClickListener(v -> {
            awActivity.createWarehouse();
            //region Create Warehouse
            ApiUtils.getService().createWarehouse(sp.getString("token", null), awActivity.whouse).enqueue(new Callback<Void>() {
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    if (response.isSuccessful()) {
                        Toast.makeText(awActivity, "Successful", Toast.LENGTH_LONG).show();
                        awActivity.clearFields();
                        awActivity.startActivity(new Intent(getApplicationContext(), UserWarehousesActivity.class));
                        awActivity.finish();
                    } else {  // error response, no access to resource
                        if (response.code() == 403) {
                            Toast.makeText(awActivity, "Error: ", Toast.LENGTH_LONG).show();
                        } else {
                            Log.e("in UserWarehouse", response.errorBody().toString());
                            Toast.makeText(awActivity, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                        }
                    }
                }

                @Override
                public void onFailure(Call<Void> call, Throwable t) {  //something went completely wrong (eg. no internet connection)
                    Toast.makeText(awActivity, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
                }
            });
            //endregion
        });

        this.btn_cancel.setOnClickListener(v -> {
            awActivity.clearFields();
            awActivity.finish();
        });
    }
}
