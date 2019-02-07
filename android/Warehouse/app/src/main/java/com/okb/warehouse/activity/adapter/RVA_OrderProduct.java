package com.okb.warehouse.activity.adapter;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.PopupMenu;
import android.widget.TextView;
import android.widget.Toast;

import com.okb.warehouse.R;
import com.okb.warehouse.businesslogic.data.Product;

import java.text.DecimalFormat;
import java.util.List;

public class RVA_OrderProduct extends RecyclerView.Adapter<RVA_OrderProduct.ViewHolder> {
    private List<Product> al_Products;
    private Context context;

    public RVA_OrderProduct(Context context, List<Product> al_Products) {
            this.al_Products = al_Products;
            this.context = context;
    }

    public class ViewHolder extends RecyclerView.ViewHolder{
        LinearLayout tr_listItem;
        TextView tv_pName;
        TextView tv_pPrice;
        TextView tv_pAmount;
        View iv_Menu;

        public ViewHolder(View itemView) {
            super(itemView);
            tv_pName = itemView.findViewById(R.id.liop_tv_pName);
            tv_pPrice = itemView.findViewById(R.id.liop_tv_pPrice);
            tv_pAmount = itemView.findViewById(R.id.liop_tv_pAmount);
            tr_listItem = itemView.findViewById(R.id.liop_tr_listItem);
            iv_Menu = itemView.findViewById(R.id.liop_iV_Menu);
        }
    }

        @NonNull
        @Override
        public RVA_OrderProduct.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.listitem_order_product, parent,false);
            return new RVA_OrderProduct.ViewHolder(view);
        }

        @Override
        public void onBindViewHolder(@NonNull RVA_OrderProduct.ViewHolder holder, int position) {
            Product p = al_Products.get(position);
            DecimalFormat df = new DecimalFormat("###,###,###.00");

            holder.tv_pName.setText(p.getName());
            holder.tv_pPrice.setText(String.valueOf(df.format(p.getPrice() * p.getAmount())) + "€");
            holder.tv_pAmount.setText(String.valueOf((int)p.getAmount()) + " x");

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
                                    al_Products.remove(p);
                                    notifyDataSetChanged();
                                    Toast.makeText(context, "Product successful deleted from List.", Toast.LENGTH_LONG ).show();
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
            return al_Products.size();
        }
}
