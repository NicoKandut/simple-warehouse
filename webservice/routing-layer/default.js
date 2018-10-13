// packages
const express = require('express'),
    router = express.Router(),
    host = process.env.HOST;

// add routes
router.get('/', function (req, res) { //TODO: adapt to new route-layout
    res.json({
        message: 'Welcome to the Werhaus API.',
        owners: host + '/owners',
        warehouses: host + '/warehouses',
        products: host + '/products',
        manufacturers: host + '/manufacturers'
    });
});

module.exports = router;