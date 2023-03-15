import { Role } from "./role";

export class users{
    userId       : number;   
    userName : string;
    password : string;
    firstName : string;
    lastName:string;
    email:string;
    clientId : number;
    clientName:string;
    userTypeId:number;
    userType:string;
    isAdmin : boolean;
    role: Role;

}