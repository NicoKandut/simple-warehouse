package com.okb.warehouse.businesslogic.data;

import java.io.Serializable;
import java.text.DecimalFormat;

public class Product implements Serializable {
    private int id;
    private String name;
    private String description;
    private double price;
    private int space;
    private int amount;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }

    public String getStringPrice(){
        return String.valueOf(price);
    }
    public int getSpace() {
        return space;
    }

    public void setSpace(int space) {
        this.space = space;
    }

    public int getAmount() {
        return amount;
    }

    public void setAmount(int amount) {
        this.amount = amount;
    }

    public Product(int id, String name, String description, double price, int space, int amount) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.price = price;
        this.space = space;
        this.amount = amount;
    }

    @Override
    public String toString() {
        DecimalFormat df = new DecimalFormat("#0.00");
        return  name + " - " +  df.format(price) + "€";
    }
}
