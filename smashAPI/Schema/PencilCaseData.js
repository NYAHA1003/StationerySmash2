const mongoose = require('mongoose');

const PencilCaseDataSchema =  mongoose.model('pencilCaseData', new mongoose.Schema({
    _maxCard : Number,
    _maxBadgeCount : Number,
    _needGauge : Number,
    _costSpeed : Number,
    _throwGaugeSpeed : Number,
    _name : String,
    _description : String,
    _pencilCaseType : Number,
    _pencilState : mongoose.Schema.Types.Mixed,
    _skinType : Number,
    _unitType : Number,
    CardData : mongoose.Schema.Types.Mixed,
    _badgeDatas : [mongoose.Schema.Types.Mixed]
    },
    { 
        versionKey : false
    },
));
exports.PencilCaseDataSchema = PencilCaseDataSchema;