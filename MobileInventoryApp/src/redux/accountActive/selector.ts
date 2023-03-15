import { Account } from "../../models/Account"

import { AppState } from "../index"
import { AppReducerType } from "../reducers"

export const accountActiveSelector = (state: AppState): Account => state[AppReducerType.ACCOUNT_ACTIVE] as Account
