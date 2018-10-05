const oracledb = require('oracledb');

const DB_STRING = "212.152.179.117/ora11g";
const DB_USER = "d5a07";
const DB_PASS = "d5a";

module.exports = {
    execute: (query, param, onSuccess, onError) => {
        oracledb.getConnection({
            user: DB_USER,
            password: DB_PASS,
            connectString: DB_STRING
        }, (err, connection) => {
            if (err)
                closeAfter(connection, onError, [err]);
            else
                connection.execute(query, param, (err, result) => {
                    if (err)
                        closeAfter(connection, onError, [err]);
                    else
                        closeAfter(connection, onSuccess, [result])
                });
        });
    }
};

function closeAfter(connection, callback, params) {
    callback(...params);
    if (connection)
        connection.close((err) => {
            if (err)
                console.error(err.message);
        });
}