import { GlobalData } from "../../models/GlobalData"

import { AppState } from "../index"
import { AppReducerType } from "../reducers"

export const GlobalDataSelector = (state: AppState): GlobalData => state[AppReducerType.GLOBAL_DATA] as GlobalData
