import { inventoryitem } from "./inventoryitem.model";



export class inventory{

    inventoryID:number;
    itemCode:string;
    description:string;
    manufacturerName:string;
    height:string;
    width:string;
    depth:string;
    hwdStr:string;
    imageName:string;
    imageUrl:string;
    qty:number;
    clientID:number;
    category:string;   
    fabric:string;
    finish:string;
    tootipInventoryItem:string;
    reservedQty:number;
    tootipReservedInventoryItem:string;
    warrantyYears:number;
    missingParts:boolean;
    inventoryItemModels :inventoryitem;
    inventoryItemModelsDisplay:Array<inventoryitem>;  
    bucketName:string;
    imagePath:string;
    filePath:string;
    imageBase64:any;
}