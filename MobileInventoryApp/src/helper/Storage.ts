import AsyncStorage from "@react-native-community/async-storage"
import { log } from "/utils/log"

class Storage {
  static get(key: string, defaultValue: string = "") {
    try {
      const value = AsyncStorage.getItem(key)

      return value || defaultValue
    } catch (err) {
      return defaultValue
    }
  }

  static set(key: string, value: string) {
    try {
      return AsyncStorage.setItem(key, value)
    } catch (err) {
      throw err
    }
  }

  static remove(key: string) {
    return AsyncStorage.removeItem(key)
  }

  static async clearAll() {
    try {
      await AsyncStorage.clear()
      log("clearAll Success!")
    } catch (e) {
      log("clearAllError:", e)
    }
  }
}

export default Storage
