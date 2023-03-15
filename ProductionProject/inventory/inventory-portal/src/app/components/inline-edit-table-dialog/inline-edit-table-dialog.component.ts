import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTable, MatTableDataSource } from '@angular/material/table';
const user = [
  {"name": "John Smith", "occupation": "Advisor", "dateOfBirth": "1984-05-05", "age": 36,"select":"Active"},
  {"name": "Muhi Masri", "occupation": "Developer", "dateOfBirth": "1992-02-02", "age": 28,"select":"InActive"},
  {"name": "Peter Adams", "occupation": "HR", "dateOfBirth": "2000-01-01", "age": 20,"select":"Active"},
  {"name": "Lora Bay", "occupation": "Marketing", "dateOfBirth": "1977-03-03", "age": 43,"select":"InActive"},
];
const COLUMNS_SCHEMA = [
  {
      key: "name",
      type: "text",
      label: "Full Name"
  },
  {
      key: "occupation",
      type: "text",
      label: "Occupation"
  },
  {
      key: "dateOfBirth",
      type: "date",
      label: "Date of Birth"
  },
  {
      key: "age",
      type: "number",
      label: "Age"
  },
  {
    key:"select",
    type:"select",
    label:"Status"
  },
  {
    key: "isEdit",
    type: "isEdit",
    label: ""
  }
];

//[
  // {
  //   Id:1,
  //   Name: 'John Smith',
  //   IsActive: ['Active','InActive'],   
  //   IsEdit:false
  // },
  // {
  //   Id:2,
  //   Name: 'test Smith',
  //   IsActive: ['Active','InActive'],   
  // }
//];
@Component({
  selector: 'app-inline-edit-table-dialog',
  templateUrl: './inline-edit-table-dialog.component.html',
  styleUrls: ['./inline-edit-table-dialog.component.scss']
})


export class InlineEditTableDialogComponent implements OnInit {

//   editUsr: any;oldUsr: any;editdisabled: boolean;
//   displayedColumns: string[] = ['sno','Name', 'isAct', 'action'];
// dataSource:any;
// displayedColumns: string[] = COLUMNS_SCHEMA.map((col) => col.key);;
//   dataSource: any = user;
//   columnsSchema: any = COLUMNS_SCHEMA;

  constructor() { 
    // this.dataSource = user;
    // this.editUsr = user;
  }

  ngOnInit(): void {
  }

  @ViewChild('table') table:MatTable<any>
  editIndex:number=-1;
editAddCompanyDetailsTable : boolean[] = [];
newValue:any;
displayedColumnsCompanyDetails: string[] = ['sNo','companyName', 'status','actions'];
dataSourceCompanyDetails = new MatTableDataSource<companyDetails>(companydetails);

  editCompanyDetails(details,i) {
    this.editIndex=i;
    this.newValue={...details}
  }

  updateCompanyDetails(i){
    this.dataSourceCompanyDetails.data[i]={...this.newValue}
    this.editIndex=-1;
  }

  discardCompanyDetails(){
    this.editIndex=-1;
  }

//   editROw(usr: any){
//     console.log(usr)
//     this.editUsr = usr && usr.Id?usr:{};
//     this.oldUsr= {...this.editUsr};
//   }
//   updateEdit(){
//     //updateEdit
//     this.editdisabled = true;
//     // this.userServ.updateUser(this.editUsr)
//     //   .subscribe((data: any) => {
//     //     this.editUsr= {};
//     //     this.editdisabled = false;
//     //     if(data.Data && data.Status==1){
//     //       this.oldUsr= {};
//     //       this.toastr.success(data.Message, 'Success!');
//     //     }else{
//     //       this.cancelEdit();
//     //       this.toastr.error(data.Message, 'Error!');
//     //     }
//     //   }, err => {
//     //       this.toastr.error("Please try after some time", 'Error!');
//     //       this.editdisabled = false;
//     //       this.cancelEdit();
//     //   });
//   }
// cancelEdit(){
//     //cancel
//     this.editUsr= {};
//     if(this.oldUsr&& this.oldUsr.Id){
//       // this.dataListSubs = this.dataSource.usersData.pipe(
//       //   distinctUntilChanged()
//       // ).subscribe((data)=>{
//       //   if(data.length<=0){
//       //   }else{
//       //     let index = data.findIndex(item => item.Id === this.oldUsr.Id)
//       //     data.splice(index, 1, this.oldUsr)
//       //     this.dataSource.changeDataSource(data);
//       //   }
//       // })
//       // this.dataListSubs.unsubscribe();
//       // console.log(this.oldUsr, 'this.oldUsr', this.dataSource.usersData)
//     }
//   }
}

export interface companyDetails {
  companyName: string;
  status: string;
}

const companydetails: companyDetails[] = [
{companyName: 'tcs', status: 'active'},
{companyName: 'acs', status: 'active'},
];
