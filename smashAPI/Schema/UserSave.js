const mongoose = require('mongoose');

const UserSaveSchema = mongoose.model('users', new mongoose.Schema({
    _userID: String,
    _haveCardSaveDatas: {
        type: [mongoose.Schema.Types.Mixed]
      },
      _haveSkinList: {
        type: [Number]
      },
      _haveStickerList: {
        type: [mongoose.Schema.Types.Mixed]
      },
      _havePencilCaseList: {
        type: [mongoose.Schema.Types.Mixed]
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
      _presetCardDatas1 : [Number],
      _presetCardDatas2 : [Number],
      _presetCardDatas3 : [Number],
      _presetPencilCaseType1 : Number,
      _presetPencilCaseType2 : Number,
      _presetPencilCaseType3 : Number,
      _setPrestIndex: Number,
      _money: Number,
      _dalgona: Number,
      _name: String,
      _level: Number,
      _nowExp: Number,
      _lastPlayStage: Number,
      _winCount: Number,
      _winningStreakCount: Number,
      _loseCount: Number,
      _themeSkinType : Number
    },
    { 
        versionKey : false
    },
));
exports.UserSaveSchema = UserSaveSchema;