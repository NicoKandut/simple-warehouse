package com.okb.warehouse.activity.base;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.support.annotation.LayoutRes;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.FrameLayout;
import android.widget.PopupMenu;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.activity.account.LoginActivity;
import com.okb.warehouse.activity.warehouse.EditUserData;
import com.okb.warehouse.activity.warehouse.ImportActivity;
import com.okb.warehouse.activity.warehouse.ExportActivity;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Warehouse;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class BaseActivity extends AppCompatActivity {

    // UI references
    private FrameLayout contentContainer;
    protected Toolbar toolbar;

    private int layout;

    protected int warehouseID;
    public SharedPreferences sp;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_base);

        sp = PreferenceManager.getDefaultSharedPreferences(this);

        // set up views
        initToolbar();
        initContainer();
    }

    private void initToolbar() {
        toolbar = findViewById(R.id.ab_toolbar);
        setSupportActionBar(toolbar);
    }

    private void initContainer() {
        contentContainer = findViewById(R.id.ab_content_container);
    }

    protected void setContent(@LayoutRes int layout) {
        LayoutInflater layoutInflater = (LayoutInflater) this.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        if (layoutInflater != null) {
            layoutInflater.inflate(layout, contentContainer);
            this.layout = layout;
        } else {
            System.out.println("layoutInflater was null in setContent of BaseActivity");
        }
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        this.finish();
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_app_bar, menu);
        handleMenuIcons(menu);
        return true;
    }

    private void handleMenuIcons(Menu menu){
        if (layout == R.layout.activity_warehouse_details) {
            menu.findItem(R.id.action_export).setVisible(true);
            menu.findItem(R.id.action_basket).setVisible(true);
        }
        else{
            menu.findItem(R.id.action_export).setVisible(false);
            menu.findItem(R.id.action_basket).setVisible(false);
        }
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        boolean result = true;
        switch (item.getItemId()) {
            case R.id.action_export:
                Intent intentE = new Intent(this, ExportActivity.class);
                intentE.putExtra("wid", warehouseID);
                startActivity(intentE);
                break;

            case R.id.action_basket:
                Intent intentB = new Intent(this, ImportActivity.class);
                intentB.putExtra("wid", warehouseID);
                startActivity(intentB);
                break;

            case R.id.action_accountSettings:
                Context context = this;
                PopupMenu popup = new PopupMenu(this, findViewById(R.id.action_accountSettings));   //creating a popup menu
                popup.inflate(R.menu.menu_app_bar_account_settings);                   //inflating menu from xml resource

                //adding click listener
                popup.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
                    @Override
                    public boolean onMenuItemClick(MenuItem item) {
                        switch (item.getItemId()) {
                            case R.id.action_editUserdata:
                                startActivity(new Intent(context, EditUserData.class));
                                break;

                            case R.id.action_deleteUser:
                                confirmUserDelete(context);
                                break;

                            case R.id.action_logout:
                                ApiUtils.getService().logoutUser(sp.getString("token", null)).enqueue(new Callback<Void>() {
                                    @Override
                                    public void onResponse(Call<Void> call, Response<Void> response) {
                                        startActivity(new Intent(context, LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP)); // opn new activity with closing all opened
                                    }

                                    @Override
                                    public void onFailure(Call<Void> call, Throwable t) {
                                        Toast.makeText(context,"Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
                                    }
                                });
                                break;
                        }
                        return false;
                    }
                });

                popup.show();          //displaying the popup
                break;
            default:
                result = super.onOptionsItemSelected(item);
        }
        return result;
    }

    private void confirmUserDelete(Context context){
        AlertDialog.Builder builder = new AlertDialog.Builder(this);
        builder.setCancelable(true);
        builder.setTitle("Confirm");
        builder.setMessage("Are you sure you want to delete your account?\nATTENTION: with your account all your warehouses and orders are going to be deleted.");


        builder.setPositiveButton("Confirm", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                deleteUserFromDatabase(context);
            }
        });

        builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.cancel();
            }
        });
        builder.create().show();
    }

    private void deleteUserFromDatabase(Context context){
        ApiUtils.getService().deleteUser(sp.getString("token", null)).enqueue(new Callback<Void>() {
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {

                Toast.makeText(context, "Successful delete", Toast.LENGTH_LONG).show();
                startActivity(new Intent(context, LoginActivity.class).addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP)); // opn new activity with closing all opened
            }

            @Override
            public void onFailure(Call<Void> call, Throwable t) {
                Toast.makeText(context, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
            }
        });
    }
}
