package com.okb.warehouse.businesslogic.data;

import java.io.Serializable;
import java.text.SimpleDateFormat;
import java.time.LocalDateTime;
import java.util.Date;
import java.util.List;

public class Order implements Serializable {
    private int id;
    private Date timestamp;
    private List<Product> products;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public Date getTimestamp() {
        return timestamp;
    }

    public String getDate(){
        SimpleDateFormat sdf = new SimpleDateFormat("dd.MM.yy hh:mm");
        return sdf.format(timestamp);
    }

    public void setTimestamp(Date timestamp) {
        this.timestamp = timestamp;
    }

    public List<Product> getProducts() {
        return products;
    }

    public void setProducts(List<Product> products) {
        this.products = products;
    }

    public Order(int id, Date timestamp, List<Product> products) {
        this.id = id;
        this.timestamp = timestamp;
        this.products = products;
    }

    @Override
    public String toString() {
        return "Order{" +
                "id=" + id +
                ", timestamp=" + timestamp +
                ", products=" + products +
                '}';
    }

    public int getAmount(){
        int amount = 0;
        for (Product p :products){
            amount = amount + p.getAmount();
        }
        return amount;
    }
}
