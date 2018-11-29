const oracledb = require('oracledb'),
    SimpleOracleDB = require('simple-oracledb'),
    DB_STRING = process.env.DB_STRING,
    DB_USER = process.env.DB_USER,
    DB_PASS = process.env.DB_PASS;

if (DB_STRING === null || DB_USER === null || DB_PASS === null)
    throw new Error("Invalid Environment");

SimpleOracleDB.extend(oracledb);

function connect() {
    return new Promise((resolve, reject) => {
        let connection;

        oracledb.getConnection({
                user: DB_USER,
                password: DB_PASS,
                connectString: DB_STRING
            })
            .then(result => {
                connection = result;
                resolve(connection);
            })
            .catch(err => {
                reject(err);
            }); 
    });
}

function close(connection) {
    connection.commit();
    connection.close();
}

function execute(connection, query, param = []) {
    return new Promise((resolve, reject) => {
        connection.execute(query, param)
            .then(result => {
                resolve(result);
            })
            .catch(err => {
                connection.rollback();
                connection.close();
                reject(err);
            });
    });
}

function batchInsert(connection, query, param = []) {
    return new Promise((resolve, reject) => {
        connection.batchInsert(query, param, options)
            .then(result => {
                resolve(result);
            })
            .catch(err => {
                connection.rollback();
                connection.close();
                reject(err);
            });
    });
}

module.exports = {
    connect: connect,
    close: close,
    execute: execute,
    batchInsert: batchInsert
};