import { createReducer, PayloadAction } from "typesafe-actions"

import { User } from "../../models/User"
import { ReduxState, ReduxStateType } from "../types"
import { getUser } from "./action"

const INITIAL_STATE: ReduxState<User> = {
  state: ReduxStateType.INIT,
}

const userReducer = createReducer(INITIAL_STATE)
  .handleAction(getUser.request, (state: ReduxState<User>) => ({
    ...state,
    state: ReduxStateType.LOADING,
  }))
  .handleAction(getUser.success, (state: ReduxState<User>, action: PayloadAction<"USER_SUCCESS", User>) => ({
    ...state,
    ...action.payload,
    state: ReduxStateType.LOADED,
  }))
  .handleAction(getUser.failure, (state: ReduxState<User>, action: PayloadAction<"USER_ERROR", User>) => ({
    ...state,
    error: action.payload,
    state: ReduxStateType.ERROR,
  }))

export default userReducer
