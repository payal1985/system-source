import SQLite, { ResultSet, Transaction } from "react-native-sqlite-storage"
import { showAlert, showLog } from "./index"

export enum STATUS_CLIENT {
  IN_PROGRESS = "InProgress",
  SUBMITTED = "Submitted",
  INVALID_DATA = "InvalidData",
  EXISTED_SUBMISSON = "ExistedSubmission",
}

export enum STATUS_CLIENT_ID {
  EXISTED_SUBMISSION = 1,
  FAILED_SUBMISSION = 2,
  IN_PROGRESS = 3,
  SUBMITTED = 4,
}

export enum STATUS_CLIENT_TITLE {
  EXISTED_SUBMISSION = "Existed Submission",
  FAILED_SUBMISSION = "Failed Submission",
  IN_PROGRESS = "In-Progress",
  SUBMITTED = "Submitted",
}

let db: SQLite.SQLiteDatabase
//table table_inventory_client
const SQLITE_TABLE_NAME = "table_inventory_client"
const SQLITE_QUERY_SELECT_TABLE = `SELECT name FROM sqlite_master  WHERE type='table' AND name='${SQLITE_TABLE_NAME}'`
const FOREIGN_KEY = `FOREIGN KEY ( idInventory ) REFERENCES table_inventory ( id ) `
const SQLITE_QUERY_CREATE_TABLE = `CREATE TABLE IF NOT EXISTS ${SQLITE_TABLE_NAME}(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,userId VARCHAR(50), deviceDate VARCHAR(50), client NVARCHAR ,clientId  VARCHAR(255), status VARCHAR(50), isBarScan INTEGER, idInventory VARCHAR(20), dataItemType NVARCHAR, InventoryClientGroupID INTEGER, LoginUserID INTEGER)`

const SQLITE_DELETE_TABLE = `DROP TABLE IF EXISTS ${SQLITE_TABLE_NAME}`
//collection
const SQLITE_INSERT_TABLE = `INSERT INTO ${SQLITE_TABLE_NAME} (userId, deviceDate,  client, clientId, status, isBarScan, idInventory, dataItemType, InventoryClientGroupID) VALUES (?,?,?,?,?,?,?,?,?)`
const SQLITE_SELECT_ALL_TABLE = `SELECT * FROM  ${SQLITE_TABLE_NAME}`
const SQLITE_DELETE_COLLECTION = `DELETE FROM ${SQLITE_TABLE_NAME} where id=?`
const SQLITE_UPDATE_COLLECTION = `UPDATE  ${SQLITE_TABLE_NAME} set userId=?, deviceDate=?, client=?,clientId=?, status = ?,isBarScan = ?, idInventory =?, dataItemType = ?, LoginUserID= ? where id=?`

export const initTableInventoryClient = async (dbRoot: SQLite.SQLiteDatabase) => {
  if (dbRoot) {
    console.log("dbRoot", dbRoot)
    try {
      db = dbRoot
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_QUERY_SELECT_TABLE, [], function (tx: Transaction, res: ResultSet) {
          console.log("initTableInventoryClient", res)
          if (res?.rows?.length === 0) {
            txn.executeSql(SQLITE_DELETE_TABLE, [])
            txn.executeSql(SQLITE_QUERY_CREATE_TABLE, [], (e) => {
              console.log("**** Database Table table_inventory_client Create *********", e)
            })
          }
        })
      })
    } catch (e) {}
  }
}

export const insertDataInventoryClient = async (
  data: any,
  successCb?: (idClient: number) => void,
  errorCb?: () => void,
) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_INSERT_TABLE, data, function (tx: Transaction, res: ResultSet) {
          console.log("insertDataInventoryClient", res)
          if (res.rowsAffected > 0) {
            successCb?.(res?.insertId)
            // showAlert("Save Success!", "Your data saved Successfully", successCb)
            showLog(" ***** Your data saved Successfully SQLite ********* ID " + res?.insertId)
          } else {
            //  showAlert("Save Error!", "Your data saved error, please try again!", errorCb)
            showLog(' ***** Save Error!", "Your data saved error SQLite, please try again! *********')
            errorCb?.()
          }
        })
      })
    } catch (e) {
      showLog(' ***** Save Error!", "Your data saved error SQLite, please try again! *********')
      errorCb?.()
      // showAlert("Save Error!", "Your data saved error, please try again!", errorCb)
    }
  }
}

export const selectAllDataInventoryClient = async (Cb?: (formatData: any) => void) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_SELECT_ALL_TABLE, [], function (tx: Transaction, res: ResultSet) {
          const data = []
          if (res.rows?.length > 0) {
            for (let i = 0; i < res.rows.length; ++i) data.push(res.rows.item(i))
          }
          Cb?.(data)
        })
      })
    } catch (e) {
      showAlert("Get Error!", "Your data get error, please try again!")
    }
  }
}

export const deleteDataInventoryClient = async (
  idCollection: string | number,
  successCb?: () => void,
  errorCb?: () => void,
) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_DELETE_COLLECTION, [idCollection], function (tx: Transaction, res: ResultSet) {
          if (res.rowsAffected > 0) {
            showLog("Delete Success! Your data delete Successfully")
            successCb?.()
          } else {
            showLog("Delete Error! Your data delete error or not found, please try again!")
            errorCb?.()
          }
        })
      })
    } catch (e) {
      showLog("Delete Error! Your data delete error, please try again!")
      errorCb?.()
    }
  }
}

