import SQLite, { ResultSet, Transaction } from "react-native-sqlite-storage"
import { showAlert, showLog } from "./index"

let db: SQLite.SQLiteDatabase
//table table_inventory_item_type
const SQLITE_TABLE_NAME_ITEM_TYPE = "table_inventory_item_type"
const SQLITE_QUERY_SELECT_TABLE = `SELECT name FROM sqlite_master  WHERE type='table' AND name='${SQLITE_TABLE_NAME_ITEM_TYPE}'`
const FOREIGN_KEY = `FOREIGN KEY ( idInventoryClient ) REFERENCES table_inventory_client ( id ) `
// const SQLITE_QUERY_CREATE_TABLE = `CREATE TABLE IF NOT EXISTS ${SQLITE_TABLE_NAME_ITEM_TYPE}(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,itemTypeID INTEGER, idInventoryClient INTEGER, itemTypeName VARCHAR(255), itemTypeOption NVARCHAR,  ${FOREIGN_KEY})`
const SQLITE_QUERY_CREATE_TABLE = `CREATE TABLE IF NOT EXISTS ${SQLITE_TABLE_NAME_ITEM_TYPE}(itemTypeID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, idInventoryClient INTEGER, itemTypeName VARCHAR(255), itemTypeOption NVARCHAR,  ${FOREIGN_KEY})`
const SQLITE_DELETE_TABLE = `DROP TABLE IF EXISTS ${SQLITE_TABLE_NAME_ITEM_TYPE}`
//collection
const SQLITE_INSERT_TABLE = `INSERT INTO ${SQLITE_TABLE_NAME_ITEM_TYPE} (idInventoryClient, itemTypeName, itemTypeOption) VALUES (?,?,?)`
const SQLITE_SELECT_ALL_TABLE = `SELECT * FROM  ${SQLITE_TABLE_NAME_ITEM_TYPE}`
const SQLITE_DELETE_COLLECTION = `DELETE FROM ${SQLITE_TABLE_NAME_ITEM_TYPE} where itemTypeID=?`
const SQLITE_UPDATE_COLLECTION = `UPDATE  ${SQLITE_TABLE_NAME_ITEM_TYPE} set idInventoryClient = ? , itemTypeName = ? , itemTypeOption = ? , where itemTypeID=?`

export const initTableItemType = async (dbRoot: SQLite.SQLiteDatabase) => {
  if (dbRoot) {
    try {
      db = dbRoot
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_QUERY_SELECT_TABLE, [], function (tx: Transaction, res: ResultSet) {
          if (res?.rows?.length === 0) {
            txn.executeSql(SQLITE_DELETE_TABLE, [])
            txn.executeSql(SQLITE_QUERY_CREATE_TABLE, [], () => {
              console.log("**** Database Table table_inventory_item_type Create *********")
            })
          }
        })
      })
    } catch (e) {}
  }
}

export const insertDataItemType = async (data: any, successCb?: () => void, errorCb?: () => void) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_INSERT_TABLE, data, function (tx: Transaction, res: ResultSet) {
          if (res.rowsAffected > 0) {
            // showAlert("Save Success!", "Your data saved Successfully", successCb)
            showLog(" ***** Your data Item Type saved Successfully SQLite ********* ID " + res?.insertId)
          } else {
            showLog(' ***** Save Error!", "Your data Item Type saved error SQLite, please try again! *********')
            // showAlert("Save Error!", "Your data saved error, please try again!", errorCb)
          }
        })
      })
    } catch (e) {
      showLog(' ***** Save Error!", "Your data Item Type saved error SQLite, please try again! *********')
      // showAlert("Save Error!", "Your data saved error, please try again!", errorCb)
    }
  }
}

export const selectAllDataItemType = async (Cb?: (formatData: any) => void) => {
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

export const deleteDataItemType = async (
  idCollection: string | number,
  successCb?: () => void,
  errorCb?: () => void,
) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_DELETE_COLLECTION, [idCollection], function (tx: Transaction, res: ResultSet) {
          if (res.rowsAffected > 0) {
            // showAlert("Delete Success!", "Your data delete Successfully", successCb)
            showLog("Delete Success! Your data delete Successfully")
          } else {
            showLog("Delete Error! Your data delete error or not found, please try again!")
            // showAlert("Delete Error!", "Your data delete error or not found, please try again!", errorCb)
          }
        })
      })
    } catch (e) {
      showLog("Delete Error! Your data delete error, please try again!")
      // showAlert("Delete Error!", "Your data delete error, please try again!")
    }
  }
}

export const updateDataItemType = async (data: any, successCb?: () => void, errorCb?: () => void) => {
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

export const getDetailItemType = async (id: any, successCb?: (data: any) => void, errorCb?: () => void) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(
          `SELECT * FROM  ${SQLITE_TABLE_NAME_ITEM_TYPE} where id= ?`,
          [id],
          function (tx: Transaction, res: ResultSet) {
            if (res.rows?.length > 0) {
              successCb?.(res.rows.item(0))
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

export const getListItemTypeByIdClient = async (
  idInventoryClient: string,
  successCb?: (data: any) => void,
  errorCb?: () => void,
) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(
          `SELECT * FROM  ${SQLITE_TABLE_NAME_ITEM_TYPE} where idInventoryClient= ?`,
          [idInventoryClient],
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
