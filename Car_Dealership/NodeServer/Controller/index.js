const express = require('express')
const router = express.Router()
const News = require('../Model/News');
const multer = require('multer');
const crypto = require("crypto");
var upload = multer({ dest: 'uploads/' }) // Creates a uploads/ Folder


var storage = multer.diskStorage({
    destination: (req, file, cb) => {
        cb(null, 'uploads/')
    },
    filename: (req, file, cb) => {
        cb(null, crypto.randomBytes(16).toString("hex") + ".jpg")
    }
});
var upload = multer({ storage: storage });


router.post('/createPhoto', upload.single('file'), (req, res) => {
    res.send({ file: req.file });
    console.log({ file: req.file })
});

router.post('/createNews', (req, res) => {
    console.log(req.body)

    News.create(req.body, (err, item) => {
        if (err) {
            console.log(err);
        }
        else {
            // item.save();
            res.send(req.body)
        }
    });
});
router.get('/allNews', (req, res) => {
    News.find((error, data) => {
        if (error) {
            return next(error)
        } else {
            res.json(data)
        }
    })
})

module.exports = router;