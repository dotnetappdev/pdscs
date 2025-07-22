import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { PersonsComponent } from './components/persons/persons.component';
@NgModule({
  declarations: [
    AppComponent,
    PersonsComponent
  ],
  bootstrap: [AppComponent], imports: [BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    FormsModule,
  RouterModule.forRoot([
    { path: '', component: PersonsComponent, pathMatch: 'full' },
    { path: 'persons', component: PersonsComponent }
  ])], providers: [provideHttpClient(withInterceptorsFromDi())]
})
export class AppModule { }
