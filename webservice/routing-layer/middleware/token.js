const crypto = require('crypto');

module.exports = {
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
        let token;

        do {
            token = crypto.createHash('md5').update(crypto.randomBytes(24).join()).digest("hex");
        } while (token in access);

        access[token] = user.id;
        return token;
    },

    remove(token) {
        if (token in access) {
            delete access[token];
            return true;
        }
    }
};

let salt = "17seventeen";

const access = {};