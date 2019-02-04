package com.okb.warehouse.activity.account;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Credentials;

import java.util.InputMismatchException;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RegisterActivity extends AppCompatActivity {

    //UI reference
    private Button btn_Register, btn_Back;
    private EditText editText_Username, editText_Password, editText_ConfirmPwd;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        initUIReferences();
        initEventHandlers();
    }

    private void initUIReferences() {
        btn_Register = findViewById(R.id.ar_btn_register);
        btn_Back = findViewById(R.id.ar_btn_back);

        editText_Username = findViewById(R.id.ar_et_username);
        editText_Password = findViewById(R.id.ar_et_password);
        editText_ConfirmPwd = findViewById(R.id.ar_et_confirmPassword);
    }

    private void initEventHandlers(){
        RegisterActivity registerActivity = this;
        btn_Register.setOnClickListener(view -> {
            try {
                Credentials c = new Credentials(registerActivity.editText_Username.getText().toString(), registerActivity.editText_Password.getText().toString());
                checkInput(c.getName(), c.getPassword(), registerActivity.editText_ConfirmPwd.getText().toString());
                //region register Service
                ApiUtils.getService().registerUser(c).enqueue(new Callback<Void>() { //asynchronous request
                    @Override
                    public void onResponse(Call<Void> call, Response<Void> response) {
                        if (response.isSuccessful()) { // response is successful
                            Toast.makeText(registerActivity, "You are registered please log in.", Toast.LENGTH_LONG).show();
                            registerActivity.startActivity(new Intent(getApplicationContext(), LoginActivity.class));
                        } else {  // error response, no access to resource
                            if (response.code() == 403) {
                                Toast.makeText(registerActivity, "Error: Your register data is wrong. Try again or login.", Toast.LENGTH_LONG).show();
                            } else {
                                Toast.makeText(registerActivity, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                            }
                            registerActivity.setTextFieldsToNull();
                        }
                    }
                    @Override
                    public void onFailure(Call<Void> call, Throwable t) {  //something went completely wrong (eg. no internet connection)
                        Toast.makeText(registerActivity, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
                    }
                });
                //endregion
            }catch(Exception ex){
                Toast.makeText(registerActivity, "Error: " + ex.getMessage(), Toast.LENGTH_LONG).show();
            }
        });

        btn_Back.setOnClickListener(view-> this.startActivity(new Intent(this, LoginActivity.class)));

    }

    private void checkInput(String username, String pwd, String confirmPwd)throws InputMismatchException{
        if (username == null || username.equals("")){
            throw new InputMismatchException("Enter a username.");
        }

        if (pwd == null || pwd.equals("")){
            throw new InputMismatchException("Enter a password.");
        }

        if (confirmPwd == null || confirmPwd.equals("")){
            throw new InputMismatchException("Enter the password again.");
        }

        if (!pwd.equals(confirmPwd)){
            throw new InputMismatchException("The password and his confirmation does not match.");
        }
    }

    private void setTextFieldsToNull(){
        editText_Username.setText("");
        editText_Password.setText("");
        editText_ConfirmPwd.setText("");
    }
}
