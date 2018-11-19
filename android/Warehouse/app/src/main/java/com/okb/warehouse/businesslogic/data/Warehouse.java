package com.okb.warehouse.businesslogic.data;

import java.util.List;

public class Warehouse {
    private int id;
    private String name;
    private String description;
    private int capacity;
    private List<Product> products;

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

    public int getCapacity() {
        return capacity;
    }

    public void setCapacity(int capacity) {
        this.capacity = capacity;
    }

    public String getStringCapacity(){return "" + capacity; }

    public List<Product> getProducts() {
        return products;
    }

    public void setProducts(List<Product> products) {
        this.products = products;
    }

    public Warehouse(int id, String name, String description, int capacity, List<Product> products) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.capacity = capacity;
        this.products = products;
    }
}
