import { Language } from "../../models/Language"
import { AppState } from "../index"
import { AppReducerType } from "../reducers"

export const languageSelector = (state: AppState): Language => state[AppReducerType.LANGUAGE] as Language
