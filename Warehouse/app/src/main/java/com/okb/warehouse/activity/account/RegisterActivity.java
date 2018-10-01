package com.okb.warehouse.activity.account;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.okb.warehouse.R;

public class RegisterActivity extends AppCompatActivity {

    //UI reference
    private Button btn_Register;
    private EditText editText_Username, editText_Password, editText_ConfirmPwd;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        initUIReferences();
        initEventHandlers();
    }

    private void initUIReferences() {
        btn_Register = findViewById(R.id.btn_register);

        editText_Username = findViewById(R.id.editText_username);
        editText_Password = findViewById(R.id.editText_password);
        editText_ConfirmPwd = findViewById(R.id.editText_confirmPassword);
    }

    private void initEventHandlers(){
        btn_Register.setOnClickListener(view -> {
            try {

            }catch (Exception ex){
                Toast.makeText(RegisterActivity.this, ex.getMessage(), Toast.LENGTH_LONG);
                ex.printStackTrace();
            }
        });

    }
}
