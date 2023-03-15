import React, { useEffect, useState } from "react"
import { View } from "react-native"
import ModalBox from "react-native-modalbox"
import { CameraConditions } from "../../screens/CameraConditions"
import { BaseBottomSheetCameraProps } from "./types"

const BaseBottomSheetCamera = ({ open, onClosed, flex = 0.5, options, onSelect, type, conditionRender, parentRowID, arrCheckQuantityServer, conditionData }: BaseBottomSheetCameraProps) => {
  const [itemArr] = useState<any>(options)
  const [_, setarrItem] = useState<any>([])

  useEffect(() => {
    setarrItem(itemArr)
  }, [])

  const testNEw = (item: object | undefined) => {
    onSelect && onSelect(item)
  }
  return (
    <ModalBox
      isOpen={open}
      entry={"bottom"}
      position={"bottom"}
      swipeToClose={false}
      onClosed={onClosed}
      style={{
        backgroundColor: "#fff",
        flex: flex,
        borderTopLeftRadius: 30,
        borderTopRightRadius: 30,
      }}
    >
      <View style={{ flex: 1 }}>
        <CameraConditions arrCheckQuantityServer={arrCheckQuantityServer} conditionData={conditionData} onClosed={onClosed} parentRowID={parentRowID} conditionRender={conditionRender} type={type} onSelect={(item: object | undefined) => testNEw(item)} />
      </View>
    </ModalBox>
  )
}

export default BaseBottomSheetCamera
