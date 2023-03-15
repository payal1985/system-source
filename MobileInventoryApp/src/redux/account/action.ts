import { ActionType, createAsyncAction } from "typesafe-actions"
import { Account } from "/models/Account"

const GET_ACCOUNT_REQUEST = "GET_ACCOUNT_REQUEST"
const GET_ACCOUNT_SUCCESS = "GET_ACCOUNT_SUCCESS"
const GET_ACCOUNT_ERROR = "GET_ACCOUNT_ERROR"

export const accountAc = createAsyncAction(GET_ACCOUNT_REQUEST, GET_ACCOUNT_SUCCESS, GET_ACCOUNT_ERROR)<
  Account,
  Account,
  Error
>()

export type AccountAction = ActionType<typeof accountAc>
