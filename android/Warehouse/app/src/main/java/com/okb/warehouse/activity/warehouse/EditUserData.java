package com.okb.warehouse.activity.warehouse;

import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.activity.base.BaseActivity;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Credentials;
import com.okb.warehouse.businesslogic.data.SimpleUser;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class EditUserData extends BaseActivity {
    private EditText et_username, et_oldPassword, et_confirmNewPassword, et_newPassword;
    private Button btn_cancel, btn_change;

    SimpleUser userCredentials;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContent(R.layout.activity_edit_user_data);

        initUIReferences();
        fillViews();
        initEventHandlers();
    }

    private void initUIReferences() {
        et_username = findViewById(R.id.aeud_et_username);
        et_oldPassword = findViewById(R.id.aeud_et_oldPassword);
        et_newPassword = findViewById(R.id.aeud_et_newPassword);
        et_confirmNewPassword = findViewById(R.id.aeud_et_confirmNewPassword);

        btn_cancel = findViewById(R.id.aeud_btn_cancel);
        btn_change = findViewById(R.id.aeud_btn_change);
    }

    private void fillViews(){
        getUser();

        this.et_username.setText(userCredentials.getName());
        this.et_newPassword.getText().clear();
        this.et_confirmNewPassword.getText().clear();
    }

    private void initEventHandlers(){
        btn_cancel.setOnClickListener(v->{
            this.finish();
        });

        btn_change.setOnClickListener(v->{
            try {
                checkPassword(this.et_oldPassword.getText().toString(), this.et_newPassword.getText().toString(), this.et_confirmNewPassword.getText().toString());
                Credentials changedUserData = new Credentials(et_username.getText().toString(), et_newPassword.getText().toString());
                EditUserData eudActivity = this;
                ApiUtils.getService().changeCredentials(sp.getString("token", null), changedUserData).enqueue(new Callback<Void>() {
                    @Override
                    public void onResponse(Call<Void> call, Response<Void> response) {
                        Toast.makeText(eudActivity, "Update Successful", Toast.LENGTH_LONG).show();
                        fillViews();
                    }

                    @Override
                    public void onFailure(Call<Void> call, Throwable t) {
                        Toast.makeText(eudActivity, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
                    }
                });

            }catch(Exception ex){
                Toast.makeText(this, "Error: " + ex.getMessage(), Toast.LENGTH_LONG).show();
            }

        });
    }

    private void getUser(){
        EditUserData eudActivity = this;
        ApiUtils.getService().getCredentials(sp.getString("token", null)).enqueue(new Callback<SimpleUser>() {
            @Override
            public void onResponse(Call<SimpleUser> call, Response<SimpleUser> response) {
                if (response.isSuccessful()) {
                    eudActivity.userCredentials = response.body();
                }else{
                    Toast.makeText(eudActivity, "Can't get user", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<SimpleUser> call, Throwable t) {
                Toast.makeText(eudActivity, "Can't get user", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void checkPassword(String oldPassword, String newPassword, String confirmNewPassword) throws Exception{
        if (!userCredentials.getPassword().equals(oldPassword)){
            throw new Exception("This is the wrong password.");
        }

        if (newPassword != confirmNewPassword){
            throw new Exception("Enter your new Password again.");
        }
    }

}
