// packages
const express = require('express'),
    database = require('../data-layer/database'),
    classParser = require('../data-layer/classParser'),
    classes = require('../data-layer/classes'),
    tokenAccess = require('./middleware/token').access,
    errorResponse = require('./misc/error'),
    router = express.Router();

// authorization
router.use(tokenAccess);

// add routes
router.route('/')
    .get((req, res) => {
        let query = 'SELECT * from SW_Owner WHERE id = :id',
            warehouseQuery = 'SELECT * from SW_Warehouse WHERE id_owner = :id',
            productQuery = 'SELECT id, name, description, price, space, amount, id_warehouse FROM SW_Stored_In left join sw_product on id = id_product',
            param = [req.uid],
            owner;

        database.execute(query, param)
            .then(result => {
                owner = classParser(result.rows, classes.Owner)[0];
                return database.execute(warehouseQuery, param);
            })
            .then(result => {
                owner.warehouses = classParser(result.rows, classes.Warehouse);
                return database.execute(productQuery);
            })
            .then(result => {
                owner.warehouses.forEach(wh => {
                    wh.products = classParser(result.rows.filter(row => row[6] === wh.id), classes.Product);
                });
                res.status(200).json(owner);
            })
            .catch(err => errorResponse(res, 500, err));
    })
    .put((req, res) => {
        let query = 'UPDATE SW_Owner SET {{set}} WHERE id = :id',
            param = [req.body.name, req.body.password, req.uid];

        if (!req.body.name && !req.body.password)
            errorResponse(res, 409.1)
        else {
            let sets = [];
            if (req.body.name)
                sets.push('name = :name');
            if (req.body.password)
                sets.push('password = :password');

            query.replace('{{set}}', sets.join(', '));
        }

        database.execute(query, param)
            .then(result => res.sendStatus(204))
            .catch(err => errorResponse(res, 500, err));
    });

router.route('/warehouses')
    .get((req, res) => {
        let query = 'SELECT * from SW_Warehouse WHERE id_owner = :id',
            productQuery = 'SELECT id, name, description, price, space, amount, id_warehouse FROM SW_Stored_In left join sw_product on id = id_product',
            param = [req.uid],
            warehouses;

        database.execute(query, param)
            .then(result => {
                warehouses = classParser(result.rows, classes.Warehouse);
                return database.execute(productQuery);
            })
            .then(result => {
                warehouses.forEach(wh => {
                    wh.products = classParser(result.rows.filter(row => row[6] === wh.id), classes.Product);
                });
                res.status(200).json(warehouses)
            })
            .catch(err => errorResponse(res, 500, err));
    })
    .post((req, res) => {
        let query = 'INSERT INTO SW_Warehouse VALUES (seq_warehouse.NEXTVAL, :name, :descripion, :capacity, :id_owner)',
            param = [req.body.name, req.body.description, req.body.capacity, req.uid];

        database.execute(query, param)
            .then(result => res.sendStatus(201))
            .catch(err => errorResponse(res, 500, err));
    });

router.route('/warehouses/:id')
    .get((req, res) => {
        let query = 'SELECT * from SW_Warehouse WHERE id = :id AND id_owner = :id_owner',
            productQuery = 'SELECT id_product, name, description, price, space, amount from SW_Stored_In INNER JOIN SW_Product ON id_product = SW_Product.id WHERE id_warehouse = :id',
            orderQuery = 'SELECT id, timestamp from SW_Order WHERE id_warehouse = :id_warehouse',
            partQuery = 'SELECT SW_Product.id, name, description, price, space, amount, id_warehouse FROM SW_Orderpart LEFT JOIN SW_Product ON id_product = SW_Product.id LEFT JOIN SW_Order ON id_order = SW_Order.id WHERE id_warehouse = :id_warehouse',
            param = [req.params.id, req.uid],
            warehouse;

        database.execute(query, param)
            .then(result => {
                warehouse = classParser(result.rows, classes.Warehouse)[0];

                if (!warehouse)
                    errorResponse(res, 404.3);
                else
                    return database.execute(productQuery, [req.params.id]);

            })
            .then(result => {
                warehouse.products = classParser(result.rows, classes.Product);
                return database.execute(orderQuery, [req.params.id]);
            })
            .then(result => {
                warehouse.orders = classParser(result.rows, classes.Product);
                return database.execute(partQuery, [req.params.id]);
            })
            .then(result => {
                warehouse.orders.forEach(o => {
                    o.products = classParser(result.rows.filter(row => row[6] === o.id), classes.Product);
                });
                res.status(200).json(warehouse);
            })
            .catch(err => errorResponse(res, 500, err));
    })
    .delete((req, res) => {
        let query = 'DELETE FROM SW_Warehouse WHERE id = :id AND id_owner = :id_owner',
            param = [req.params.id, req.uid];

        database.execute(query, param)
            .then(result => {
                if (result && result.rowsAffected === 1)
                    res.sendStatus(204);
                else
                    errorResponse(res, 404.3);
            })
            .catch(err => errorResponse(res, 500, err));
    });

router.route('/warehouses/:id/products')
    .get((req, res) => {
        let query = 'SELECT  id_product, SW_Product.name, SW_Product.description, price, space, amount from SW_Product INNER JOIN SW_Stored_In ON SW_Product.id = id_product INNER JOIN SW_Warehouse ON SW_Warehouse.id = id_warehouse WHERE id_warehouse = :id AND id_owner = :id_owner',
            param = [req.params.id, req.uid];

        database.execute(query, param)
            .then(result => res.status(200).json(classParser(result.rows, classes.Product)))
            .catch(err => errorResponse(res, 500, err));
    });

router.route('/warehouses/:id/orders')
    .get((req, res) => {
        let query = 'SELECT id_product, SW_Product.name, SW_Product.description, price, space, amount, timestamp from SW_Product INNER JOIN SW_Order ON SW_Product.id = id_product INNER JOIN SW_Warehouse ON SW_Warehouse.id = id_warehouse WHERE id_warehouse = :id AND id_owner = :id_owner',
            partQuery = 'SELECT SW_Product.id, name, description, price, space, amount, id_warehouse FROM SW_Orderpart LEFT JOIN SW_Product ON id_product = SW_Product.id LEFT JOIN SW_Order ON id_order = SW_Order.id WHERE id_warehouse = :id_warehouse',
            param = [req.params.id, req.uid],
            orders;

        database.execute(query, param)
            .then(result => {
                orders = classParser(result.rows, classes.Order);
                return database.execute(partQuery, [req.params.id]);
            })
            .then(result => {
                orders.forEach(o => {
                    o.products = classParser(result.rows.filter(row => row[6] === o.id), classes.Product);
                });
                res.status(200).json(order);
            })
            .catch(err => errorResponse(res, 500, err));
    })
    .post((req, res) => {
        let seqQuery = 'SELECT seq_order.NEXTVAL FROM DUAL',
            orderQuery = 'INSERT INTO SW_Order VALUES (:id, :id_warehouse, CURRENT_TIMESTAMP)',
            partQuery = 'INSERT INTO SW_Orderpart VALUES (:id_order, :id_product, :amount)'
        param;

        database.execute(seqQuery)
            .then(result => {
                param = req.body.foreach(v => v.id_order = result.rows[0][0]);
                return database.execute(orderQuery, [orderId, req.params.id]);
            })
            .then(result => {
                if (result.rowsAffected !== 1)
                    errorResponse(res, 500, "Error when creating Order.");
                else
                    return database.batchInsert(partQuery, param);
            })
            .then(result => res.sendStatus(204))
            .catch(err => {
                errorResponse(res, 500, err);
            });
    });

module.exports = router;