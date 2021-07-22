import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs';
import { Subject } from 'rxjs';
import { filter, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EventBusService {
  
  /*
   how to use
   to send an event
   -this.eventbus.emit(new EmitEvent(Events.[YourEvent],[data]))
    ex: this.eventbus.emit(new EmitEvent(Events.RowSelected,this.data));
    
   to subscribe to an event
   -this.eventbus.on(Events.[YourEvent],[callback function])
    ex:this.eventbus.on(Events.RowSelected,(data=> this.data =data));
  */
  private subject = new Subject<any>();
  
  constructor() { }

  on(event: Events, action:any): Subscription {
    return this.subject
        .pipe(
           filter((e: EmitEvent) => {
              return e.name === event;
           }),
           map((event:EmitEvent) => {
             return event.value;
           })
        )
        .subscribe(action);
  }

  emit(event: EmitEvent){
    this.subject.next(event);
  }


}

export class EmitEvent {
   constructor(public name: any, public value?:any){}
}

export enum Events{
  //add events here
  RowSelected
}
