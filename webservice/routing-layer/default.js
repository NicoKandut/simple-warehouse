// packages
const express = require('express'),
    router = express.Router(),
    host = process.env.HOST;

// add routes
router.get('/', function (req, res) {
    res.json({
        title: 'The Werhaus API',
        message: 'This is a simple api to access the werhaus database. For more information see https://drive.google.com/open?id=1Q7M9immTpeOPTIU5YpzBSn9f_k-VgX0Ii9DalNl0xmg',
        auth: host + '/auth',
        user: host + '/user',
        catalog: host + '/catalog'
    });
});

module.exports = router;