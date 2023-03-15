import { ActionType, createAsyncAction } from "typesafe-actions"

const LOGIN_REQUEST = "LOGIN_REQUEST"
const LOGIN_SUCCESS = "LOGIN_SUCCESS"
const LOGIN_ERROR = "LOGIN_ERROR"
const LOGIN_CANCEL = "LOGIN_CANCEL"

export const login = createAsyncAction(LOGIN_REQUEST, LOGIN_SUCCESS, LOGIN_ERROR, LOGIN_CANCEL)<any, Error, undefined>()

export type TokenAction = ActionType<typeof login>
