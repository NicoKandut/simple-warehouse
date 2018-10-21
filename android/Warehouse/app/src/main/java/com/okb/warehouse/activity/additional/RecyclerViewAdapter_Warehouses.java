package com.okb.warehouse.activity.additional;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.okb.warehouse.R;

import java.util.ArrayList;
import java.util.List;

public class RecyclerViewAdapter_Warehouses extends RecyclerView.Adapter<RecyclerViewAdapter_Warehouses.ViewHolder>{

    private List<String> al_WarehouseNames;
    private List<String> al_WarehouseDescription;
    private List<String> al_WarehouseCapacity;
    private Context context;

    public RecyclerViewAdapter_Warehouses(Context context, List<String> al_WarehouseNames, List<String> al_WarehouseDescription, List<String> al_WarehouseCapacity) {
        this.al_WarehouseNames = al_WarehouseNames;
        this.al_WarehouseDescription = al_WarehouseDescription;
        this.al_WarehouseCapacity = al_WarehouseCapacity;
        this.context = context;
    }

    public class ViewHolder extends RecyclerView.ViewHolder{
        RelativeLayout rl_listitem;
        TextView tv_name;
        TextView tv_description;
        TextView tv_capacity;

        public ViewHolder(View itemView) {
            super(itemView);
            tv_name = itemView.findViewById(R.id.tv_name);
            tv_description = itemView.findViewById(R.id.tv_description);
            tv_capacity = itemView.findViewById(R.id.tv_capacity);
            rl_listitem = itemView.findViewById(R.id.rv_warehouses);
        }
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.layout_listitem, parent,false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.tv_name.setText(al_WarehouseNames.get(position));
        holder.tv_name.setText(al_WarehouseDescription.get(position));
        holder.tv_name.setText(al_WarehouseCapacity.get(position));

        holder.rl_listitem.setOnClickListener(v -> {
            //TODO: on click listener for items to get to warehouse details
            Toast.makeText(context, al_WarehouseNames.get(position), Toast.LENGTH_LONG).show();
        });

    }

    @Override
    public int getItemCount() {
        return al_WarehouseNames.size();
    }


}
