const mongoose = require('mongoose');

const StickerDataSchema =  mongoose.model('stickerData', new mongoose.Schema({
    _onlyUnitType : Number,
	_stickerType : Number,
	_skinType : Number,
	_name : String,
	_decription : String,
    _level : Number
    },
    { 
        versionKey : false
    },
));
exports.StickerDataSchema = StickerDataSchema;