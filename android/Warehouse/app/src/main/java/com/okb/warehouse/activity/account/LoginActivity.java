package com.okb.warehouse.activity.account;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.google.gson.JsonObject;
import com.okb.warehouse.R;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.connection.ConnectionService;
import com.okb.warehouse.businesslogic.data.Credentials;

import java.util.InputMismatchException;

import oracle.jdbc.util.Login;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoginActivity extends AppCompatActivity {

    private ConnectionService cService;
    //UI reference
    private Button btn_Login, btn_Register;
    private EditText editText_Username, editText_Password;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        initUIReferences();
        initEventHandlers();
    }

    private void initUIReferences() {
        btn_Login = findViewById(R.id.btn_login);
        btn_Register = findViewById(R.id.btn_register);

        editText_Username = findViewById(R.id.editText_username);
        editText_Password = findViewById(R.id.editText_password);
    }

    private void initEventHandlers(){
        LoginActivity self = this; // need instant of acutal activity to use in the new thread for Retrofit client
        btn_Login.setOnClickListener(view ->{
            Credentials c = new Credentials(self.editText_Username.getText().toString(), self.editText_Password.getText().toString());
            checkInput(c.getName(), c.getPassword());

            //region Login Service
            ApiUtils.getService().loginUser(c).enqueue(new Callback<JsonObject>() { //asynchronous request
                @Override
                public void onResponse(Call<JsonObject> call, Response<JsonObject> response) {
                    if (response.isSuccessful()){
                        getSharedPreferences("Userdata", MODE_PRIVATE).edit().putString("actualToken", response.body().get("token").getAsString()); //store actual Token in sharedPreferences
                        self.startActivity(new Intent(getApplicationContext(), UserWarehousesActivity.class));
                    }else{  // error response, no access to resource
                        if (response.code() == 403){
                            Toast.makeText(self, "Error: Your login data is wrong. Try again or register.", Toast.LENGTH_LONG).show();
                        }else{
                            Toast.makeText(self, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                        }
                        self.setTextFieldsToNull();
                    }
                }

                @Override
                public void onFailure(Call<JsonObject> call, Throwable t) {  //something went completely wrong (eg. no internet connection)
                    Toast.makeText(self, "Error: " + t.getMessage(), Toast.LENGTH_LONG);
                }
            });
            //endregion
        });

        btn_Register.setOnClickListener(view -> startActivity(new Intent(LoginActivity.this, RegisterActivity.class)));
    }

    private void checkInput(java.lang.String username, java.lang.String pwd){
        if (username == null || username.equals("")){
            new InputMismatchException("Enter a username");
        }

        if (pwd == null || pwd.equals("")){
            new InputMismatchException("Enter a password");
        }
    }

    private void setTextFieldsToNull(){
        editText_Username.setText("");
        editText_Password.setText("");
    }
}
