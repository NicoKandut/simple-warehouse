// packages
var express = require('express');
var router = express.Router();
var host = process.env.HOST;

// add routes
router.get('/', function (req, res) {
    res.json({
        message: 'Welcome to the Simple Warehouse API.'
    });
});

module.exports = router;

