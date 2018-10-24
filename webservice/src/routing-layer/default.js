// packages
const express = require('express'),
    router = express.Router(),
    host = process.env.HOST;

// add routes
router.get('/', (req, res) => {
    res.json({
        title: 'The Werhaus API',
        documentation: 'https://drive.google.com/open?id=1Q7M9immTpeOPTIU5YpzBSn9f_k-VgX0Ii9DalNl0xmg',
        auth: host + '/auth',
        user: host + '/user',
        catalog: host + '/catalog'
    });
});

module.exports = router;