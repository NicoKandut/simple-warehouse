package com.okb.warehouse.activity.adapter;

import android.content.Context;
import android.content.Intent;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.okb.warehouse.R;
import com.okb.warehouse.activity.warehouse.UserWarehousesActivity;
import com.okb.warehouse.activity.warehouse.WarehouseDetailsActivity;
import com.okb.warehouse.businesslogic.data.Warehouse;

import java.util.List;

public class RVA_Warehouse extends RecyclerView.Adapter<RVA_Warehouse.ViewHolder>{
    private List<Warehouse> al_Warehouses;
    private Context context;

    public RVA_Warehouse(Context context, List<Warehouse> al_Warehouses) {
        this.al_Warehouses = al_Warehouses;
        this.context = context;
    }

    public class ViewHolder extends RecyclerView.ViewHolder{
        LinearLayout rl_listItem;
        TextView tv_wName;
        TextView tv_wDescription;
        TextView tv_wCapacity;

        public ViewHolder(View itemView) {
            super(itemView);
            tv_wName = itemView.findViewById(R.id.liw_tv_wName);
            tv_wDescription = itemView.findViewById(R.id.liw_tv_wDescription);
            tv_wCapacity = itemView.findViewById(R.id.liw_tv_wCapacity);
            rl_listItem = itemView.findViewById(R.id.liw_rl_listItem);
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
        holder.tv_wCapacity.setText(al_Warehouses.get(position).getStringCapacity());

        int id = al_Warehouses.get(position).getId();
        Context uwa = context;

        holder.rl_listItem.setOnClickListener(v -> {
            Intent i = new Intent(uwa, WarehouseDetailsActivity.class);
            i.putExtra("warehouseId", id);
            context.startActivity(i);
        });
    }

    @Override
    public int getItemCount() {
        return al_Warehouses.size();
    }
}
