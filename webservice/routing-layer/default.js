// packages
const express = require('express');
const router = express.Router();
const host = process.env.HOST;

// add routes
router.get('/', function (req, res) {
    res.json({
        message: "Welcome to the Werhaus API.",
        owners: host + "/owners",
        warehouses: host + "/warehouses"
    });
});

module.exports = router;