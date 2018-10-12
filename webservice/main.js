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
const port = process.env.PORT || 8080;
process.env.HOST = process.env.HOST || 'http://localhost' + ':' + port;

// routing
const defaultRouter = require('./routing-layer/default'),
    ownerRouter = require('./routing-layer/owner'),
    warehouseRouter = require('./routing-layer/warehouse'),
    productRouter = require('./routing-layer/product'),
    manufacturerRouter = require('./routing-layer/manufacturer');
    
ownerRouter.use('/:id', warehouseRouter);
defaultRouter.use('/auth', require('./routing-layer/auth'));
defaultRouter.use('/user', require('./routing-layer/user'));
defaultRouter.use('/owners', ownerRouter);
defaultRouter.use('/warehouses', warehouseRouter);
defaultRouter.use('/products', productRouter);
defaultRouter.use('/manufacturers', manufacturerRouter);
app.use('/', defaultRouter);

// start
app.listen(port);
console.log('Server started on port ' + port);