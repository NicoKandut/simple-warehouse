// packages
const express = require('express'),
    database = require('../data-layer/database'),
    classParser = require('../data-layer/classParser'),
    classes = require('../data-layer/classes'),
    errorResponse = require('./misc/error'),
    router = express.Router(),
    host = process.env.HOST;

// add routes
router.get('/', (req, res) => res.json({
    products: host + '/catalog/products',
    manufacturers: host + '/catalog/manufacturers'
}));

router.get('/products', (req, res) => {
    database.execute('SELECT * from SW_Product')
        .then(result => {
            res.json(classParser(result.rows, classes.ProductBase));
        })
        .catch(err => {
            errorResponse(res, 500, err);
        });
});

router.get('/manufacturers', (req, res) => {
    database.execute('SELECT * from SW_Manufacturer')
        .then(result => res.json(classParser(result.rows, classes.Manufacturer)))
        .catch(err => errorResponse(res, 500, err));
});

router.get('/manufacturers/:id', (req, res) => {
    let query = 'SELECT * from SW_Manufacturer WHERE id = :id',
        param = [req.params.id],
        innerQuery = 'SELECT * from SW_Product WHERE id_manufacturer = :id',
        manufacturer;

    database.execute(query, param)
        .then(result => {
            manufacturer = classParser(result.rows, classes.Manufacturer)[0];

            if (!manufacturer)
                errorResponse(res, 404.4);
            else
                return database.execute(innerQuery, param);

        })
        .then(result => {
            manufacturer.products = classParser(result.rows, classes.ProductBase);
            res.json(manufacturer);
        })
        .catch(err => errorResponse(res, 500, err));
});

module.exports = router;