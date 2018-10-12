// packages
const express = require('express'),
    oracleConnection = require('../data-layer/oracleDataAccess'),
    token = require('./middleware/token'),
    classParser = require('../data-layer/classParser'),
    classes = require('../data-layer/classes'),
    router = express.Router();

// add routes
router.post('/login', (req, res) => {
    let query = 'SELECT * FROM SW_Owner WHERE name = :name AND password = :password',
        param = [req.body.name, req.body.password];

    oracleConnection.execute(query, param,
        (result) => {
            // get user
            let user = classParser(result.rows, classes.Owner)[0];

            // generate & send token
            res.status(200).json({
                token: token.create(user)
            });
        },
        (err) => res.status(403).json({
            error: "Authentification Error",
            message: "Wrong combination of username and password"
        }));
});

router.get('/logout', (req, res) => {
    token.remove(req.headers.token);
    res.sendStatus(200);
});

router.post('/register', (req, res) => {
    let query = 'INSERT INTO SW_Owner VALUES (seq_owner.NEXTVAL, :name, :password)',
        param = [req.body.name, req.body.password];

    oracleConnection.execute(query, param,
        (result) => res.sendStatus(201),
        (err) => res.status(500).json({
            message: err.message,
            details: err
        }));
});

router.delete('/delete', (req, res) => {
    let query = 'DELETE FROM SW_Owner WHERE id = :id',
        param = [token.get(req.headers.token)];

    oracleConnection.execute(query, param,
        (result) => res.sendStatus(200),
        (err) => res.status(500).json({
            message: err.message,
            details: err
        }));
});
module.exports = router;