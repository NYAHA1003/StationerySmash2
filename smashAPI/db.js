const mongoose = require('mongoose');
module.exports = () => {
  function connect() {
    mongoose.connect('mongodb+srv://moonkanghyuck:zxc123zxc123561@cluster0.aqves.mongodb.net/?retryWrites=true&w=majority', {dbName: 'MoonTestBase'}, function(err) {
      if (err) {
        console.error('mongodb connection error', err);
      }
      console.log('mongodb connected');
    });
  }
  connect();
  mongoose.connection.on('disconnected', connect);
  require('./user.js'); 
};