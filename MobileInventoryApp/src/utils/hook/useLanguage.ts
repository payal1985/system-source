import { useDispatch, useSelector } from "react-redux"
import { Dispatch } from "redux"

import { languageAction } from "/redux/language/action"

import { Language } from "../../models/Language"
import { AppAction } from "../../redux/index"
import { languageSelector } from "../../redux/language/selector"

const useLanguage = (): [Language, (params: any) => void] => {
  const language = useSelector(languageSelector)
  const dispatch = useDispatch<Dispatch<AppAction>>()

  return [language, (data: any) => dispatch(languageAction.request(data))]
}

export default useLanguage
