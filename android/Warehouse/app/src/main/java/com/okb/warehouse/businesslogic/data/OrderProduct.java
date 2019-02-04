package com.okb.warehouse.businesslogic.data;

import java.text.DecimalFormat;

public class OrderProduct {
    private int id;
    private double amount;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public double getAmount() {
        return amount;
    }

    public void setAmount(double amount) {
        this.amount = amount;
    }

    public OrderProduct(int id, double amount) {
        this.id = id;
        this.amount = amount;
    }

    public OrderProduct(Product p){
        this.id = p.getId();
        this.amount = p.getAmount();
    }

}
