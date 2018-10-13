// packages //TODO: integrate into user.js and remove
const express = require('express');
const oracleConnection = require('../data-layer/oracleDataAccess');
const classParser = require('../data-layer/classParser');
const classes = require('../data-layer/classes');
const router = express.Router();

// add routes
router.route('/')
    .get((req, res) => {
        let query = 'SELECT * from SW_Warehouse',
            param = [];

        oracleConnection.execute(query, param,
            (result) => res.status(200).json(classParser(result.rows, classes.Warehouse)),
            (err) => res.status(404).json({
                message: err.message,
                details: err
            })
        );
    }).post((req, res) => {
        let query = 'INSERT INTO SW_Warehouse VALUES (seq_warehouse.NEXTVAL, :name, :descripion, :capacity, :id_owner)',
            param = [req.body.name, req.body.description, req.body.capacity, req.body.id_owner || req.body.owner.id];

        oracleConnection.execute(query, param,
            (result) => res.status(201).json(result),
            (err) => res.status(500).json({
                message: err.message,
                details: err
            }));
    });

router.route('/:id')
    .get((req, res) => {
        let query = 'SELECT * from SW_Warehouse WHERE id = :id',
            param = [req.params.id],
            innerQuery = 'SELECT id_product, name, description, price, space, amount from SW_Stored_In INNER JOIN SW_Product ON id_product = SW_Product.id WHERE id_warehouse = :id';

        oracleConnection.execute(query, param,
            (result) => {
                let warehouse = classParser(result.rows, classes.FatWarehouse)[0];

                if (!warehouse)
                    res.sendStatus(404);
                else
                    oracleConnection.execute(innerQuery, param,
                        (result) => {
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