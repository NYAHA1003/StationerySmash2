const mongoose = require('mongoose');

const UserSchema = mongoose.model('users', new mongoose.Schema({
    _userID: String,
    _haveCardSaveDatas: {
        type: [mongoose.Schema.Types.Mixed]
      },
      _ingameSaveDatas: {
        type: [mongoose.Schema.Types.Mixed]
      },
      _haveSkinList: {
        type: [Number]
      },
      _haveStickerList: {
        type: [mongoose.Schema.Types.Mixed]
      },
      _havePencilCaseList: {
        type: [Number]
      },
      _currentPencilCaseType: Number,
      _haveBadgeSaveDatas: {
        type: [mongoose.Schema.Types.Mixed]
      },
      _currentProfileType: Number,
      _haveProfileList: {
        type: [Number]
      },
      _materialDatas: {
        type: [mongoose.Schema.Types.Mixed]
      },
      _haveCollectionDatas: {
        type: [mongoose.Schema.Types.Mixed]
      },
      _setPrestIndex: Number,
      _money: Number,
      _dalgona: Number,
      _name: String,
      _level: Number,
      _nowExp: Number,
      _lastPlayStage: Number,
      _winCount: Number,
      _winningStreakCount: Number,
      _loseCount: Number
    },
    { 
        versionKey : false
    },
));
exports.UserSchema = UserSchema;