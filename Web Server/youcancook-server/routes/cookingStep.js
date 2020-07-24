var express = require('express');
var router = express.Router();
var {azuredb, Request} = require('../azuredb');

router.get('/', (req, res, next) => {
    console.log('selecting cooking steps where foodID: ' + req.query.foodID);

    // generate query statement
    var request = new Request(
        'SELECT CookingStep.Description, Recipe.CookingOrder '+
        'FROM Recipe join CookingStep on Recipe.CookingStepID = CookingStep.ID where Recipe.FoodID = '
        + req.query.foodID, err => {
            if(err) {
                console.error(err.message);
            }
    });
  
    // the callback will be called once per each row,
    // so res.send() shouldn't be used inside
    request.on('row', (columns) => {
        columns.forEach((column, index) => {
            // column name and value are seperated by colon
            res.write(column.metadata.colName + ":" + column.value);
            console.log(column.metadata.colName + ":" + column.value);

            // each column is seperated by comma
            if(index < columns.length-1) {
            res.write(",");
            }
        })

        // each row is seperated by newline
        res.write("\n");
    });

    // send result after writing every row
    // one of the three events(done, doneInProc, doneProc) will happen when result handling is done
    endResponseIfDone = function(rowCount, more, rows) {
        if(!more) {
            res.end();
        }
    };
    request.on('done', endResponseIfDone);
    request.on('doneInProc', endResponseIfDone);
    request.on('doneProc', endResponseIfDone);

    azuredb.execSql(request);
});

module.exports = router;