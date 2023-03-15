import { ActionType, createAsyncAction } from "typesafe-actions"

import { User } from "../../models/User"

const USER_REQUEST = "USER_REQUEST"
const USER_SUCCESS = "USER_SUCCESS"
const USER_ERROR = "USER_ERROR"

export const getUser = createAsyncAction(USER_REQUEST, USER_SUCCESS, USER_ERROR)<undefined, User, Error>()

export type UserAction = ActionType<typeof getUser>
