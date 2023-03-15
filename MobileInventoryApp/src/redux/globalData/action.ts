import { ActionType, createAsyncAction } from "typesafe-actions"
import { GlobalData } from "/models/GlobalData"

const GET_GLOBAL_DATA_REQUEST = "GET_GLOBAL_DATA_REQUEST"
const GET_GLOBAL_DATA_SUCCESS = "GET_GLOBAL_DATA_SUCCESS"
const GET_GLOBAL_DATA_ERROR = "GET_GLOBAL_DATA_ERROR"
const SET_CODE_PUSH_DATA = "SET_CODE_PUSH_DATA"
const SET_CODE_PUSH_DATA_SUCCESS = "SET_CODE_PUSH_DATA_SUCCESS"
const SET_CODE_PUSH_DATA_FAIL = "SET_CODE_PUSH_DATA_FAIL"

export const globalDataActions = createAsyncAction(
  GET_GLOBAL_DATA_REQUEST,
  GET_GLOBAL_DATA_SUCCESS,
  GET_GLOBAL_DATA_ERROR,
)<GlobalData, GlobalData, Error>()

export const codePushActions = createAsyncAction(
  SET_CODE_PUSH_DATA,
  SET_CODE_PUSH_DATA_SUCCESS,
  SET_CODE_PUSH_DATA_FAIL,
)<any, any, any>()

export type globalDataActions = ActionType<typeof globalDataActions>
export type codePushActions = ActionType<typeof codePushActions>
