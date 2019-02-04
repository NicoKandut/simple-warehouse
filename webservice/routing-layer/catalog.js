// packages
const express = require('express'),
    oracleConnection = require('../data-layer/oracleDataAccess'),
    classParser = require('../data-layer/classParser'),
    classes = require('../data-layer/classes'),
    router = express.Router()
host = process.env.HOST;

// add routes
router.get('/', (req, res) => res.json({
    products: host + '/catalog/products',
    manufacturers: host + '/catalog/manufacturers'
}));

router.get('/products', (req, res) => {
    let query = 'SELECT * from SW_Product';

    oracleConnection.execute(query)
        .then((result) => res.json(classParser(result.rows, classes.ProductBase)))
        .catch((err) => res.status(404).json({
            message: err.message,
            details: err
        }));
});

router.get('/manufacturers', (req, res) => {
    oracleConnection.execute('SELECT * from SW_Manufacturer')
        .then((result) => res.json(classParser(result.rows, classes.Manufacturer)))
        .catch((err) => res.status(404).json({
            message: err.message,
            details: err
        }));
});

router.get('/manufacturers/:id', (req, res) => {
    let query = 'SELECT * from SW_Manufacturer WHERE id = :id',
        param = [req.params.id],
        innerQuery = 'SELECT * from SW_Product WHERE id_manufacturer = :id',
        manufacturer;

    oracleConnection.execute(query, param)
        .then((result) => {
            manufacturer = classParser(result.rows, classes.Manufacturer)[0];

            if (!manufacturer)
                res.sendStatus(404);
            else
                return oracleConnection.execute(innerQuery, param);
                   
        })
        .then((result) => {
            manufacturer.products = classParser(result.rows, classes.ProductBase);
            res.json(manufacturer);
        })
        .catch((err) => res.status(404).json({
            message: err.message,
            details: err
        }));
});

module.exports = router;