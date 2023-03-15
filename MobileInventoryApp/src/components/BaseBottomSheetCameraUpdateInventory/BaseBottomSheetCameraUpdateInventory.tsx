import React, { useEffect, useState } from "react"
import { View } from "react-native"
import ModalBox from "react-native-modalbox"
import { CameraConditions } from "../../screens/CameraConditions"
import { BaseBottomSheetCameraProps } from "./types"
import CameraUpdateInventory from "/screens/CameraUpdateInventory/CameraUpdateInventory";

const BaseBottomSheetCameraUpdateInventory = ({ title, conditionID, inventoryItemID, InventoryID, open, onClosed, flex = 0.5, options, onSelect, type, conditionRender, parentRowID, arrCheckQuatity, idxAddItemPhoto }: BaseBottomSheetCameraProps) => {
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
      {
        console.log("InventoryID2222", InventoryID)
      }
      <View style={{ flex: 1 }}>
        <CameraUpdateInventory title={title} conditionID={conditionID} inventoryItemID={inventoryItemID} InventoryID={InventoryID} arrCheckQuatity={arrCheckQuatity} idxAddItemPhoto={idxAddItemPhoto} onClosed={onClosed} parentRowID={parentRowID} conditionRender={conditionRender} type={type} onSelect={(item: object | undefined) => testNEw(item)} />
      </View>
    </ModalBox>
  )
}

export default BaseBottomSheetCameraUpdateInventory
