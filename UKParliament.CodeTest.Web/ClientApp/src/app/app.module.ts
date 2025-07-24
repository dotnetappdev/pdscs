import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { PersonsComponent } from './components/persons/persons.component';
import { DepartmentComponent } from './components/departments/department.component';
import { ConfirmDeleteModalModule } from './components/shared/confirm-delete-modal.module';
import { ToastComponent } from './components/shared/toast.component';
import { DepartmentService } from './services/department.service';
@NgModule({
  declarations: [
    AppComponent,
    PersonsComponent,
    DepartmentComponent,
    ToastComponent
  ],
  bootstrap: [AppComponent], imports: [BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
    ConfirmDeleteModalModule,
  RouterModule.forRoot([
    { path: '', component: PersonsComponent, pathMatch: 'full' },
    { path: 'persons', component: PersonsComponent },
    { path: 'departments', component: DepartmentComponent }
  ])], providers: [provideHttpClient(withInterceptorsFromDi()), DepartmentService]
})
export class AppModule { }
