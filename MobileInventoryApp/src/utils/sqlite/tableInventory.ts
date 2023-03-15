import SQLite, { ResultSet, Transaction } from "react-native-sqlite-storage"
import { showAlert } from "./index"

let db: SQLite.SQLiteDatabase
//table inventory
export const SQLITE_TABLE_NAME_INVENTORY = "table_inventory"
const SQLITE_QUERY_SELECT_TABLE = `SELECT name FROM sqlite_master  WHERE type='table' AND name='${SQLITE_TABLE_NAME_INVENTORY}'`
const SQLITE_QUERY_CREATE_TABLE = `CREATE TABLE IF NOT EXISTS ${SQLITE_TABLE_NAME_INVENTORY}(id VARCHAR(20) PRIMARY KEY, text VARCHAR(255), version VARCHAR(20), buble INT(10))`
const SQLITE_DELETE_TABLE = `DROP TABLE IF EXISTS ${SQLITE_TABLE_NAME_INVENTORY}`
//collection
const SQLITE_INSERT_TABLE = `INSERT INTO ${SQLITE_TABLE_NAME_INVENTORY} (id, text, version,buble) VALUES (?,?,?,?)`
const SQLITE_SELECT_ALL_TABLE = `SELECT * FROM  ${SQLITE_TABLE_NAME_INVENTORY}`
const SQLITE_DELETE_COLLECTION = `DELETE FROM ${SQLITE_TABLE_NAME_INVENTORY} where id=?`
const SQLITE_UPDATE_COLLECTION = `UPDATE  ${SQLITE_TABLE_NAME_INVENTORY} set text=?, version=?, buble=? where id=?`
const SQLITE_SELECT_TABLE = `SELECT * FROM  ${SQLITE_TABLE_NAME_INVENTORY} where id= ?`

export const initTableInventory = (dbRoot: SQLite.SQLiteDatabase) => {
  if (dbRoot) {
    try {
      db = dbRoot
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_QUERY_SELECT_TABLE, [], function (tx: Transaction, res: ResultSet) {
          if (res?.rows?.length === 0) {
            txn.executeSql(SQLITE_DELETE_TABLE, [])
            txn.executeSql(SQLITE_QUERY_CREATE_TABLE, [], () => {
              console.log("**** Database Table table_inventory Create *********")
            })
          }
        })
      })
    } catch (e) {}
  }
}

export const insertDataInventory = (data: any, successCb?: () => void, errorCb?: () => void) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_INSERT_TABLE, data, function (tx: Transaction, res: ResultSet) {
          if (res.rowsAffected > 0) {
            showAlert("Save Success!", "Your data saved Successfully", successCb)
          } else {
            showAlert("Save Error!", "Your data saved error, please try again!", errorCb)
          }
        })
      })
    } catch (e) {
      console.log({ e })
      showAlert("Save Error!", "Your data saved error, please try again!", errorCb)
    }
  }
}

export const selectAllDataInventory = (Cb?: (formatData: any) => void) => {
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

export const deleteDataInventory = (idCollection: string | number, successCb?: () => void, errorCb?: () => void) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_DELETE_COLLECTION, [idCollection], function (tx: Transaction, res: ResultSet) {
          if (res.rowsAffected > 0) {
            showAlert("Delete Success!", "Your data delete Successfully", successCb)
          } else {
            showAlert("Delete Error!", "Your data delete error or not found, please try again!", errorCb)
          }
        })
      })
    } catch (e) {
      showAlert("Delete Error!", "Your data delete error, please try again!")
    }
  }
}

export const updateDataInventory = (data: any, successCb?: () => void, errorCb?: () => void) => {
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

export const getDetailInventory = async (id: string, successCb?: (data: any) => void, errorCb?: () => void) => {
  if (db) {
    try {
      db.transaction(function (txn: SQLite.Transaction) {
        txn.executeSql(SQLITE_SELECT_TABLE, [id], function (tx: Transaction, res: ResultSet) {
          if (res.rows?.length > 0) {
            //get data table inventory
            successCb?.(res.rows.item(0))
          } else {
            successCb?.(null)
          }
        })
      })
    } catch (e) {
      errorCb?.()
    }
  }
}

export const insertListDataInventory = async (list: any) => {
  if (db) {
    try {
      await db.transaction(async (txn: SQLite.Transaction) => {
        for (let i = 0; i < list?.length; i++) {
          const item = list[i]
          if (item?.id) {
            txn.executeSql(
              `SELECT id FROM ${SQLITE_TABLE_NAME_INVENTORY} WHERE id=? LIMIT 1`,
              [item?.id],
              (tx: Transaction, res: ResultSet) => {
                if (res?.rows?.length === 0) {
                  //create if not exist
                  txn.executeSql(SQLITE_INSERT_TABLE, [item.id, item.text, item.version || "", item.buble || 0])
                }
              },
            )
          }
        }
      })
      console.log("****** Sync List Inventory SQlite Success ! *******")
      return true
    } catch (e) {
      showAlert("Save Error!", "Your data saved error, please try again!")
      return null
    }
  }
  return null
}
