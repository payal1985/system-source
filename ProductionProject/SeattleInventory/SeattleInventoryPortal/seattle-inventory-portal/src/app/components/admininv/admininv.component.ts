import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { client } from 'src/app/model/client';
import { AdminService } from 'src/app/services/admininv.service';
import { LoginService } from 'src/app/services/login.service';
import { AddClientDialogComponent } from './add-client-dialog/add-client-dialog.component';

@Component({
  selector: 'app-admininv',
  templateUrl: './admininv.component.html',
  styleUrls: ['./admininv.component.css']
})
export class AdmininvComponent implements OnInit {

 // dataSource: client[];
  
  @ViewChild(MatPaginator) paginator: MatPaginator;
  // @ViewChild(MatTable,{static:true}) table: MatTable<client>;

  displayedColumns: string[] = ['select', 'client name'];
  dataSource = new MatTableDataSource<client>();
  selection = new SelectionModel<client>(true, []);

  //checked: boolean = false;

  constructor(private adminService: AdminService,public dialog: MatDialog,private loginService: LoginService,private router:Router) {

    this.adminService.getClients().subscribe(data => {
      this.dataSource = new MatTableDataSource(data);
      this.dataSource.paginator = this.paginator;
    });

   }

  ngOnInit(): void {
    //this.dataSource.paginator = this.paginator;
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  
  //  /** Whether the number of selected elements matches the total number of rows. */
  // isAllSelected() {
  //   //debugger;
  //   const numSelected = this.selection.selected.length;
  //   const numRows = this.dataSource.data.length;
  //   return numSelected === numRows;
  // }
  //  isSelectedPage() {
  //    debugger;
  //   const numSelected = this.selection.selected.length;
  //   const page = this.dataSource.paginator.pageSize;
  //   let endIndex: number;
	// // First check whether data source length is greater than current page index multiply by page size.
	// // If yes then endIdex will be current page index multiply by page size.
	// // If not then select the remaining elements in current page only.
  //   if (this.dataSource.data.length > (this.dataSource.paginator.pageIndex + 1) * this.dataSource.paginator.pageSize) {
  //     endIndex = (this.dataSource.paginator.pageIndex + 1) * this.dataSource.paginator.pageSize;
  //   } else {
  //     // tslint:disable-next-line:max-line-length
  //     endIndex = this.dataSource.data.length - (this.dataSource.paginator.pageIndex * this.dataSource.paginator.pageSize);
  //   }
  //   console.log(endIndex);
  //   return numSelected === endIndex;
  // }

  //   /** Selects all rows if they are not all selected; otherwise clear selection. */
  //   masterToggle() {
  //     debugger;
  //     this.isAllSelected() ?
  //         this.selection.clear() :
  //         this.dataSource.data.forEach(row => this.selection.select(row));
  //   }
  //    selectRows() {
  //      debugger;
  //     // tslint:disable-next-line:max-line-length
  //     let endIndex: number;
  //     // tslint:disable-next-line:max-line-length
  //     if (this.dataSource.data.length > (this.dataSource.paginator.pageIndex + 1) * this.dataSource.paginator.pageSize) {
  //       endIndex = (this.dataSource.paginator.pageIndex + 1) * this.dataSource.paginator.pageSize;
  //     } else {
  //       // tslint:disable-next-line:max-line-length
  //       endIndex = this.dataSource.data.length;
  //     }
    
  //     for (let index = (this.dataSource.paginator.pageIndex * this.dataSource.paginator.pageSize); index < endIndex; index++) {
  //       this.selection.select(this.dataSource.data[index]);
  //     }
  //   }

    onChange(event,item) {
     // debugger;
      item.has_inventory = event.checked;
      this.adminService.updateClientHasInventory(item).subscribe(
        response => { 
          if(response)
          {
            alert("hasInventory Updated....")           
            //this.router.navigate([this.router.url]);
          }
          console.log(response); 
        },
        error => {      
          //debugger;     
          alert(error.message + "\n Unable to update client hasInventory");          
          console.log(error); 
        }); 

      // can't event.preventDefault();
     // console.log('onChange event.checked '+event.checked);
    }

    // openDialog(action,obj) {
    //   obj.action = action;
    //   const dialogRef = this.dialog.open(AddClientDialogComponent, {
    //     width: '250px',
    //     data:obj
    //   });
  
    //   dialogRef.afterClosed().subscribe(result => {
    //     if(result.event == 'Add'){
    //       debugger;
    //       this.addRowData(result.data);
    //     }
    //   });
    // }

    // addRowData(row_obj){
    //   debugger;
    //   //var d = new Date();
    
    //   var clientitem = new client();
    //   // clientitem.ssidb_client_id = 0;
    //   // clientitem.teamdesign_cust_no = 0;
    //   clientitem.client_name = row_obj.name;
    //   clientitem.has_inventory = false;



    //   this.adminService.SaveClientInfo(clientitem).subscribe(
    //     response => { 
    //       if(response)
    //       {
    //         debugger;
    //         alert("Client Added Successfully")
    //         this.dataSource.data.push(clientitem);
       
    //       }
    //       console.log(response); 
    //     },
    //     error => {      
    //       debugger;     
    //       alert(error.message + "\n Unable to create client");
    //     //  this.clicked=false;
    //       console.log(error); 
    //     });   
  
      
    // }

    logout(){
      this.loginService.logout();
      this.router.navigateByUrl('login');
    }
  
}
