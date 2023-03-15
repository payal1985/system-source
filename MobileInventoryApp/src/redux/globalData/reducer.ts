import { createReducer, PayloadAction } from "typesafe-actions"

import { ReduxState, ReduxStateType } from "../types"
import { globalDataActions, codePushActions } from "./action"
import { GlobalData } from "/models/GlobalData"

const INITIAL_STATE: ReduxState<GlobalData> = {
  state: ReduxStateType.INIT,
}

const globalDataReducer = createReducer(INITIAL_STATE)
  .handleAction(globalDataActions.request, (state: ReduxState<GlobalData>) => ({
    ...state,
    state: ReduxStateType.LOADING,
  }))
  .handleAction(
    globalDataActions.success,
    (state: ReduxState<GlobalData>, action: PayloadAction<"GET_ACCOUNT_ACTIVE_SUCCESS", GlobalData>) => ({
      ...action.payload,
    }),
  )
  .handleAction(
    globalDataActions.failure,
    (state: ReduxState<GlobalData>, action: PayloadAction<"GET_ACCOUNT_ACTIVE_ERROR", GlobalData>) => ({
      ...state,
      error: action.payload,
      state: ReduxStateType.ERROR,
    }),
  )
  .handleAction(codePushActions.request, (state: ReduxState<any>) => ({
    ...state,
  }))
  .handleAction(
    codePushActions.success,
    (state: ReduxState<any>, action: PayloadAction<"SET_CODE_PUSH_DATA_SUCCESS", any>) => ({
      ...state,
      codePushData: action.payload,
    }),
  )

export default globalDataReducer
