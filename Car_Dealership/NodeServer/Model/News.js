const mongoose = require('mongoose');

let NewsSchema = mongoose.Schema({
    title: { type: String, required: true },
    description: { type: String, required: true },
    img_path: {type:String}
},
    
    {
        timestamps: true
    })
var collectionName = 'News'
module.exports = mongoose.model('News', NewsSchema, collectionName)