import { createReducer } from "typesafe-actions"

import { ReduxState, ReduxStateType } from "/redux/types"
import { manufacturer } from "./actions"

const INITIAL_STATE: ReduxState<any> = {
  state: ReduxStateType.INIT,
}

const manufacturerReducer = createReducer(INITIAL_STATE)
  .handleAction(manufacturer.request, (state: any) => ({
    ...state,
    error: "",
  }))
  .handleAction(manufacturer.success, (state: any, action: { payload: any }) => ({
    ...state,
    data: action.payload,
  }))
  .handleAction(manufacturer.failure, (state: any, action: { payload: any }) => ({
    ...state,
    ...action.payload,
  }))
export default manufacturerReducer