export const updateDataItemTypeInClient = async (
  id: any,
  itemType: any,
  successCb?: () => void,
  errorCb?: () => void,
) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(
          `SELECT * FROM  ${SQLITE_TABLE_NAME} where id= ?`,
          [id],
          function (tx: Transaction, res: ResultSet) {
            if (res.rows?.length > 0) {
              const dataDetail = res.rows.item(0)
              let dataItemType = dataDetail?.dataItemType === "" ? [] : JSON.parse(dataDetail?.dataItemType)
              if (Array.isArray(dataItemType)) {
                dataItemType.push(itemType)
                txn.executeSql(
                  `UPDATE ${SQLITE_TABLE_NAME} set dataItemType = ? where id=?`,
                  [JSON.stringify(dataItemType), id],
                  function (tx: Transaction, res: ResultSet) {
                    if (res.rowsAffected > 0) {
                      successCb?.()
                      showLog("Update Success! Your data dataItemType Updated Successfully")
                    } else {
                      showLog("Update Error! Your data Update dataItemType error or not found, please try again!")
                      errorCb?.()
                    }
                  },
                )
              }
            } else {
              errorCb?.()
            }
          },
        )
      })
    } catch (e) {
      errorCb?.()
      showLog("Update Error! Your data Update dataItemType error or not found, please try again!")
    }
  }
}

export const updateDataInventoryClient = async (data: any, successCb?: () => void, errorCb?: () => void) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_UPDATE_COLLECTION, data, function (tx: Transaction, res: ResultSet) {
          if (res.rowsAffected > 0) {
            showAlert("Update Success!", "Your data Updated Successfully", successCb)
          } else {
            showAlert("Update Error!", "Your data Update error or not found, please try again!", errorCb)
          }
        })
      })
    } catch (e) {
      showAlert("Update Error!", "Your data Update error, please try again!")
    }
  }
}

export const getDetailInventoryClient = async (id: any, successCb?: (data: any) => void, errorCb?: () => void) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(
          `SELECT * FROM  ${SQLITE_TABLE_NAME} where id = ?`,
          [id],
          function (tx: Transaction, res: ResultSet) {
            if (res.rows?.length > 0) {
              //get data table inventory
              let item = res.rows.item(0) || {}
              item.clientId = parseInt(item.clientId)
              item.userId = parseInt(item.userId)
              if (item?.dataItemType) {
                item.dataItemType = JSON.parse(item.dataItemType)
              } else {
                item.dataItemType = []
              }
              item.isBarScan = item.isBarScan == 1 ? true : false
              successCb?.(item)
            } else {
              successCb?.(null)
            }
          },
        )
      })
    } catch (e) {
      errorCb?.()
    }
  }
}

export const getListByInventoryId = async (
  idInventory: string,
  successCb?: (data: any) => void,
  errorCb?: () => void,
) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(
          `SELECT * FROM  ${SQLITE_TABLE_NAME} where idInventory= ?`,
          [idInventory],
          function (tx: Transaction, res: ResultSet) {
            if (res.rows?.length > 0) {
              const data = []
              if (res.rows?.length > 0) {
                for (let i = 0; i < res.rows.length; ++i) data.push(res.rows.item(i))
              }
              successCb?.(data)
            } else {
              successCb?.([])
            }
          },
        )
      })
    } catch (e) {
      errorCb?.()
    }
  }
}

export const getListByInventoryIdAndStatus = async (
  idInventory: string,
  status: string,
  successCb?: (data: any) => void,
  errorCb?: () => void,
) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(
          `SELECT * FROM  ${SQLITE_TABLE_NAME} where idInventory= ? and status= ?`,
          [idInventory, status],
          function (tx: Transaction, res: ResultSet) {
            if (res.rows?.length > 0) {
              const data = []
              if (res.rows?.length > 0) {
                for (let i = 0; i < res.rows.length; ++i) data.push(res.rows.item(i))
              }
              successCb?.(data)
            } else {
              successCb?.([])
            }
          },
        )
      })
    } catch (e) {
      errorCb?.()
    }
  }
}

export const getListInventoryClientByQuery = async (
  query: string,
  data: string[],
  successCb?: (data: any) => void,
  errorCb?: () => void,
) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(
          `SELECT * FROM  ${SQLITE_TABLE_NAME} where ${query}`,
          data,
          function (tx: Transaction, res: ResultSet) {
            if (res.rows?.length > 0) {
              const data = []
              if (res.rows?.length > 0) {
                for (let i = 0; i < res.rows.length; ++i) {
                  let item = res.rows.item(i)
                  item.clientId = parseInt(item.clientId)
                  item.userId = parseInt(item.userId)
                  if (item.dataItemType) {
                    item.dataItemType = JSON.parse(item.dataItemType)
                  } else {
                    item.dataItemType = []
                  }
                  item.isBarScan = item.isBarScan == 1 ? true : false
                  data.push(res.rows.item(i))
                }
              }
              successCb?.(data)
            } else {
              successCb?.([])
            }
          },
        )
      })
    } catch (e) {
      errorCb?.()
    }
  }
}

export const updateDataInventoryClientByQuery = async (
  query: string,
  data: any,
  successCb?: () => void,
  errorCb?: () => void,
) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        console.log("query", { query, data })
        txn.executeSql(`UPDATE  ${SQLITE_TABLE_NAME} set ${query}`, data, function (tx: Transaction, res: ResultSet) {
          if (res.rowsAffected > 0) {
            showLog("Update Success! Your data Updated Successfully table_inventory_client")
            successCb?.()
          } else {
            showLog("Update Error! Your data Update table_inventory_client error or not found, please try again!")
            errorCb?.()
          }
        })
      })
    } catch (e) {
      showLog("Update Error! Your data Update error table_inventory_client, please try again!")
      errorCb?.()
    }
  }
}
