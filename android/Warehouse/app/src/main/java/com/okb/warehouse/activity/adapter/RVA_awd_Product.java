package com.okb.warehouse.activity.adapter;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.okb.warehouse.R;
import com.okb.warehouse.businesslogic.data.Product;

import org.w3c.dom.Text;

import java.util.List;

public class RVA_awd_Product extends RecyclerView.Adapter<RVA_awd_Product.ViewHolder> {
    private List<Product> al_Products;
    private Context context;

    public RVA_awd_Product(Context context, List<Product> al_Products) {
        this.al_Products = al_Products;
        this.context = context;
    }

    public class ViewHolder extends RecyclerView.ViewHolder{
        RelativeLayout rl_listItem;
        TextView tv_pName;
        TextView tv_pDescription;
        TextView tv_pPrice;
        TextView tv_pAmount;

        public ViewHolder(View itemView) {
            super(itemView);
            tv_pName = itemView.findViewById(R.id.lip_tv_pName);
            tv_pDescription = itemView.findViewById(R.id.lip_tv_pDescription);
            tv_pPrice = itemView.findViewById(R.id.lip_tv_pPrice);
            tv_pAmount = itemView.findViewById(R.id.lip_tv_pAmount);
            rl_listItem = itemView.findViewById(R.id.lip_rl_listItem);
        }
    }

    @NonNull
    @Override
    public RVA_awd_Product.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listitem_awd_product, parent,false);
        return new RVA_awd_Product.ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull RVA_awd_Product.ViewHolder holder, int position) {
        Product p = al_Products.get(position);
        holder.tv_pName.setText(p.getName());
        holder.tv_pDescription.setText(p.getDescription());
        holder.tv_pPrice.setText(String.valueOf(p.getPrice() * p.getAmount()) );
        holder.tv_pAmount.setText(String.valueOf(p.getAmount()) + "x");

        holder.rl_listItem.setOnClickListener(v -> {
            //Intent i = new Intent(context, WarehouseDetailsActivity.class);
            //i.putExtra("warehouseId", al_Products.get(position).getId());
            //context.startActivity(i);
        });
    }

    @Override
    public int getItemCount() {
        return al_Products.size();
    }
}
