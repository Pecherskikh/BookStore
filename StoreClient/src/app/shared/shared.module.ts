import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { routes } from './shared-routing.module';
import { RemoveComponent } from './components/remove/remove.component';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '../material/material.module';


@NgModule({
  declarations: [RemoveComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    MaterialModule
  ]
})
export class SharedModule { }
