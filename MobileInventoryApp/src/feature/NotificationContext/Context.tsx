import React from "react"

export interface ModalContextType {
  setShowPopup: (value: any) => void

  setData: (value: any) => void
  showPopup: boolean
}

export const ContextModal = React.createContext<ModalContextType>({
  setShowPopup: () => {},
  setData: () => {},
  showPopup: false,
})
