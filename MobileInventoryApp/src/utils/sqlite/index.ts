import { Alert } from "react-native"
import SQLite from "react-native-sqlite-storage"
SQLite.DEBUG(true)
SQLite.enablePromise(true)

export const SQLITE_NAME = "data.db"
export const SQLITE_PATH = "./data.db"
export const SQLITE_VERSION = "1.0"
let db: SQLite.SQLiteDatabase

export const showLog = (message: string) => {
  message && console.info(message)
}

export const showAlert = (message: string, des: string, cb?: () => void) => {
  Alert.alert(
    message,
    des,
    [
      {
        text: "Ok",
        onPress: () => {
          cb?.()
        },
      },
    ],
    { cancelable: false },
  )
}
export const closeSqlite = async (db: SQLite.SQLiteDatabase) => {
  if (db) {
    db.close().then(() => {
      console.log("Database CLOSED")
    })
  }
}
export const initSqlite = async () => {
  await closeSqlite(db)
  db = await SQLite.openDatabase({
    name: SQLITE_NAME,
    location: "default",
  })
  return db
}
