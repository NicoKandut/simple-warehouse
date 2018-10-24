// packages
const app = require('./src/app');

// environment
const port = process.env.PORT || 8080;
process.env.HOST = process.env.HOST || 'http://localhost:' + port;

// start
app.listen(port);
console.log('Server started on port ' + port);