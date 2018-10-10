// packages
const express = require('express'),
    oracleConnection = require('../data-layer/oracleDataAccess'),
    classParser = require('../data-layer/classParser'),
    classes = require('../data-layer/classes'),
    router = express.Router();

// add routes
router.route('/')
    .get((req, res) => {
        let query = 'SELECT * from SW_Manufacturer',
            param = [];

        oracleConnection.execute(query, param,
            (result) => res.status(200).json(classParser(result.rows, classes.Manufacturer)),
            (err) => res.status(404).json({
                message: err.message,
                details: err
            })
        );
    })
    .post((req, res) => {
        let query = 'INSERT INTO SW_Manufacturer VALUES (seq_owner.NEXTVAL, :name, :descripion)',
            param = [req.body.name, req.body.description];

        oracleConnection.execute(query, param,
            (result) => res.status(201).json(result),
            (err) => res.status(500).json({
                message: err.message,
                details: err
            }));
    });

router.route('/:id')
    .get((req, res) => {
        let query = 'SELECT * from SW_Manufacturer WHERE id = :id',
            param = [req.params.id],
            innerQuery = 'SELECT * from SW_Product WHERE id_manufacturer = :id';

        oracleConnection.execute(query, param,
            (result) => {
                let manufacturer = classParser(result.rows, classes.FatManufacturer)[0];

                if (!manufacturer)
                    res.sendStatus(404);
                else
                    oracleConnection.execute(innerQuery, param,
                        (result) => {
                            manufacturer.products = classParser(result.rows, classes.ProductBase);

                            res.status(200).json(manufacturer);
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