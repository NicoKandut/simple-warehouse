package com.okb.warehouse.activity.account;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.okb.warehouse.R;

import java.util.InputMismatchException;

public class LoginActivity extends AppCompatActivity {

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
        btn_Login.setOnClickListener(view ->{
            try{


            }catch (Exception ex){
                Toast.makeText(LoginActivity.this, ex.getMessage(), Toast.LENGTH_LONG);
                ex.printStackTrace();
            }
        });

        btn_Register.setOnClickListener(view -> startActivity(new Intent(LoginActivity.this, RegisterActivity.class)));
    }

    private void checkInput(String username, String pwd){
        if (username == null || username.equals("")){
            new InputMismatchException("Enter a username");
        }

        if (pwd == null || pwd.equals("")){
            new InputMismatchException("Enter a password");
        }
    }
}
