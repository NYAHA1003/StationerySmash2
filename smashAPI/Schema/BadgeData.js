const mongoose = require('mongoose');

const BadgeDataSchema =  mongoose.model('badgeData', new mongoose.Schema({
    _level : Number,
    _name : String,
    _decription : String,
    _skinType : Number,
    _badgeType : Number
    },
    { 
        versionKey : false
    },
));
exports.BadgeDataSchema = BadgeDataSchema;

