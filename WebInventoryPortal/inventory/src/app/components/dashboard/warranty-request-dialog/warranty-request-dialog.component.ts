import { Component, OnInit,Inject,ViewChild,AfterViewInit, ChangeDetectorRef  } from '@angular/core';
import { MatDialog,MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {FormControl, Validators} from '@angular/forms';
import { addcart } from 'src/app/models/addcart';
import { AddCartService } from 'src/app/services/addcart.service';
import { ImageCarouselDialogComponent } from '../image-carousel-dialog/image-carousel-dialog.component';
import {  Observable } from 'rxjs';
import { LoginService } from 'src/app/services/login.service';
import { users } from 'src/app/models/users';
import { ClientService } from 'src/app/services/client.service';
import { client } from 'src/app/models/client';
import { InventoryItemWarrantyService } from 'src/app/services/inventoryitemwarranty.service';


@Component({
  selector: 'app-warranty-request-dialog',
  templateUrl: './warranty-request-dialog.component.html',
  styleUrls: ['./warranty-request-dialog.component.scss']
})
export class WarrantyRequestDialogComponent implements OnInit {

 

 // warrantyDataSource:cart[]; //old flow code support line
 //warrantyDataSource:ordercart[];
 warrantyDataSource:addcart[];
 inventoryBuldinglist!: string[];
 inventoryFloorlist!: string[];
 depcostCenterlist:string[];
 firstError :string;
 masterSelected:boolean;
 addDialogResult: boolean;
//  ordCart: ordercart[];
selectedDepCostCenter:Observable<string[]>;
currentUser: users;
currentClient: client;

  //items = [];
 // emptyData = new MatTableDataSource([{ empty: "row" }]);
  displayedColumns: string[] = ['inv_image_name','item_code', 'description', 'pullinfo','cond','pullqty','comment','remove'];
  // displayedColumns: string[] = ['inv_image_name','item_code', 'description', 'pullinfo','cond','pullqty','remove','email','requestor','dest_bldg','dest_flr','dest_loc','req_inst_date','comment'];
  // displayedColumns: string[] = ['inv_image_name','item_code', 'description', 'pullinfo','cond','pullqty','remove'];
  //displayedColumns: string[] = ['building', 'floor', 'mploc', 'qty','star'];
  qtyFormControl = new FormControl('', []);
  // @ViewChild(MatTable, {static: false}) table: MatTable<any>;
  clicked = false;
  //checkedList:addcart[];

  emailFormControl = new FormControl('', [Validators.required,Validators.email]);
 requestorFormControl = new FormControl('', [Validators.required]);
//dest_buildingFormControl = new FormControl('', [Validators.required]);
//dest_floorFormControl = new FormControl('', [Validators.required]);
//dest_locFormControl = new FormControl('', [Validators.required]);
//req_inst_dateFormControl  = new FormControl('', [Validators.required]);
commentFormControl = new FormControl('');
//dest_depcostFormControl = new FormControl('',[Validators.required]);
// matcher = new MyErrorStateMatcher();


  constructor(public dialogRef: MatDialogRef<WarrantyRequestDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    //,private cartService: OrderCartService
    ,private cartService: AddCartService
    ,public dialog: MatDialog
    ,private cdr: ChangeDetectorRef
    ,private inventoryItemWarrantyService: InventoryItemWarrantyService
    //,private inventoryItemService: InventoryItemService
   // ,private datePipe: DatePipe
   ,private loginService: LoginService
   ,private clientService: ClientService
    
    ) { 
      debugger;
      console.log('cart data', this.data);
      this.loginService.currentUser.subscribe(x => this.currentUser = x);      
      this.clientService.currentClient.subscribe(x => this.currentClient = x);      
      this.emailFormControl.setValue(this.currentUser.email);
      this.requestorFormControl.setValue(this.currentUser.firstName + ' ' + this.currentUser.lastName);
    }

  ngOnInit(): void {
//debugger;

this.warrantyDataSource = this.data;
this.warrantyDataSource.forEach(x =>  {
 // x.isSelected = false,
  x.clientName = this.currentClient.clientName
  
  //x.arrayBuildings=this.inventoryBuldinglist;
});



//debugger;
    console.log('cart data Source data', this.warrantyDataSource);     
  }

  ngAfterViewInit() {
    //debugger;
    this.cdr.detectChanges();
  }



  pullqtychange($event:any,index:number){
    //debugger;
    if($event.target.value > this.warrantyDataSource[index].qty)
    {
      alert("There are not enough items availalbe");
      this.warrantyDataSource[index].pullQty = this.warrantyDataSource[index].qty;
    }
    else
    {
      this.warrantyDataSource[index].pullQty = $event.target.value;      
    }
   // console.log('new item',item);
  }

  //removefromcart()  {
  removeFromCart(item:any,i:any)  {
    //debugger;
    //this.cartService.removeCartItem(item);
    this.cartService.removeItem(item,i);
    //this.warrantyDataSource = this.cartService.getItems();
    this.warrantyDataSource = this.warrantyDataSource
      .filter(i => i !== item)
      .map((i, idx) => (i.inventoryItemID = (idx + 1), i));
   // this.items = this.cartService.getItems();
//debugger;

  }

  SendRequest(){
  debugger;
    if(!this.emailFormControl.value)
     this.emailFormControl.setValue(this.data.email);

     if(!this.requestorFormControl.value)
     this.requestorFormControl.setValue(this.currentUser.firstName + ' ' + this.currentUser.lastName);

    if(this.emailFormControl.value && this.requestorFormControl.value)
    {
      if(this.warrantyDataSource)
      {
        this.warrantyDataSource.forEach(x =>  {
              x.email = this.emailFormControl.value,
              x.reqName = this.requestorFormControl.value
          });
          this.inventoryItemWarrantyService.SaveWarrantyRequest(this.warrantyDataSource).subscribe(
            response => { 
              if(response)
              {
                debugger;
                alert("Thank you for your request.")
                
                this.dialogRef.close();
                window.location.reload();
                //this.router.navigate([this.router.url]);
              }
              console.log(response); 
            },
            error => {      
              //debugger;     
              alert(error.message + "\n Unable to create warranty request");
              this.clicked=false;
              console.log(error); 
            });

      }
    }
  }

  

  openDialog(inv_item_id:number,condition:string) {
   
    this.dialog.open(ImageCarouselDialogComponent, {    
      width: '100%',
      height: '100%',   
      //panelClass: 'image-dialog',
      data: {id:inv_item_id,cond:condition}
    });
  }
 
  changedComment($event:any,i:number){
    //debugger; 
    this.warrantyDataSource[i].comment = $event.target.value;
  }


}
