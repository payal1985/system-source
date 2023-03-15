import { useDispatch, useSelector } from "react-redux"
import { Dispatch } from "redux"
import { log } from "../log"

import { LoginParams } from "/apis/auth/types"
import { UseToken } from "/models/Token"
import { AppAction } from "/redux"
import { login } from "/redux/login/actions"
import { tokenSelector } from "/redux/login/selectors"

const useLogin = (): [(params: LoginParams) => void, UseToken] => {
  const dispatch = useDispatch<Dispatch<AppAction>>()
  const tokenState = useSelector(tokenSelector)

  return [(params) => dispatch(login.request(params)), tokenState]
}

export default useLogin
