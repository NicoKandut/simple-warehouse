const oracledb = require('oracledb'),
    SimpleOracleDB = require('simple-oracledb'),
    DB_STRING = process.env.DB_STRING,
    DB_USER = process.env.DB_USER,
    DB_PASS = process.env.DB_PASS;

let connection;

if (DB_STRING === null || DB_USER === null || DB_PASS === null)
    throw new Error("Invalid Environment");

SimpleOracleDB.extend(oracledb);

function connect() {
    if (!connection)
        oracledb.getConnection({
            user: DB_USER,
            password: DB_PASS,
            connectString: DB_STRING
        })
        .then(result => {
            connection = result;
        })
        .catch(err => {
            console.log("Connection Failed: " + err);
        });
}

function execute(query, param = []) {
    return new Promise((resolve, reject) => {
        connection.execute(query, param)
            .then(result => {
                connection.commit();
                resolve(result);
            })
            .catch(err => {
                connection.rollback();
                reject(err);
            });
    });
}

function batchInsert(query, param = []) {
    return new Promise((resolve, reject) => {
        connection.batchInsert(query, param, {
                autoCommit: true
            })
            .then(result => {
                resolve(result);
            })
            .catch(err => {
                connection.rollback();
                reject(err);
            });
    });
}

connect();

process.on("SIGTERM", () => {
    console.log("Closing connection...");
    if (connection)
        connection.close();
});

module.exports = {
    execute: execute,
    batchInsert: batchInsert
};