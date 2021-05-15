const express = require('express');
const morgan = require('morgan');
const cors = require('cors');
const mongoose = require('mongoose');
const app = express();


mongoose.set('useFindAndModify', false);
//configure database and mongoose
mongoose.set('useCreateIndex', true);
mongoose // Connect to the database
    .connect("mongodb://localhost:27017/Car_Dealership", { useNewUrlParser: true, useUnifiedTopology: true })
    .then(() => {
        console.log('Database is connected');
    })
    .catch(err => {
        console.log({ database_error: err });
    })
// db configuaration ends here

//registering cors 
app.use(cors());

app.use(express.urlencoded({ extended: false }));
app.use(express.json());

app.use(morgan('dev')); // configire morgan

const PORT = 3000
app.listen(PORT, (err) => {
    if (err) {
        console.log("Huoston, we got a problem.", err);
    } else
        console.log(`App is running on port: ${ PORT }`);
})

// define first route
app.get('/', (req, res) => {
    res.send("Send");
})

const NewsRoutes = require('./Controller/index');

app.use('/user', NewsRoutes);