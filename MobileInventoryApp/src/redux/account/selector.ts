import { Account } from "./../../models/Account"

import { AppState } from "../index"
import { AppReducerType } from "../reducers"

export const accountSelector = (state: AppState): Account => state[AppReducerType.ACCOUNT] as Account
