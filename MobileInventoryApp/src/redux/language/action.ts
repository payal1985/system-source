import { ActionType, createAsyncAction } from "typesafe-actions"

import { Language } from "/models/Language"

const GET_LANGUAGE_REQUEST = "GET_LANGUAGE_REQUEST"
const GET_LANGUAGE_SUCCESS = "GET_LANGUAGE_SUCCESS"
const GET_LANGUAGE_ERROR = "GET_LANGUAGE_ERROR"

export const languageAction = createAsyncAction(GET_LANGUAGE_REQUEST, GET_LANGUAGE_SUCCESS, GET_LANGUAGE_ERROR)<
  Language,
  Language,
  Error
>()

export type LanguageAction = ActionType<typeof languageAction>
