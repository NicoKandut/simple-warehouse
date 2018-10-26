const crypto = require('crypto'),
    errorResponse = require('../misc/error'),
    access = {};
    
module.exports = {
    access(req, res, next) {
        let token = req.headers.token;

        if (!token)
            errorResponse(res, 401.1);
        else if (!access[token])
            errorResponse(res, 403.2);
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

    remove(uid) {
        for (let token in access) {
            if(access[token] === uid){
                delete access[token];
                return true;
            }
        }

        return false;
    }
};