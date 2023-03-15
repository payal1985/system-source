import { AppState } from "/redux"
import { AppReducerType } from "/redux/reducers"

export const tokenSelector = (state: AppState): any => state[AppReducerType.TOKEN]
