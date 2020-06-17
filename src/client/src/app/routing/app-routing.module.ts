import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainComponent } from '../main/main.component';
import { NotExistComponent } from '../not-exist/not-exist.component';


const routes: Routes = [
  {path: '', redirectTo: '/app', pathMatch: 'full'},
  {path: 'app', component: MainComponent, pathMatch: 'full'},
  {path: '**', component: NotExistComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
