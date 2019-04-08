package com.okb.warehouse.activity.adapter;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.support.annotation.NonNull;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.PopupMenu;
import android.widget.TableRow;
import android.widget.TextView;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.activity.base.BaseActivity;
import com.okb.warehouse.activity.warehouse.WarehouseDetailsActivity;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Warehouse;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class RVA_Warehouse extends RecyclerView.Adapter<RVA_Warehouse.ViewHolder>{
    private List<Warehouse> al_Warehouses;
    private Context context;

    public RVA_Warehouse(Context context, List<Warehouse> al_Warehouses) {
        this.al_Warehouses = al_Warehouses;
        this.context = context;
    }

    public class ViewHolder extends RecyclerView.ViewHolder{
        TableRow tr_listItem;
        TextView tv_wName;
        TextView tv_wDescription;
        TextView tv_wCapacity;
        View iv_Menu;

        public ViewHolder(View itemView) {
            super(itemView);
            tv_wName = itemView.findViewById(R.id.liw_tv_wName);
            tv_wDescription = itemView.findViewById(R.id.liw_tv_wDescription);
            tv_wCapacity = itemView.findViewById(R.id.liw_tv_wCapacity);
            tr_listItem = itemView.findViewById(R.id.liw_tr_listItem);
            iv_Menu = itemView.findViewById(R.id.liw_iV_Menu);
        }
    }

    @NonNull
    @Override
    public RVA_Warehouse.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listitem_warehouse, parent,false);
        return new RVA_Warehouse.ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.tv_wName.setText(al_Warehouses.get(position).getName());
        holder.tv_wDescription.setText(al_Warehouses.get(position).getDescription());
        holder.tv_wCapacity.setText("Capacity: " + al_Warehouses.get(position).getStringCapacity());

        int id = al_Warehouses.get(position).getId();
        Context uwa = context;

        holder.tr_listItem.setOnClickListener(v -> {
            Intent i = new Intent(uwa, WarehouseDetailsActivity.class);
            i.putExtra("warehouseId", id);
            context.startActivity(i);
        });

        holder.iv_Menu.setOnClickListener(new View.OnClickListener(){
            @Override
            public void onClick(View view){
                //creating a popup menu
                PopupMenu popup = new PopupMenu(context, holder.iv_Menu);
                //inflating menu from xml resource
                popup.inflate(R.menu.menu_order_product);

                //adding click listener
                popup.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
                    @Override
                    public boolean onMenuItemClick(MenuItem item) {
                        switch (item.getItemId()) {
                            case R.id.action_delete:
                                ApiUtils.getService().deleteWarehouse(PreferenceManager.getDefaultSharedPreferences(context).getString("token",null), id).enqueue(new Callback<Void>(){
                                    @Override
                                    public void onResponse(Call<Void> call, Response<Void> response) {
                                        if (response.isSuccessful()){
                                            al_Warehouses.remove(al_Warehouses.get(position));
                                            notifyDataSetChanged();
                                            Toast.makeText(context, "Warehouse successful deleted.", Toast.LENGTH_LONG ).show();
                                        }else{  // error response, no access to resource

                                            Toast.makeText(context, "Error: " + response.code() + " = " + response.errorBody().toString(), Toast.LENGTH_LONG).show();
                                        }
                                    }

                                    @Override
                                    public void onFailure(Call<Void> call, Throwable t) {  //something went completely wrong (eg. no internet connection)
                                        Toast.makeText(context, "Error: " + t.getMessage(), Toast.LENGTH_LONG).show();
                                    }
                                });
                                break;
                        }
                        return false;
                    }
                });
                //displaying the popup
                popup.show();
            }
        });
    }

    @Override
    public int getItemCount() {
        return al_Warehouses.size();
    }
}
