import { User } from "../../models/User"
import { AppState } from "../index"
import { AppReducerType } from "../reducers"

export const userSelector = (state: AppState): User => state[AppReducerType.USER] as User
