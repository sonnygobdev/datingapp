import { Input } from '@angular/core';
import { Self } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ControlValueAccessor, NgControl, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css'],
  
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() label!:string;
  @Input() type = 'text';
  
  
  constructor(@Self() public ngControl:NgControl) {
     this.ngControl.valueAccessor = this;
   }

  writeValue(obj: any): void {
     
  }
  registerOnChange(fn: any): void {
    
  }
  registerOnTouched(fn: any): void {
     
  }

  

}
