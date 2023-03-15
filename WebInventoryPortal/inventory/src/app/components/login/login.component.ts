import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { users } from 'src/app/models/users';
import { LoginService } from 'src/app/services/login.service';

// const EMAIL_REGEX =  /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm:FormGroup;
  // user= new users();
  // public matched;
  //user: users[];
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

 get uname() { return this.loginForm.get('uname'); }
  get password() { return this.loginForm.get('password'); }

  login(){
    //debugger;

    this.loginService.validateLogin(this.loginForm.value.uname, this.loginForm.value.password).
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

//this.matched = this.loginService.login(this.loginForm.controls.uname.value, this.loginForm.controls.password.value)
// this.matched = this.loginService.login(this.loginForm.value.uname, this.loginForm.value.password)
//             .then(x=> {return x});
//             if (this.matched) {
//             //debugger;
//             setTimeout(() => {
//               this.router.navigateByUrl('index');
//             }, 3000)
//                         //this.router.navigateByUrl('index');
//                        // this.router.navigateByUrl('admin');
//                       } else {
//             //debugger;

//                         alert("Enter valid Details");
//                       }
  //  await this.loginService.login(this.loginForm.controls.uname.value, this.loginForm.controls.password.value)
  //   //this.loginService.login(this.user)
  //   .subscribe(
  //       matched => {
  //         if (matched) {
  //           this.router.navigateByUrl('index');
  //          // this.router.navigateByUrl('admin');
  //         } else {
  //           alert("Enter valid Details");
  //         }
  //       });
  }
}
