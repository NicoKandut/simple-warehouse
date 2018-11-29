// packages
const express = require('express'),
    connection = require('../data-layer/connection'),
    token = require('./middleware/token'),
    errorResponse = require('./misc/error'),
    classParser = require('../data-layer/classParser'),
    classes = require('../data-layer/classes'),
    router = express.Router();

// limit access to logout and delete to logged in users
router.use('/logout|/delete', token.access);

// add routes
router.post('/login', (req, res) => {
    if (!req.body.name && !req.body.password)
        return errorResponse(res, 400.2);

    connection.execute('SELECT * FROM SW_Owner WHERE name = :name AND password = :password', [req.body.name, req.body.password])
        .then(result => {
            let user = classParser(result.rows, classes.Owner)[0];
            if (user)
                res.status(200).json({
                    token: token.create(user)
                });
            else
                errorResponse(res, 403.1);
        })
        .catch(err => {
            errorResponse(res, 403.1);
        });
});

router.get('/logout', (req, res) => {
    if (token.remove(req.uid))
        res.sendStatus(204);
    else
        errorResponse(res, 403.2);
});

router.post('/register', (req, res) => {
    if (!req.body.name && !req.body.password)
        return errorResponse(res, 400.2);

    connection.execute('INSERT INTO SW_Owner VALUES (seq_owner.NEXTVAL, :name, :password)', [req.body.name, req.body.password])
        .then(result => {
            res.sendStatus(201);
        })
        .catch(err => {
            errorResponse(res, 500, err);
        });
});

router.delete('/delete', (req, res) => {
    connection.execute('DELETE FROM SW_Owner WHERE id = :id', [req.uid])
        .then(result => {
            if (result && result.rowsAffected === 1) {
                token.remove(req.uid);
                res.sendStatus(204);
            } else
                errorResponse(res, 404.2);
        })
        .catch(err => {
            errorResponse(res, 500, err);
        });
});
module.exports = router;