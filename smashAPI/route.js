const mongoose = require('mongoose');
const express = require('express');
const router = express.Router();
const userSave = require('./Schema/UserSave');
const unitData = require('./Schema/UnitData');
const stickerData = require('./Schema/StickerData');
const pencilCaseData = require('./Schema/PencilCaseData');
const badgeData = require('./Schema/BadgeData');
const strategyData = require('./Schema/StrategyData');
const deckData = require('./Schema/DeckData');

//유저 세이브 데이터 업데이트
function UpdateUserSaveData(req, res, next) {
  userSave.UserSaveSchema.findOneAndUpdate({ _userID: req.body.post._userID }, req.body.post, { upsert: true },)
    .then(() => {
      res.status(200).json({
        message: "UPDATE",
      });
    })
    .catch(err => {
      res.status(500).json({
        message: err
      });
    });
}

//유저 세이브 데이터 검색
function FindUserSaveData(req, res) {
  userSave.UserSaveSchema.findOne({ _userID: req.body.post._userID })
    .then(post => {
      if (!post) {
        //데이터가 없으면 NONE 메시지를 반환
        return res.status(200).json({ message: "NONE" });
      }
      else {
        //데이터가 있으면 FIND, 찾은 값을 반환
        console.log("Read Detail 완료");
        res.status(200).json({
          message: "FIND",
          post
        });
      }
    })
    .catch(err => {
      res.status(500).json({
        message: err
      });
    });
}

//유저 세이브 데이터 넣거나 가져오기
router.post('/UserSaveData', (req, res, next) => {
  switch (req.body.message) {
    case 'GET':
      console.log('get');
      FindUserSaveData(req, res);
      break;

    case 'POST':
      console.log('post');
      UpdateUserSaveData(req, res, next);
      break;
  }
});

//유닛데이터 데이터 넣기
router.post('/UnitData/Post', (req, res, next) => {
  
  unitData.UnitDataSchema.findOneAndUpdate({ _unitType : req.body._unitType }, req.body, { upsert: true },)
    .then((result) => {
      res.json(result);
    })
    .catch((err) => {
      console.error(err);
      next(err);
    })
});

//유닛데이터 데이터 가져오기
router.get('/UnitData/Get', (req, res, next) => {

  unitData.UnitDataSchema.find()
    .then(post => {
      if (!post) {
        //하나도 데이터가 없으면 에러 반환
        return res.status(500).json({
          message: "None"
        });
      }
      else {
        //데이터가 있으면 반환
        res.status(200).json({
          post
        });
      }
    })
    .catch(err => {
      res.status(500).json({
        message: err
      });
    });
});

//스티커데이터 데이터 넣기
router.post('/StickerData/Post', (req, res, next) => {
  
  stickerData.StickerDataSchema.findOneAndUpdate({ _stickerType : req.body._stickerType }, req.body, { upsert: true },)
    .then((result) => {
      res.json(result);
    })
    .catch((err) => {
      console.error(err);
      next(err);
    })
});

//스티커데이터 데이터 가져오기
router.get('/StickerData/Get', (req, res, next) => {

  stickerData.StickerDataSchema.find()
    .then(post => {
      if (!post) {
        //하나도 데이터가 없으면 에러 반환
        return res.status(500).json({
          message: "None"
        });
      }
      else {
        //데이터가 있으면 반환
        res.status(200).json({
          post
        });
      }
    })
    .catch(err => {
      res.status(500).json({
        message: err
      });
    });
});


//필통데이터 데이터 넣기
router.post('/PencilCaseData/Post', (req, res, next) => {
  
  pencilCaseData.PencilCaseDataSchema.findOneAndUpdate({ _pencilCaseType : req.body._pencilCaseType }, req.body, { upsert: true },)
    .then((result) => {
      res.json(result);
    })
    .catch((err) => {
      console.error(err);
      next(err);
    })
});

//필통데이터 데이터 가져오기
router.get('/PencilCaseData/Get', (req, res, next) => {

  pencilCaseData.PencilCaseDataSchema.find()
    .then(post => {
      if (!post) {
        //하나도 데이터가 없으면 에러 반환
        return res.status(500).json({
          message: "None"
        });
      }
      else {
        //데이터가 있으면 반환
        res.status(200).json({
          post
        });
      }
    })
    .catch(err => {
      res.status(500).json({
        message: err
      });
    });
});

//뱃지데이터 데이터 넣기
router.post('/BadgeData/Post', (req, res, next) => {
  
  badgeData.BadgeDataSchema.findOneAndUpdate({ _badgeType : req.body._badgeType }, req.body, { upsert: true },)
    .then((result) => {
      res.json(result);
    })
    .catch((err) => {
      console.error(err);
      next(err);
    })
});

//뱃지데이터 데이터 가져오기
router.get('/BadgeData/Get', (req, res, next) => {

  badgeData.BadgeDataSchema.find()
    .then(post => {
      if (!post) {
        //하나도 데이터가 없으면 에러 반환
        return res.status(500).json({
          message: "None"
        });
      }
      else {
        //데이터가 있으면 반환
        res.status(200).json({
          post
        });
      }
    })
    .catch(err => {
      res.status(500).json({
        message: err
      });
    });
});

//전략데이터 데이터 넣기
router.post('/StrategyData/Post', (req, res, next) => {
  
  strategyData.StrategyDataSchema.findOneAndUpdate({ _starategyType : req.body._starategyType }, req.body, { upsert: true },)
    .then((result) => {
      res.json(result);
    })
    .catch((err) => {
      console.error(err);
      next(err);
    })
});

//전략데이터 데이터 가져오기
router.get('/StrategyData/Get', (req, res, next) => {

  strategyData.StrategyDataSchema.find()
    .then(post => {
      if (!post) {
        //하나도 데이터가 없으면 에러 반환
        return res.status(500).json({
          message: "None"
        });
      }
      else {
        //데이터가 있으면 반환
        res.status(200).json({
          post
        });
      }
    })
    .catch(err => {
      res.status(500).json({
        message: err
      });
    });
});


//카드데이터 데이터 넣기
router.post('/DeckData/Post', (req, res, next) => {
  
  deckData.DeckDataSchema.findOneAndUpdate({ _cardNamingType : req.body._cardNamingType }, req.body, { upsert: true },)
    .then((result) => {
      res.json(result);
    })
    .catch((err) => {
      console.error(err);
      next(err);
    })
});

//카드데이터 데이터 가져오기
router.get('/DeckData/Get', (req, res, next) => {

  deckData.DeckDataSchema.find()
    .then(post => {
      if (!post) {
        //하나도 데이터가 없으면 에러 반환
        return res.status(500).json({
          message: "None"
        });
      }
      else {
        //데이터가 있으면 반환
        res.status(200).json({
          post
        });
      }
    })
    .catch(err => {
      res.status(500).json({
        message: err
      });
    });
});

module.exports = router;