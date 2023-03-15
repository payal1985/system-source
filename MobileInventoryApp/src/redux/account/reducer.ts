import { createReducer, PayloadAction } from "typesafe-actions"

import { ReduxState, ReduxStateType } from "../types"
import { accountAc } from "./action"
import { Account } from "/models/Account"

const INITIAL_STATE: ReduxState<Account> = {
  state: ReduxStateType.INIT,
}

const accountReducer = createReducer(INITIAL_STATE)
  .handleAction(accountAc.request, (state: ReduxState<Account>) => ({
    ...state,
    state: ReduxStateType.LOADING,
  }))
  .handleAction(
    accountAc.success,
    (state: ReduxState<Account>, action: PayloadAction<"GET_ACCOUNT_SUCCESS", Account>) => ({
      ...action.payload,
    }),
  )
  .handleAction(
    accountAc.failure,
    (state: ReduxState<Account>, action: PayloadAction<"GET_ACCOUNT_ERROR", Account>) => ({
      ...state,
      error: action.payload,
      state: ReduxStateType.ERROR,
    }),
  )

export default accountReducer
