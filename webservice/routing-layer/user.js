// packages
const express = require('express'),
    oracleConnection = require('../data-layer/oracleDataAccess'),
    classParser = require('../data-layer/classParser'),
    classes = require('../data-layer/classes'),
    tokenAccess = require('./middleware/token').access,
    router = express.Router();

// authorization
router.use(tokenAccess);

// add routes
router.route('/')
    .get((req, res) => {
        let query = 'SELECT * from SW_Owner WHERE id = :id',
            warehouseQuery = 'SELECT * from SW_Warehouse WHERE id_owner = :id',
            param = [req.uid],
            owner;

        oracleConnection.execute(query, param)
            .then(result => {
                owner = classParser(result.rows, classes.Owner)[0];
                return oracleConnection.execute(warehouseQuery, param);
            })
            .then(result => {
                owner.warehouses = classParser(result.rows, classes.Warehouse);
                res.status(200).json(owner);
            })
            .catch(err => res.status(404).json({
                message: err.message,
                details: err
            }));
    })
    .put((req, res) => {
        let query = 'UPDATE SW_Owner SET name = :name, password = :password WHERE id = :id',
            param = [req.body.name, req.body.password, req.uid];

        oracleConnection.execute(query, param)
            .then(result => res.sendStatus(200))
            .catch(err => res.status(404).json({
                message: err.message,
                details: err
            }));
    });

router.route('/warehouses')
    .get((req, res) => {
        let query = 'SELECT * from SW_Warehouse WHERE id_owner = :id',
            param = [req.uid];

        oracleConnection.execute(query, param)
            .then(result => res.status(200).json(classParser(result.rows, classes.Warehouse)))
            .catch(err => res.status(404).json({
                message: err.message,
                details: err
            }));
    })
    .post((req, res) => {
        let query = 'INSERT INTO SW_Warehouse VALUES (seq_warehouse.NEXTVAL, :name, :descripion, :capacity, :id_owner)',
            param = [req.body.name, req.body.description, req.body.capacity, req.uid];

        oracleConnection.execute(query, param)
            .then(result => res.sendStatus(201))
            .catch(err => res.status(500).json({
                message: err.message,
                details: err
            }));
    });

router.route('/warehouses/:id')
    .get((req, res) => {
        let query = 'SELECT * from SW_Warehouse WHERE id = :id AND id_owner = :id_owner',
            productQuery = 'SELECT id_product, name, description, price, space, amount from SW_Stored_In INNER JOIN SW_Product ON id_product = SW_Product.id WHERE id_warehouse = :id',
            orderQuery = 'SELECT id_product, name, description, price, space, amount, timestamp from SW_Order INNER JOIN SW_Product ON id_product = SW_Product.id WHERE id_warehouse = :id',
            param = [req.params.id, req.uid],
            warehouse;

        oracleConnection.execute(query, param) //TODO: single db call
            .then(result => {
                warehouse = classParser(result.rows, classes.Warehouse)[0];

                if (!warehouse)
                    res.sendStatus(404);
                else
                    return oracleConnection.execute(productQuery, [param[0]]);

            })
            .then(result => {
                warehouse.products = classParser(result.rows, classes.Product);
                return oracleConnection.execute(orderQuery, [param[0]]);
            })
            .then(result => {
                warehouse.orders = classParser(result.rows, classes.Product);
                res.status(200).json(warehouse);
            })
            .catch(err => res.status(404).json({
                message: err.message,
                details: err
            }));
    })
    .delete((req, res) => {
        let query = 'DELETE FROM SW_Warehouse WHERE id = :id AND id_owner = :id_owner',
            param = [req.params.id, req.uid];

        oracleConnection.execute(query, param)
            .then(result => res.sendStatus(200))
            .catch(err => res.status(500).json({
                message: err.message,
                details: err
            }));
    });

router.route('/warehouses/:id/products')
    .get((req, res) => {
        let query = 'SELECT  id_product, SW_Product.name, SW_Product.description, price, space, amount from SW_Product INNER JOIN SW_Stored_In ON SW_Product.id = id_product INNER JOIN SW_Warehouse ON SW_Warehouse.id = id_warehouse WHERE id_warehouse = :id AND id_owner = :id_owner',
            param = [req.params.id, req.uid];

        oracleConnection.execute(query, param)
            .then(result => res.status(200).json(classParser(result.rows, classes.Product)))
            .catch(err => res.status(404).json({
                message: err.message,
                details: err
            }));
    });

router.route('/warehouses/:id/orders')
    .get((req, res) => {
        let query = 'SELECT  id_product, SW_Product.name, SW_Product.description, price, space, amount, timestamp from SW_Product INNER JOIN SW_Order ON SW_Product.id = id_product INNER JOIN SW_Warehouse ON SW_Warehouse.id = id_warehouse WHERE id_warehouse = :id AND id_owner = :id_owner',
            param = [req.params.id, req.uid];

        oracleConnection.execute(query, param)
            .then(result => res.status(200).json(classParser(result.rows, classes.Order)))
            .catch(err => res.status(404).json({
                message: err.message,
                details: err
            }));
    })
    .post((req, res) => {
        let query = 'INSERT INTO SW_Order VALUES (:id_product, :id_warehouse, :amount, CURRENT_TIMESTAMP)',
            param = [req.body.id_product, req.params.id, req.body.amount];

        oracleConnection.execute(query, param)
            .then(result => res.sendStatus(201))
            .catch(err => res.status(404).json({
                message: err.message,
                details: err
            }));
    });

module.exports = router;