const mongoose = require('mongoose');

const UnitDataSchema = mongoose.model('unitData', new mongoose.Schema({
    _hp: Number,
    _weight: Number,
    _knockback: Number,
    _dir: Number,
    _accuracy: Number,
    _moveSpeed: Number,
    _damage: Number,
    _attackSpeed: Number,
    _range: Number,
    _colideData: {
      originpoints: {
        type: [mongoose.Schema.Types.Mixed]
      }
    },
    _stickerType: Number,
    _attackType: Number,
    _unitType: Number,
    _unitablityData: {
      type: [Number]
    }
    },
    { 
        versionKey : false
    },
));
exports.UnitDataSchema = UnitDataSchema;