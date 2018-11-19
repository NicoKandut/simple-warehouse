package com.okb.warehouse.businesslogic.data;

public class Product {
    private int id;
    private String name;
    private String description;
    private double price;
    private int space;
    private double amount;

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

    public double getAmount() {
        return amount;
    }

    public void setAmount(double amount) {
        this.amount = amount;
    }

    public Product(int id, String name, String description, double price, int space, double amount) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.price = price;
        this.space = space;
        this.amount = amount;
    }
}
