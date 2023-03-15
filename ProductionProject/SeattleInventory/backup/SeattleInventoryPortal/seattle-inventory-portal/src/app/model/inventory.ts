import { inventoryitem } from "./inventoryitem";

export class inventory{

    inventory_id:number;
    item_code:string;
    description:string;
     manuf:string;
     height:string;
     width:string;
     depth:string;
     hwd_str:string;
    //  building:string;
    //  floor:string;
    //  mploc:string;
    //  cond:string;
     image_name:string;
    //  image_url:string;
     qty:number;
    //  inv_item_id:number;
     fabric:string;
     finish:string;
     tootip_inv_item:string;
     inventoryItemModels :any;
     inventoryItemModelsDisplay:Array<inventoryitem>;     
  
}