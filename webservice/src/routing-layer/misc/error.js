module.exports = {
    respondWith(res, code, error) {
        let err;

        switch (code) {
            case 400:
                err = new HttpError("Bad Request", "The Request was not a valid format.");
                break;
            case 403.1:
                code = 403;
                err = new HttpError("Authentification Error", "Wrong Combination of name and password.");
                break;

            default:
                code = 500;
                err = new HttpError("Internal Server Error", error.message);
        }

        res.status(code).json(err);
    }
};

class HttpError {
    constructor(message, details) {
        this.message = message;
        this.details = details;
    }
}