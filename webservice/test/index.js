const expect = require('chai').expect,
    request = require('supertest');

let app,
    token;

describe('simple-warehouse-api', function () {
    this.timeout(10000);

    before(function (done) {
        app = require("../src/app");
        app.listen(function (err) {
            if (err)
                return done(err);
            else
                done();
        });
    });

    it('api up', function () {
        request(app)
            .get('/')
            .expect(200);
    });

    describe('auth', function () {
        describe('login', function () {
            it('no credentials', function () {
                request(app)
                    .post('/auth/login')
                    .send({})
                    .expect(403);
            });

            it('wrong credentials', function () {
                request(app)
                    .post('/auth/login')
                    .send({
                        name: "asdf938264",
                        password: "923723o749237427349723974"
                    })
                    .expect(403);
            });

            it('valid credentials', function () {
                request(app)
                    .post('/auth/login')
                    .set('Accept', 'application/json')
                    .send({
                        name: "Martin",
                        password: "martin3101"
                    })
                    .expect(200)
                    .end(function (err, res) {
                        if (err)
                            return done(err);
                        if (!res || !res.body.token)
                            return done(new Error("no token in response"));

                        token = res.body.token;
                        done();
                    });
            });
        });

        describe('logout', function() {
            it("valid", function(done) {
                request(app)
                    .get('/auth/logout')
                    .set('token', token)
                    .expect(200)
                    .end(done);
            });
        });
    });

    describe('catalog', function () {
        it('catalog exists', function () {
            request(app)
                .get('/catalog/')
                .expect('Content-Type', /json/)
                .expect(200);
        });

        it('products exists', function () {
            request(app)
                .get('/catalog/products')
                .expect('Content-Type', /json/)
                .expect(200);
        });

        it('manufacturers exists', function () {
            request(app)
                .get('/catalog/manufacturers')
                .expect('Content-Type', /json/)
                .expect(200);
        });

        it('single manufacturer exists', function () {
            request(app)
                .get('/catalog/manufacturers/1')
                .expect('Content-Type', /json/)
                .expect(200);
        });

        it('single manufacturer invalid', function () {
            request(app)
                .get('/catalog/manufacturers/aaa')
                .expect('Content-Type', /json/)
                .expect(404);
        });
    });
});