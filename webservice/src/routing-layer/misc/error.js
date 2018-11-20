class HttpError {
    constructor(message, details) {
        this.message = message;
        this.details = details;
    }
}

module.exports = (res, code, cause) => {
    let err;

    if (cause) //IDEA: switch case for cause - make errors more specific
        err = new HttpError("Database Error", cause.message);
    else
        switch (code) {
            case 400.1:
                code = 400;
                err = new HttpError("No Attribute specified", "You need to specify at least one attribute to update.");
                break;
            case 400.2:
                code = 400;
                err = new HttpError("Missing Attributes", "You need to specify name and password.");
                break;
            case 401.1:
                code = 401;
                err = new HttpError("Missing Token", "You need to log in, to view this resource.");
                break;
            case 403.1:
                code = 403;
                err = new HttpError("Login Failed", "Wrong Combination of name and password.");
                break;
            case 403.2:
                code = 403;
                err = new HttpError("Wrong Token", "Provided token is either outdated or invalid.");
                break;
            case 404.2:
                code = 404;
                err = new HttpError("Account Not Found", "No Account was found for the given id.");
                break;
            case 404.3:
                code = 404;
                err = new HttpError("Warehouse Not Found", "No Warehouse was found for the given id.");
                break;
            case 404.4:
                code = 404;
                err = new HttpError("Manufacturer Not Found", "No Manufacturer was found for the given id.");
                break;

            default:
                code = 500;
                err = new HttpError("Internal Server Error", "Please dont't do that again.");
        }

        console.error(code, " : ", err);

    res.status(code).json(err);
};