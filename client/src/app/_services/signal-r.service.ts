import { Injectable } from '@angular/core';
import { MessageModel } from '../models/message-model';
import  * as signalR from '@aspnet/signalr';
@Injectable({
  providedIn: 'root'
})

export class SignalRService {
  data!: MessageModel;
  hubConnection!:signalR.HubConnection;


  constructor() { }

  startConnection(){
    this.hubConnection = new signalR.HubConnectionBuilder()
                      .withUrl('https://localhost:5001/messenger')
                      .build();
    
    this.hubConnection
    .start()
    .then(()=> console.log('Connection started'))
    .catch(err => console.log('Error while starting connection ',err));

  } 

  addUpdateStatusListener(){
    this.hubConnection.on('updatestatus',(data)=>{
        this.data = data;
        console.log(data);
    });
  }
}
