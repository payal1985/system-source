import { AppState } from "/redux"
import { AppReducerType } from "/redux/reducers"

export const manufacturerSelector = (state: AppState): any => state[AppReducerType.MANUFACTURER]
