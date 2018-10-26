// packages
const express = require('express'),
    router = express.Router(),
    host = process.env.HOST;

// add routes
router.get('/', (req, res) => {
    res.json({
        title: 'The Werhaus API',
        version: '2.1',
        documentation: 'https://documenter.getpostman.com/view/3279137/RzZ1qNC2',
        auth: host + '/auth',
        user: host + '/user',
        catalog: host + '/catalog'
    });
});

module.exports = router;