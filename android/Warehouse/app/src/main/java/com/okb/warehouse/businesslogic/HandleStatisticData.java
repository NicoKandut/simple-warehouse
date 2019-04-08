package com.okb.warehouse.businesslogic;
import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.renderscript.Sampler;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.widget.Toast;

import com.anychart.chart.common.dataentry.DataEntry;
import com.anychart.chart.common.dataentry.ValueDataEntry;
import com.okb.warehouse.activity.statisticsFragments.ProductOverviewFragment;
import com.okb.warehouse.businesslogic.connection.ApiUtils;
import com.okb.warehouse.businesslogic.data.Order;
import com.okb.warehouse.businesslogic.data.Product;
import com.okb.warehouse.businesslogic.data.Warehouse;

import java.util.ArrayList;
import java.util.List;

import javax.security.auth.callback.Callback;

import retrofit2.Call;
import retrofit2.Response;

public class HandleStatisticData {
    private static HandleStatisticData instance = null;
    private List<Order> orders;
    private List<Product> products;

    private HandleStatisticData(){

    }

    public static HandleStatisticData newInstance(){
        if (instance == null){
            instance = new HandleStatisticData();
        }
        return instance;
    }

    public List<List<DataEntry>> getInOutData(String token, int warehouseId){
        List<List<DataEntry>> result = new ArrayList<>();
        List<DataEntry> inEntries = new ArrayList<>();
        List<DataEntry> outEntries = new ArrayList<>();
        //getAllOrders(token, warehouseId);

        int capacityIn = 0;
        int capacityOut = 0;
        for(Order o : orders)
        {
            if (o.getAmount() < 0)
            {
                capacityIn += o.getAmount();
                outEntries.add(new ValueDataEntry(o.getDate(), o.getAmount()));
            }
            else
            {
                capacityOut += o.getAmount();
                inEntries.add(new ValueDataEntry(o.getTimestamp().toString(), o.getAmount()));
            }
        }

        result.add(outEntries);
        result.add(inEntries);
        return result;
    }


    public void getHistographyData(String token, int warehouseId, UpdateView caller)
    {
        List<DataEntry> entries = new ArrayList<>();
        ApiUtils.getService().getOrdersFromWarehouse(token, warehouseId).enqueue(new retrofit2.Callback<List<Order>>() {
            @Override
            public void onResponse(Call<List<Order>> call, Response<List<Order>> response) {
                if (response.isSuccessful()){
                    int capacity = 0;
                    for (Order o : response.body())
                    {
                        if (o.getProducts().size() > 0)
                        {
                            capacity = capacity + o.getAmount();
                            entries.add(new ValueDataEntry(o.getDate(), capacity));
                        }
                    }
                    caller.createChart(entries);
                }
            }
            @Override
            public void onFailure(Call<List<Order>> call, Throwable t) {

            }
        });
    }


    public void getProducts(String token, int wid, int capacity, UpdateView caller) {
        final int[] freeSpace = {capacity};
        List<DataEntry> allProducts = new ArrayList<>();
        ApiUtils.getService().getProductsFromWarehouse(token, wid).enqueue(new retrofit2.Callback<List<Product>>() {
            @Override
            public void onResponse(Call<List<Product>> call, Response<List<Product>> response) {
                if (response.isSuccessful()){
                    if (response.body() != null){
                        for (Product p: response.body()){
                            allProducts.add(new ValueDataEntry(p.getName(), p.getAmount()));
                            freeSpace[0] = freeSpace[0] - p.getAmount();
                        }
                        allProducts.add(new ValueDataEntry("Empty", freeSpace[0]));
                        caller.createChart(allProducts);
                    }
                }
            }
            @Override
            public void onFailure(Call<List<Product>> call, Throwable t) {}
        });
    }
}
