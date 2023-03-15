import React, { useEffect, useRef, useState } from "react"
import { View } from "react-native"
import ModalBox from "react-native-modalbox"
import { CameraAddMoreConditions } from "../../screens/CameraAddMoreConditions"
import { BaseBottomSheetCameraAddMoreConditionProps } from "./types"

const BaseBottomSheetCameraAddMoreCondition = ({
  open,
  onClosed,
  flex = 0.5,
  options,
  onSelect,
  type,
  arrCheckQuantityServer,
  conditionData,
}: BaseBottomSheetCameraAddMoreConditionProps) => {
  const [itemArr] = useState<any>(options)
  const [_, setarrItem] = useState<any>([])

  const testNEw = (item: object | undefined) => {
    onSelect(item)
  }

  useEffect(() => {
    setarrItem(itemArr)
  }, [])

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
        <CameraAddMoreConditions
          onClosed={onClosed}
          options={options}
          type={type}
          onSelect={(item: any) => testNEw(item)}
          arrCheckQuantityServer={arrCheckQuantityServer}
          conditionData={conditionData}
        />
      </View>
    </ModalBox>
  )
}

export default BaseBottomSheetCameraAddMoreCondition
