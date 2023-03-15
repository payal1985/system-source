import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialog,MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Validators } from '@angular/forms';
import { FormBuilder, FormControl, FormGroup,FormGroupDirective, NgForm, } from '@angular/forms';
//import {ErrorStateMatcher} from '@angular/material/core';
import { InventoryItemService } from 'src/app/services/inventoryitem.service';
import { order } from 'src/app/model/order';
import { InventoryorderService } from 'src/app/services/inventoryorder.service';
import { CartService } from 'src/app/services/cart.service';
// import {MomentDateAdapter} from '@angular/material-moment-adapter';
// import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
// import * as _moment from 'moment';
 import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
// const moment = _moment;

// export const MY_FORMATS = {
//   parse: {
//     dateInput: 'LL',
//   },
//   display: {
//     dateInput: 'MM-DD-YYYY',
//     monthYearLabel: 'YYYY',
//     dateA11yLabel: 'LL',
//     monthYearA11yLabel: 'YYYY',
//   },
// };

// import { threadId } from 'worker_threads';
//import {MatDatepickerInputEvent, MatDatepickerInput} from '@angular/material/datepicker';

/** Error when invalid control is dirty, touched, or submitted. */
// export class MyErrorStateMatcher implements ErrorStateMatcher {
//   isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
//     const isSubmitted = form && form.submitted;
//     return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
//   }

// }


@Component({
  selector: 'app-sendorder-dialog',
  templateUrl: './sendorder-dialog.component.html',
  styleUrls: ['./sendorder-dialog.component.css'],
  providers: [
   // {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]}

    //{provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  ],
})
export class SendorderDialogComponent implements OnInit {

  // @Input() matDatepicker: boolean;
  inventoryBuldinglist!: string[];
  inventoryFloorlist!: string[];
  cartData:any;
 // orderdetail:order[];
  orderdetail:order[];
  firstError :string;
  date:Date;
  // emailFormControl = new FormControl('', [
  //   Validators.required,
  //   Validators.email,
  // ]);

  // addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
  //   //this.events.push(`${type}: ${event.value}`);
  // }

  //req_inst_date = new FormControl(moment(new Date()));

  createOrderForm = this.fb.group({
    emailFormControl: ['', [Validators.required,Validators.email]],
    requestorFormControl:['',[Validators.required]],
    dest_buildingFormControl:['',[Validators.required]],
    dest_floorFormControl:['',[Validators.required]],
    dest_locFormControl:['',[Validators.required]],
    req_inst_date :['',[Validators.required]],
    commentFormControl:['']
  });
  //matcher = new MyErrorStateMatcher();
  constructor(public dialogRef: MatDialogRef<SendorderDialogComponent>
    ,@Inject(MAT_DIALOG_DATA) public data: any,
   private fb: FormBuilder
    ,private inventoryItemService: InventoryItemService
    ,private inventoryOrderService: InventoryorderService
    ,private cartService: CartService
    ,private datePipe: DatePipe
    ,private router: Router, private activatedRoute: ActivatedRoute
    ) { 

    this.inventoryItemService.getBuildings().subscribe(data => {
      this.inventoryBuldinglist = data;
    });
    this.inventoryItemService.getFloor().subscribe(data => {
      this.inventoryFloorlist = data;
    });

   
  }

  ngOnInit(): void {
this.cartData = this.data;
  }

  save(){
    debugger;
    //console.log(this.createOrderForm.getRawValue());
    console.log(this.createOrderForm.value);
    //console.log(this.cartData);
this.date = this.createOrderForm.value.req_inst_date;
    console.log(this.datePipe.transform(this.date, "yyyy-dd-MM"));

    // var orderdetail = new order();
    // orderdetail = this.createOrderForm.value;
    // orderdetail.cart_item = this.cartData;

    // console.log(orderdetail);
    // console.log(orderdetail.cart_item);

    
    debugger;
    //this.date = new Date(this.createOrderForm.value.req_inst_date);
   console.log(this.datePipe.transform(this.createOrderForm.value.req_inst_date, "yyyy-MM-dd HH:mm:ss"));
   
    var orderitem = new order();
    orderitem.requestoremail = this.createOrderForm.value.emailFormControl,
    orderitem.request_individual_project = this.createOrderForm.value.requestorFormControl,
    orderitem.destination_building = this.createOrderForm.value.dest_buildingFormControl,
    orderitem.destination_floor = this.createOrderForm.value.dest_floorFormControl,
    orderitem.destination_location = this.createOrderForm.value.dest_locFormControl,
    //orderitem.requested_inst_date = this.myDatepipe.transform(this.createOrderForm.value.req_inst_date, 'yyyy-MM-dd'),
    orderitem.requested_inst_date = this.datePipe.transform(this.createOrderForm.value.req_inst_date, "yyyy-MM-dd HH:mm:ss"),
    //this.datePipe.transform(this.myFormControl.value,'yyyy-MM-dd');
    //orderitem.requested_inst_date = this.createOrderForm.value.req_inst_date,
    orderitem.comments = this.createOrderForm.value.commentFormControl,
    orderitem.cart_item = this.cartData

    //  var orderdetail = new order();
    //  orderdetail = orderitem;


    this.inventoryOrderService.SaveOrderInfo(orderitem).subscribe(
        response => { 
          if(response)
          {
            alert("Thank you for your order.")
            this.clearCart();
            this.dialogRef.close();
            window.location.reload();
            //this.router.navigate([this.router.url]);
          }
          console.log(response); 
        },
        error => { 
          
          alert(error + " Unable to create order");
          console.log(error); 
        });   
  }

  clearCart() {
    // this.items.forEach((item, index) => this.cartService.removeItem(index));
    this.cartService.clearCart();
    //this.items = [...this.cartService.getItems()];
  }

}
