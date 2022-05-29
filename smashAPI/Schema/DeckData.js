const mongoose = require('mongoose');

const DeckDataSchema = mongoose.model('cardData', new mongoose.Schema({

    _cardNamingType: Number,
    _cardType: Number,
    _starategyType: Number,
    _unitType: Number,
    _name: String,
    _description: String,
    _cost: Number,
    _skinData: {
        _skinType: Number
    },
    _effectType: Number,
    _level: Number
},
    {
        versionKey: false
    },
));
exports.DeckDataSchema = DeckDataSchema;

