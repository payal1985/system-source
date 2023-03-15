import { ActionType, createAsyncAction } from "typesafe-actions"
import { Account } from "/models/Account"

const GET_ACCOUNT_ACTIVE_REQUEST = "GET_ACCOUNT_ACTIVE_REQUEST"
const GET_ACCOUNT_ACTIVE_SUCCESS = "GET_ACCOUNT_ACTIVE_SUCCESS"
const GET_ACCOUNT_ACTIVE_ERROR = "GET_ACCOUNT_ACTIVE_ERROR"

export const accountActiveAc = createAsyncAction(
  GET_ACCOUNT_ACTIVE_REQUEST,
  GET_ACCOUNT_ACTIVE_SUCCESS,
  GET_ACCOUNT_ACTIVE_ERROR,
)<Account, Account, Error>()

export type AccountActiveAction = ActionType<typeof accountActiveAc>
