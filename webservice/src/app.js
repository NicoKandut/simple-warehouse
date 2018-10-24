const express = require('express'),
    cors = require('cors'),
    bodyParser = require('body-parser'),
    app = express();

// middleware
app.use(cors());
app.use(bodyParser.json());

// routing
const api = require('./routing-layer/default');
api.use('/auth', require('./routing-layer/auth'));
api.use('/user', require('./routing-layer/user'));
api.use('/catalog', require('./routing-layer/catalog'));
app.use('/', api);

module.exports = app;