const express = require('express')
const router = express.Router()
const News = require('../Model/News');
const ReportNews = require('../Model/ReportNews')
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
             item.save();
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
router.delete('/remove_news/:id', (req, res) => {
    var id = req.params.id;
    console.log(id);
    News.findByIdAndDelete(id, function (err, data) {
        if (err) {
            console.log(err);
        } else {
            res.json(data)
        }
    });
        
});

router.post('/report_news/:id', async(req, res) => {
    try {
        console.log(req.body, req.params)
        const report = new ReportNews({
            newsId: req.body.newsId,
            reportReason: req.body.reportReason
        })


        await report.save();
        console.log("Report Created!")
        return res.status(201).json(report)

    } catch (err) {
        return console.log(err)
    }
})

router.get('/getNewsReports', (req, res) => {
    ReportNews.find().sort({ createdAt: -1 })
        .then(reports => {
            res.status(200).json(reports)
            console.log(reports)
        })
        .catch(err => {
            console.log(err)
            res.status(500).json(err)
        })
})

router.delete('/removeNewsReport/:id', (req, res) => {
    console.log(req.params.id)
    ReportNews.findByIdAndDelete(req.params.id)
        .then(data => {
            res.status(200).json(data)
        })
        .catch(err => {
            res.send(err)
        })
})

/*app.get('/img', function (req, res) {
    res.sendFile(__dirname + "/index.html");
});
*/
module.exports = router;