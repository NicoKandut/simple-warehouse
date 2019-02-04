// packages
const express = require('express'),
    oracleConnection = require('../data-layer/oracleDataAccess'),
    token = require('./middleware/token'),
    error = require('./misc/error'),
    classParser = require('../data-layer/classParser'),
    classes = require('../data-layer/classes'),
    router = express.Router();

// limit access to logout and delete to logged in users
router.use('/logout|/delete', token.access);

// add routes
router.post('/login', (req, res) => {
    let query = 'SELECT * FROM SW_Owner WHERE name = :name AND password = :password',
        param = [req.body.name, req.body.password];

    oracleConnection.execute(query, param)
        .then((result) => {
            let user = classParser(result.rows, classes.Owner)[0];
            if (user)
                res.status(200).json({
                    token: token.create(user)
                });
            else
                error.respondWith(res, 403.1);
        })
        .catch((err) => error.respondWith(res, 403.1));
});

router.get('/logout', (req, res) => {
    if (token.remove(req.headers.token))
        res.sendStatus(200);
    else
        res.sendStatus(404);
});

router.post('/register', (req, res) => {
    let query = 'INSERT INTO SW_Owner VALUES (seq_owner.NEXTVAL, :name, :password)',
        param = [req.body.name, req.body.password];

    oracleConnection.execute(query, param)
        .then((result) => res.sendStatus(201))
        .catch((err) => res.status(500).json({
            message: err.message,
            details: err
        }));
});

router.delete('/delete', (req, res) => {
    let query = 'DELETE FROM SW_Owner WHERE id = :id',
        param = [token.get(req.headers.token)];

    oracleConnection.execute(query, param)
        .then((result) => {
            if (result && result.rowsAffected === 1)
                res.sendStatus(200);
            else
                res.sendStatus(404);
        })
        .catch((err) => res.status(500).json({
            message: err.message,
            details: err
        }));
});
module.exports = router;