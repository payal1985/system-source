import { ActionType, createAsyncAction } from "typesafe-actions"

//handle action logout
const LOGOUT_REQUEST = "LOGOUT_REQUEST"
const LOGOUT_SUCCESS = "LOGOUT_SUCCESS"
const LOGOUT_ERROR = "LOGOUT_ERROR"

export const logout = createAsyncAction(LOGOUT_REQUEST, LOGOUT_SUCCESS, LOGOUT_ERROR)<any, Error, undefined>()

export type LogoutAction = ActionType<typeof logout>
