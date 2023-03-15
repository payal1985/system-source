import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, UrlSegment } from '@angular/router';
import { take } from 'rxjs/operators';
import { users } from 'src/app/model/users';
import { LoginService } from 'src/app/services/login.service';

// const EMAIL_REGEX =  /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm:FormGroup;
  //user= new users();
  //public matched;
  public loginValid=true;
  user: users[];
  
  constructor(private fb:FormBuilder, private router:Router, private loginService:LoginService) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      // email: ['', Validators.compose([Validators.required, Validators.pattern(EMAIL_REGEX)])],
      uname:['', Validators.compose([Validators.required])],
      password: ['', Validators.compose([Validators.required])]
      //remember: [true]
    })
  }

  login(){
    //debugger;
    // this.loginService.validateLogin(this.loginForm.controls.uname.value, this.loginForm.controls.password.value).subscribe(
    // usr=>{
    //   //debugger;
    //   this.user = usr;

    // if(this.user)
    // {
    //   debugger;
    //   this.loginValid = this.loginService.setUserFlag(this.user);
    // }

    // if(this.loginValid)
    // {
    //   debugger;
    //   this.router.navigateByUrl('index');
    // }
    // else {
    //   debugger;
    //    // alert("Enter valid Details");
    //    this.loginValid = false;
    // }
    // });

    this.loginService.validateLogin(this.loginForm.controls.uname.value, this.loginForm.controls.password.value).
    subscribe({
      next: _ => {
       // debugger;
        //this.loginValid = _ != null ? this.loginService.setUserFlag(_) : false;
        this.loginValid = _ != null ? true : false;
        if(this.loginValid)
          this.router.navigateByUrl('index');
      },
      error: _ => this.loginValid = false
    });
    
  }

//   login(){
//     debugger;
// //     this.loginService.getUsers(this.loginForm.controls.uname.value, this.loginForm.controls.password.value)
// //                      .subscribe(data=>{
// //                        debugger;
// //       console.log('Log in component data=>',data);
// //      // this.user.push(data);
// //       this.user = data;

// //       this.loginService.login(this.user)
// //       .subscribe(
// //           matched => {
// //             if (matched) {
// //               this.router.navigateByUrl('index');
// //              // this.router.navigateByUrl('admin');
// //             } else {
// //               alert("Enter valid Details");
// //             }
// //           });

// // //       clientId: 30
// // // isAdmin: false
// // // password: "test123"
// // // userId: 1
// // // userName: "test"
// // //       this.user.user_id = data.userId;
// // //         this.user.username = data.username;
// // //         this.user.client_id = data.client_id;
// // //         this.user.password = data.password;
// // //         this.user.isadmin = data.isadmin;
// //     });

// this.matched = this.loginService.login(this.loginForm.controls.uname.value, this.loginForm.controls.password.value)
//             .then(x=> {return x});
//             if (this.matched) {
//             debugger;
//             setTimeout(() => {
//               this.router.navigateByUrl('index');
//             }, 5000)
//                         //this.router.navigateByUrl('index');
//                        // this.router.navigateByUrl('admin');
//                       } else {
//             debugger;

//                         alert("Enter valid Details");
//               this.router.navigateByUrl('');

//                       }

//   //  await this.loginService.login(this.loginForm.controls.uname.value, this.loginForm.controls.password.value)
//   //   //this.loginService.login(this.user)
//   //   .subscribe(
//   //       matched => {
//   //         if (matched) {
//   //           this.router.navigateByUrl('index');
//   //          // this.router.navigateByUrl('admin');
//   //         } else {
//   //           alert("Enter valid Details");
//   //         }
//   //       });
//   }
 }
