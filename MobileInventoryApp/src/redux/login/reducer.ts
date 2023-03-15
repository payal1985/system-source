import { createReducer } from "typesafe-actions"

import { ReduxState, ReduxStateType } from "/redux/types"

import { login } from "./actions"
import { logout } from "../logout/actions"

const INITIAL_STATE: ReduxState<any> = {
  state: ReduxStateType.INIT,
}

const tokenReducer = createReducer(INITIAL_STATE)
  .handleAction(login.request, (state: any) => ({
    ...state,
    error: "",
    state: ReduxStateType.LOADING,
  }))
  .handleAction(login.success, (state: any, action: { payload: any }) => ({
    ...state,
    ...action.payload,
    state: ReduxStateType.LOADED,
  }))
  .handleAction(login.failure, (state: any, action: { payload: any }) => ({
    ...state,
    error: action.payload,
    state: ReduxStateType.LOADED,
  }))
  .handleAction(login.cancel, () => ({
    state: ReduxStateType.INIT,
  }))
  .handleAction(logout.success, () => ({
    ...INITIAL_STATE,
  }))

export default tokenReducer
