import { useDispatch, useSelector } from "react-redux"
import { Dispatch } from "redux"

import { AppAction } from "../../redux/index"
import { GlobalData } from "/models/GlobalData"
import { globalDataActions } from "/redux/globalData/action"

import { GlobalDataSelector } from "/redux/globalData/selector"

const useGlobalData = (): [GlobalData, (params: any) => void] => {
  const globalData = useSelector(GlobalDataSelector)

  const dispatch = useDispatch<Dispatch<AppAction>>()

  return [globalData, (data: any) => dispatch(globalDataActions.request(data))]
}

export default useGlobalData
