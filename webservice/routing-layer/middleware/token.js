const crypto = require('crypto');

module.exports = { //TODO: make truly random and unique
    access(req, res, next) {
        let token = req.headers.token;

        if (!token)
            return res.sendStatus(401);
        else if (!access[token])
            res.sendStatus(403);
        else {
            req.uid = access[token];
            next();
        }
    },

    get(token) {
        return access[token];
    },

    create(user) {
        let token = crypto.createHash('md5').update(user.id + salt).digest("hex");
        access[token] = user.id;
        return token;
    },

    remove(token) {
        delete access[token];
    }
};

let salt = "17seventeen";

const access = {};