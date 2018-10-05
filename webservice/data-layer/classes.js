class Owner {
    constructor(id, name, password) {
        this.id = id;
        this.name = name;
        this.password = password;
    }
}

class FatOwner extends Owner {
    constructor(id, name, password, warehouses) {
        super(id, name, password);

        this.warehouses = warehouses || [];
    }
}

class Warehouse {
    constructor(id, name, description, capacity) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.capacity = capacity;
    }
}

class FatWarehouse extends Warehouse {
    constructor(id, name, description, capacity, products) {
        super(id, name, description, capacity);

        this.products = products || [];
    }
}

class ProductBase {
    constructor(id, name, description, price, space) {
        this.id = id;
        this.name = name;
        this.description = description;
        this.price = price;
        this.space = space;
    }
}

class Product extends ProductBase {
    constructor(id, name, description, price, space, amount) {
        super(id, name, description, price, space);

        this.amount = amount;
    }
}

module.exports = {
    Owner: Owner,
    FatOwner: FatOwner,
    Warehouse: Warehouse,
    FatWarehouse: FatWarehouse,
    ProductBase: ProductBase,
    Product: Product
};