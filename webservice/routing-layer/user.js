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
            param = [req.uid];

        oracleConnection.execute(query, param,
            (result) => {
                let owner = classParser(result.rows, classes.FatOwner);

                let query = 'SELECT * from SW_Warehouse WHERE id_owner = :id',
                    param = [req.uid];

                oracleConnection.execute(query, param,
                    (result) => {
                        owner.warehouses = classParser(result.rows, classes.Warehouse);
                        res.status(200).json(owner);
                    },
                    (err) => res.status(404).json({
                        message: err.message,
                        details: err
                    }));
            },
            (err) => res.status(404).json({
                message: err.message,
                details: err
            }));
    })
    .put((req, res) => {
        let query = 'UPDATE from SW_Owner SET name = :name, password = :password WHERE id = :id',
            param = [req.body.name, req.body.password, req.uid];

        oracleConnection.execute(query, param,
            (result) => res.sendStatus(200),
            (err) => res.status(404).json({
                message: err.message,
                details: err
            }));
    });

router.route('/warehouses')
    .get((req, res) => {
        let query = 'SELECT * from SW_Warehouse WHERE id_owner = :id',
            param = [req.uid];

        oracleConnection.execute(query, param,
            (result) => res.status(200).json(classParser(result.rows, classes.Warehouse)),
            (err) => res.status(404).json({
                message: err.message,
                details: err
            }));
    })
    .post((req, res) => {
        let query = 'INSERT INTO SW_Warehouse VALUES (seq_warehouse.NEXTVAL, :name, :descripion, :capacity, :id_owner)',
            param = [req.body.name, req.body.description, req.body.capacity, req.uid];

        oracleConnection.execute(query, param,
            (result) => res.sendStatus(201),
            (err) => res.status(500).json({
                message: err.message,
                details: err
            }));
    });

router.route('/warehouses/:id')
    .get((req, res) => {
        let query = 'SELECT * from SW_Warehouse WHERE id = :id AND id_owner = :id_owner',
            param = [req.params.id, req.uid],
            innerQuery = 'SELECT id_product, name, description, price, space, amount from SW_Stored_In INNER JOIN SW_Product ON id_product = SW_Product.id WHERE id_warehouse = :id';

        oracleConnection.execute(query, param,
            (result) => {
                let warehouse = classParser(result.rows, classes.FatWarehouse)[0];

                if (!warehouse)
                    res.sendStatus(404);
                else
                    oracleConnection.execute(innerQuery, [param[0]],
                        (result) =>{
                            warehouse.products = classParser(result.rows, classes.Product);

                            res.status(200).json(warehouse);
                        },
                        (err) => res.status(404).json({
                            message: err.message,
                            details: err
                        })
                    );
            },
            (err) => res.status(404).json({
                message: err.message,
                details: err
            })
        );
    });

module.exports = router;