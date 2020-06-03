var express = require('express');
var router = express.Router();

router.get('/', (req, res, next) => {
    var message = req.query.message;
    console.log('echoing message: ' + message);
    
    res.send(message);
});

module.exports = router;