const express = require('express');
const path = require('path');
const app = express();
const db = require('./db.js'); // db 불러오기
const route = require('./route.js');
const bodyParser = require('body-parser');

app.set('view engine', 'pug');
app.set('views', path.join('테스트API', 'html'));
db(); // 실행
app.use(express.static(path.join('테스트API', 'html')));
app.use(express.json());
app.use(bodyParser.urlencoded({extended:true})); 
app.use(bodyParser.json()); 
app.use('/', route);

app.listen(80, () => {
  console.log("REST API 테스트");
});