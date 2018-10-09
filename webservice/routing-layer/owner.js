// packages
const express = require('express'),
    oracleConnection = require('../data-layer/oracleDataAccess'),
    classParser = require('../data-layer/classParser'),
    classes = require('../data-layer/classes'),
    router = express.Router();

// add routes
router.route('/')
    .get((req, res) => {
        let query = 'SELECT * from SW_Owner',
            param = [];

        oracleConnection.execute(query, param,
            (result) => res.status(200).json(classParser(result.rows, classes.Owner)),
            (err) => res.status(404).json({
                message: err.message,
                details: err
            })
        );
    })
    .post((req, res) => {
        let query = 'INSERT INTO SW_Owner VALUES (seq_owner.NEXTVAL, :name, :password)',
            param = [req.body.name, req.body.password];

        oracleConnection.execute(query, param,
            (result) => res.status(201).json(result),
            (err) => res.status(500).json({
                message: err.message,
                details: err
            }));
    });

router.route('/:id')
    .get((req, res) => {
        let query = 'SELECT * from SW_Owner WHERE id = :id',
            param = [req.params.id],
            innerQuery = 'SELECT * from SW_Warehouse WHERE id_owner = :id';

        oracleConnection.execute(query, param,
            (result) => {
                let owner = classParser(result.rows, classes.FatOwner)[0];

                if (!owner)
                    res.sendStatus(404);
                else
                    oracleConnection.execute(innerQuery, param,
                        (result) => {
                            owner.warehouses = classParser(result.rows, classes.Warehouse);

                            res.status(200).json(owner);
                        },
                        (err) => res.status(404).json({
                            message: err.message,
                            details: err
                        })
                    );
            },
            (err) => res.status(404).json({
                message: err.message,
                details: err
            })
        );
    });

module.exports = router;