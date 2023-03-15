import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

//cdk
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { MatSpinner } from '@angular/material/progress-spinner';
import { SpinnerComponent } from './spinner.component';



@Injectable({
  providedIn: 'root'
})
export class SpinnerService {

  private count = 0;
  private spinner$ = new BehaviorSubject<boolean>(false);

  private spinnerTopRef = this.cdkSpinnerCreate();

 
  constructor(private overlay: Overlay) { }

  private cdkSpinnerCreate() {
    return this.overlay.create({
        hasBackdrop: true,
        backdropClass: 'dark-backdrop',
        positionStrategy: this.overlay.position()
            .global()
            .centerHorizontally()
            .centerVertically()
    })
}

  getSpinnerObserver(): Observable<boolean> {
    return this.spinner$.asObservable();
  }

  requestStarted() {
    if (++this.count === 1) {
      this.spinner$.next(true);
      this.spinnerTopRef.attach(new ComponentPortal(SpinnerComponent))
    }
  }

  requestEnded() {
    if (this.count === 0 || --this.count === 0) {
      this.spinner$.next(false);
      this.spinnerTopRef.detach() ;
    }
  }

  resetSpinner() {
    this.count = 0;
    this.spinner$.next(false);
    this.spinnerTopRef.detach() ;
  }

 
}
