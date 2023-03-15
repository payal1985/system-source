import { Payload } from "../feature/types"

class PayloadManager {
  static _instance: any

  payload: Payload = {}

  static getInstance() {
    if (PayloadManager._instance == null) {
      PayloadManager._instance = new PayloadManager()
    }

    return this._instance
  }

  getPayload(): Payload {
    return this.payload
  }

  setPayload(pay: Payload) {
    this.payload = pay
  }
}

export default PayloadManager
