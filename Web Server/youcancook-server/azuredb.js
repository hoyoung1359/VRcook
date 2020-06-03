const { Connection, Request } = require("tedious");

const config = {
  authentication: {
    options: {
      userName: "youcancook",
      password: "ycc1357*"
    },
    type: "default"
  },
  server: "youcancook-db-server.database.windows.net",
  options: {
    database: "youcancook-testdb",
    encrypt: true
  }
};

const connection = new Connection(config);

connection.on("connect", err => {
  if (err) {
    console.error(err.message);
  } else {
    console.log("successfully connected to azure db server");
  }
});

module.exports = { 
    azuredb: connection,
    Request: Request
};