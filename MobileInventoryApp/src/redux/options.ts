import AsyncStorage from "@react-native-community/async-storage"
import { ReduxLoggerOptions } from "redux-logger"
import { createTransform, PersistConfig } from "redux-persist"
import { SagaMiddlewareOptions } from "redux-saga"

import { AppState } from "./"
import { AppReducerType } from "./reducers"
import { isDataRedux, ReduxStateType } from "./types"

export const loggerOptions: ReduxLoggerOptions = {
  collapsed: true,
  duration: true,
  timestamp: true,
}

export const sagaOptions: SagaMiddlewareOptions = {}

export const persistOptions: PersistConfig<AppState> = {
  key: "root",
  version: 0,
  storage: AsyncStorage,
  whitelist: [AppReducerType.USER, AppReducerType.TOKEN],
  transforms: [
    createTransform(
      (states) => {
        if (isDataRedux(states)) {
          return { ...states, state: ReduxStateType.INIT, error: undefined }
        }

        return states
      },
      (states) => {
        return states
      },
    ),
  ],
}
