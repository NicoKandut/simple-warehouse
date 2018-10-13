// packages
const express = require('express'),
    cors = require('cors'),
    bodyParser = require('body-parser'),
    logger = require('./routing-layer/middleware/logger');
    app = express();

// middleware
app.use(cors());
app.use(bodyParser.json());
app.use(logger);

// environment
const port = process.env.PORT || 8080; //TODO: modules with .mjs
process.env.HOST = process.env.HOST || 'http://localhost' + ':' + port;

// routing
const defaultRouter = require('./routing-layer/default');
defaultRouter.use('/auth', require('./routing-layer/auth'));
defaultRouter.use('/user', require('./routing-layer/user'));
defaultRouter.use('/catalog', require('./routing-layer/catalog'));
app.use('/', defaultRouter);

// start
app.listen(port);
console.log('Server started on port ' + port);