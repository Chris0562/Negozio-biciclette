import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { DEFAULT_INTERRUPTSOURCES, Idle } from '@ng-idle/core';
import { LoginManagerService } from './login-manager.service';

@Injectable({
  providedIn: 'root'
})
export class SessionManagerService {

  constructor(private idle: Idle,
    private srv: LoginManagerService,
    private router: Router) {

    this.idle.setIdle(300);

    this.idle.setTimeout(1000);

    this.idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);

    this.idle.onIdleStart.subscribe(() => console.log('sono idle'))

    this.idle.onTimeoutWarning.subscribe((countdown) => {
      console.log(`You will logout in ${countdown}`)
    });

    this.idle.onTimeout.subscribe(() => {
      this.srv.clearSession();
      this.router.navigateByUrl('login');
    })
  }   
  
  reset() {
    this.idle.watch();
  }

  ngOnInit() {
    this.reset();
  }

  stop() {
    this.idle.stop();
  }
}
