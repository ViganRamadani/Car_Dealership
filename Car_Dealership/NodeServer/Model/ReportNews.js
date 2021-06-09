const mongoose = require('mongoose')

const reports = mongoose.Schema({
    newsId: { type: String, required: true },
    reportReason: { type: String, required: true },
},
{
    timestamps: true
})

module.exports = mongoose.model('Reports', reports)