import { inventoryitem } from "./inventoryitem";

export class inventory{

    inventoryID:number;
    itemCode:string;
    description:string;
     manuf:string;
     height:string;
     width:string;
     depth:string;
     hwdStr:string;
    //  building:string;
    //  floor:string;
    //  mploc:string;
    //  cond:string;
    imageName:string;
    imageUrl:string;
     qty:number;
     clientID:number;
     category:string;
    //  inv_item_id:number;
     fabric:string;
     finish:string;
     tootipInventoryItem:string;
     inventoryItemModels :any;
     inventoryItemModelsDisplay:Array<inventoryitem>;     
  
     //         category: "Bench"
// clientID: 127
// depth: "-1.000"
// description: "Custom ply bench with metal legs,"
// fabric: " "
// finish: "Ply seat, Black metal legs,"
// height: "-1.000"
// hwdStr: ""
// imageName: "B2.jpg"
// imageUrl: "https//dev.systemsource.com/invadi/aimg/"
// inventoryID: 175

}