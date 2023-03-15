import { createReducer, PayloadAction } from "typesafe-actions"

import { ReduxState, ReduxStateType } from "../types"
import { accountActiveAc } from "./action"
import { Account } from "/models/Account"

const INITIAL_STATE: ReduxState<Account> = {
  state: ReduxStateType.INIT,
}

const accountActiveReducer = createReducer(INITIAL_STATE)
  .handleAction(accountActiveAc.request, (state: ReduxState<Account>) => ({
    ...state,
    state: ReduxStateType.LOADING,
  }))
  .handleAction(
    accountActiveAc.success,
    (state: ReduxState<Account>, action: PayloadAction<"GET_ACCOUNT_ACTIVE_SUCCESS", Account>) => ({
      ...action.payload,
    }),
  )
  .handleAction(
    accountActiveAc.failure,
    (state: ReduxState<Account>, action: PayloadAction<"GET_ACCOUNT_ACTIVE_ERROR", Account>) => ({
      ...state,
      error: action.payload,
      state: ReduxStateType.ERROR,
    }),
  )

export default accountActiveReducer
