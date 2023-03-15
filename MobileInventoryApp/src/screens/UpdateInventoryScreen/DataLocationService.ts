import { useEffect, useState } from 'react'
import { BaseAlert } from '/components'
import {
    getFloorsDataService,
    getBuildDataService,
    getItemTypeService,
} from "/redux/progress/service"
const DataLocationsService = (clientID: any) => {
    const [buildingData, setBuildingData] = useState([])
    const [floorData, setFloorData] = useState([])
    const [itemTypesData, setItemTypesData] = useState([])

    const handleGetBuildingDataService = () => {
        let url = `/api/InventoryBuildings/GetBuildings?clientId=${parseInt(clientID)}`
        getBuildDataService(
            url,
            (res) => {
                if (res.status == 200) {
                    setBuildingData(res.data)
                } else {
                    BaseAlert(res, "Get Building")
                }
            },
            (e) => {
                BaseAlert(e, "Get Building")
            },
        )
    }

    const handleGetFloorsDataService = () => {
        getFloorsDataService(
            (res) => {
                if (res.status == 200) {
                    setFloorData(res.data)
                } else {
                    BaseAlert(res, "Get Floors")
                }
            },
            (e) => {
                BaseAlert(e, "Get Floors")
            },
        )
    }

    const handleGetDataItemType = () => {
        let url = `/api/ItemType/GetItemTypes?clientId=${parseInt(clientID)}`
        getItemTypeService(
            url,
            (res) => {
                if (res.status == 200) {
                    setItemTypesData(res.data)
                } else {
                    BaseAlert(res, "Get DataItemType")
                }
            },
            (e) => {
                BaseAlert(e, "Get DataItemType")
            },
        )
    }

    useEffect(() => {
        handleGetBuildingDataService()
        handleGetFloorsDataService()
        handleGetDataItemType()
    }, [])
    return { buildingData, floorData, itemTypesData, handleGetBuildingDataService }
}

export default DataLocationsService