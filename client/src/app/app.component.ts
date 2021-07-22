import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './models/user';
import { AccountService } from './_services/account.service';
import { SignalRService } from './_services/signal-r.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating App';
  users:any;

  constructor(private accountService:AccountService,
    private signalRService:SignalRService,private httpClient:HttpClient){

  }
  ngOnInit(): void {
      this.setCurrentUser();
     // this.signalRService.startConnection();
      //this.signalRService.addUpdateStatusListener();
      //this.startHttpRequest();
  }

  setCurrentUser(){
    const user:User = JSON.parse(localStorage.getItem('user')!);
    this.accountService.setCurrentUser(user);
  }

  startHttpRequest(){
    this.httpClient.get('https://localhost:5001/api/messenger')
    .subscribe(response =>{
        console.log(response);  
    })
  }

  

}
