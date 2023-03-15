import { combineReducers } from "redux"

import accountReducer from "./account/reducer"
import accountActiveReducer from "./accountActive/reducer"
import languageReducer from "./language/reducer"
import tokenReducer from "./login/reducer"
import userReducer from "./user/reducer"
import globalDataReducer from "./globalData/reducer"
import manufacturerReducer from "./manufacturer/reducer"

export enum AppReducerType {
  USER = "user",
  SYSTEM = "System",
  TOKEN = "token",
  ACCOUNT = "account",
  ACCOUNT_ACTIVE = "accountActive",
  LANGUAGE = "language",
  GLOBAL_DATA = "GlobalData",
  MANUFACTURER = "Manufacturer",
}

const reducers = combineReducers({
  [AppReducerType.USER]: userReducer,
  [AppReducerType.TOKEN]: tokenReducer,
  [AppReducerType.ACCOUNT]: accountReducer,
  [AppReducerType.ACCOUNT_ACTIVE]: accountActiveReducer,
  [AppReducerType.LANGUAGE]: languageReducer,
  [AppReducerType.GLOBAL_DATA]: globalDataReducer,
  [AppReducerType.MANUFACTURER]: manufacturerReducer,
})

export default reducers
