const express = require('express')
const router = express.Router()
const News = require('../Model/News');
const multer = require('multer');
const uploads = multer({ dest: 'uploads/' });
var storage = multer.diskStorage({
    destination: function (req, file, cb) {
        cb(null, 'uploads/')
    },
    filename: function (req, file, cb) {
        cb(null, crypto.randomBytes(16).toString("hex") + ".jpg") //Appending extension
    }
})
var upload = multer({ storage: storage });

router.post('/createNews', (req, res) => {
    News.create(req.body)
        .then(data => {
            res.status(200).send(data);
        }).catch(err => {
            res.send("Some error occurred.");
            console.log(err);
        });
});

module.exports = router;