import { StateType } from "typesafe-actions"

import { AccountAction } from "./account/action"
import { AccountActiveAction } from "./accountActive/action"
import { LanguageAction } from "./language/action"
import { TokenAction } from "./login/actions"
import reducers from "./reducers"
import { UserAction } from "./user/action"
import { globalDataActions } from "./globalData/action"

export type AppState = StateType<typeof reducers>
export type AppAction = UserAction | TokenAction | AccountAction | AccountActiveAction | LanguageAction | globalDataActions
