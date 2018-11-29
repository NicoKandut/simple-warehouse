const oracledb = require('oracledb'),
    SimpleOracleDB = require('simple-oracledb'),
    DB_STRING = process.env.DB_STRING,
    DB_USER = process.env.DB_USER,
    DB_PASS = process.env.DB_PASS;

if (DB_STRING === null || DB_USER === null || DB_PASS === null)
    throw new Error("Invalid Environment");

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
        .then(connection => {
            return new Promise((resolve, reject) => {
                return connection.execute(query, param)
                    .then(result => {
                        resolve(result);
                    })
                    .catch(err => {
                        connection.rollback();
                        reject(err);
                    })
                    .then(() => {
                        connection.commit();
                        connection.close();
                    });
            });
        })
        .catch(err => {
            err.message = "Database Error: " + err.message;
            throw err;
        });
}

function batchInsert(query, param = []) {
    return connect()
        .then(connection => new Promise((resolve, reject) => {
            return connection.batchInsert(query, param, options)
                .then(result => {
                    resolve(result);
                })
                .catch(err => {
                    reject(err);
                })
                .then(() => {
                    connection.close();
                });
        }));
}

module.exports = {
    execute: execute,
    batchInsert: batchInsert
};