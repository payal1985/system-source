import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { SpinnerService } from 'src/app/features/spinner/spinner.service';
import { finalize} from 'rxjs/operators';

@Injectable()
export class SpinnerInterceptor implements HttpInterceptor{
    constructor(private spinnerService:SpinnerService ){

    }
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
//debugger;
        this.spinnerService.requestStarted();

        // return next.handle(req)
        // .pipe(
        //     finalize(() => { this.spinnerService.requestEnded();})
        // );

        return next.handle(req).pipe(            
                     finalize(() => {  
                         //debugger;                    
                         //setTimeout( () => this.spinnerService.requestEnded(), 5000 );
                         this.spinnerService.requestEnded();
                      }
                    )
                  )   
              
        //return this.handler(next,req);
    }
    // handler(next,req){
    //     debugger;
    //     return next.handler(req)
    //     .pipe(
    //         //tap(
    //             (event)=>{
    //                 debugger;
    //                 if(event instanceof HttpResponse){
    //                     this.spinnerService.requestEnded();
    //                 }
    //             },
    //             (error:HttpErrorResponse)=>{
    //                 debugger;
    //                 this.spinnerService.resetSpinner();
    //                 console.log(error.error);
    //                 throw error;
    //             }
    //         //)
    //     )
    // }
}
