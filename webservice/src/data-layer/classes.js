class Owner {
    constructor(id, name, password) {
        this.id = id;
        this.name = name;
        this.password = password;
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

class Order {
    constructor(id, name, description, price, space, amount, timestamp) {
        this.product = new ProductBase(id, name, description, price, space);
        this.amount = amount;
        this.timestamp = timestamp;
    }
}

class Manufacturer {
    constructor(id, name, description) {
        this.id = id;
        this.name = name;
        this.description = description;
    }
}

module.exports = {
    Owner: Owner,
    Warehouse: Warehouse,
    ProductBase: ProductBase,
    Product: Product,
    Order: Order,
    Manufacturer: Manufacturer
};