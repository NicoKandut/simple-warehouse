// packages
const express = require('express');
const app = express();
const cors = require('cors');
const bodyParser = require('body-parser');

// middleware
app.use(cors());
app.use(bodyParser.json());
app.use(express.static('Frontend'));

// environment
var port = process.env.PORT || 8080;
process.env.HOST = process.env.HOST || 'http://localhost' +  ':' + port;

// routing
const defaultRouter = require('./routing/default');
app.use('/', defaultRouter);

// start
app.listen(port);
console.log('Server started on port ' + port);