// packages
const express = require('express');
const oracleConnection = require('../data-layer/oracleDataAccess');
const classParser = require('../data-layer/classParser');
const classes = require('../data-layer/classes');
const router = express.Router();

// add routes
router.route('/')
    .get((req, res) => {
        let query = 'SELECT * from SW_Product',
            param = [];

        oracleConnection.execute(query, param,
            (result) => res.status(200).json(classParser(result.rows, classes.ProductBase)),
            (err) => res.status(404).json({
                message: err.message,
                details: err
            })
        );
    }).post((req, res) => {
        let query = 'INSERT INTO SW_Product VALUES (seq_product.NEXTVAL, :name, :descripion, :price, :space, : id_manufacturer)',
            param = [req.body.name, req.body.description, req.body.prace, req.body.price, req.body.space, req.body.id_manufaturer, req.body.manufacturer.id];

        oracleConnection.execute(query, param,
            (result) => res.status(201).json(result),
            (err) => res.status(500).json({
                message: err.message,
                details: err
            }));
    });

router.route('/:id')
    .get((req, res) => {
        let query = 'SELECT * from SW_Product WHERE id = :id',
            param = [req.params.id];

        oracleConnection.execute(query, param,
            (result) => {
                let product = classParser(result.rows, classes.ProductBase)[0];
                res.status(200).json(product);                    
            },
            (err) => res.status(404).json({
                message: err.message,
                details: err
            })
        );
    });

module.exports = router;