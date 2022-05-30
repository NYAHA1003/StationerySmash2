const mongoose = require('mongoose');

const StrategyDataSchema = mongoose.model('strategyData', new mongoose.Schema({
    
    _starategyType : Number,
    _starategyablityData : [Number],
    },
    { 
        versionKey : false
    },
));
exports.StrategyDataSchema = StrategyDataSchema;