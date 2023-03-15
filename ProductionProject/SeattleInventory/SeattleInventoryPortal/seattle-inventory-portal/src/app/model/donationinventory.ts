import { inventoryitem } from "./inventoryitem";

export class donationinventory{

    inventory_id:number;
    item_code:string;
    description:string;
    additional_description:string;
     manuf:string;     
     qty:number;   
     fabric:string;
     finish:string;
     tootip_inv_item:string;
     inventoryItemModels :Array<inventoryitem>;
     inventoryItemModelsDisplay:Array<inventoryitem>;     
     image_url:Array<string>;
     image_name:string;
}