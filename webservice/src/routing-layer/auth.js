// packages
const express = require('express'),
    database = require('../data-layer/database'),
    token = require('./middleware/token'),
    errorResponse = require('./misc/error'),
    classParser = require('../data-layer/classParser'),
    classes = require('../data-layer/classes'),
    router = express.Router();

// limit access to logout and delete to logged in users
router.use('/logout|/delete', token.access);

// add routes
router.post('/login', (req, res) => {
    let query = 'SELECT * FROM SW_Owner WHERE name = :name AND password = :password',
        param = [req.body.name, req.body.password];

    database.execute(query, param)
        .then(result => {
            let user = classParser(result.rows, classes.Owner)[0];
            if (user)
                res.status(200).json({
                    token: token.create(user)
                });
            else
                errorResponse(res, 403.1);
        })
        .catch(err => errorResponse(res, 403.1));
});

router.get('/logout', (req, res) => {
    if (token.remove(req.uid))
        res.sendStatus(204);
    else
        errorResponse(res, 403.2);
});

router.post('/register', (req, res) => {
    let query = 'INSERT INTO SW_Owner VALUES (seq_owner.NEXTVAL, :name, :password)',
        param = [req.body.name, req.body.password];

    if(!req.body.name && !req.body.password)
        return errorResponse(res, 400.2);

    database.execute(query, param)
        .then(result => res.sendStatus(201))
        .catch(err => errorResponse(res, 500, err));
});

router.delete('/delete', (req, res) => {
    let query = 'DELETE FROM SW_Owner WHERE id = :id',
        param = [req.uid];

    database.execute(query, param)
        .then(result => {
            if (result && result.rowsAffected === 1) {
                token.remove(req.uid);
                res.sendStatus(204);
            } else
                errorResponse(res, 404.2);
        })
        .catch(err => errorResponse(res, 500, err));
});
module.exports = router;