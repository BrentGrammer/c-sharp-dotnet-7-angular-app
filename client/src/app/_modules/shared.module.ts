import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
/**
 * Group shared modules here. 
 * Remember to add this to the app module
 */
@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(),
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
    }),
  ],
  // need to export for the imports shared to work
  exports: [
    BsDropdownModule,
    ToastrModule // No need for forRoot in here or config, just need to export the names.
  ]
})
export class SharedModule {}
