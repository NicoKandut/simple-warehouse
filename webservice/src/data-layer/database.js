const oracledb = require('oracledb'),
    SimpleOracleDB = require('simple-oracledb'),
    DB_STRING = process.env.DB_STRING || '212.152.179.117/ora11g',
    DB_USER = process.env.DB_USER || 'd5a13',
    DB_PASS = process.env.DB_PASS || 'd5a',
    options = {
        autoCommit: true
    };

// modify the original oracledb library
SimpleOracleDB.extend(oracledb);

function connect() {
    return oracledb.getConnection({
        user: DB_USER,
        password: DB_PASS,
        connectString: DB_STRING
    });
}

function execute(query, param = []) {
    return connect()
        .then(connection => new Promise((resolve, reject) => {
            return connection.execute(query, param, options)
                .then(result => resolve(result))
                .catch(err => reject(err))
                .then(() => connection.close());
        }))
        .catch(err => {
            err.message = "Database Error: " + err.message;
            throw err;
        });
}

function batchInsert(query, param = []) {
    return connect()
        .then(connection => new Promise((resolve, reject) => {
            return connection.batchInsert(query, param, options)
                .then(result => resolve(result))
                .catch(err => reject(err))
                .then(() => connection.close());
        }))
        .catch(err => {
            err.message = "Database Error: " + err.message;
            throw err;
        });
}

module.exports = {
    execute: execute,
    batchInsert: batchInsert
};