import { cart } from "./cart";

export class order{
    requestoremail:string;
    request_individual_project:string;
    destination_building:string;
    destination_floor:string;
    destination_location:string;
    requested_inst_date:string;
    comments:string;
    cart_item:Array<cart>;

}