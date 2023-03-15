import { createReducer, PayloadAction } from "typesafe-actions"

import { Language } from "../../models/Language"
import { ReduxState, ReduxStateType } from "../types"
import { languageAction } from "./action"

const INITIAL_STATE: ReduxState<Language> = {
  state: ReduxStateType.INIT,
}

const languageReducer = createReducer(INITIAL_STATE)
  .handleAction(languageAction.request, (state: ReduxState<Language>) => ({
    ...state,
    state: ReduxStateType.LOADING,
  }))
  .handleAction(
    languageAction.success,
    (state: ReduxState<Language>, action: PayloadAction<"GET_LANGUAGE_SUCCESS", Language>) => ({
      ...action.payload,
    }),
  )
  .handleAction(
    languageAction.failure,
    (state: ReduxState<Language>, action: PayloadAction<"GET_LANGUAGE_ERROR", Language>) => ({
      ...state,
      error: action.payload,
      state: ReduxStateType.ERROR,
    }),
  )

export default languageReducer
