import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LessonRoutingModule } from './lesson-routing.module';
import { FiszkaComponent } from './components/fiszka/fiszka.component';
import {
  LessonHttpBaseService,
  LessonHttpService,
  LessonHttpMockService
} from './services/lesson-http/lesson-http.service';
import { environment } from 'src/environments/environment';
import { StoreModule, Store } from '@ngrx/store';
import { reducer } from './store/reducer';
import { EffectsModule } from '@ngrx/effects';
import { LessonEffects } from './store/effects';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ShareModule } from '../share/share.module';
import { StoperComponent } from './components/stoper/stoper.component';
import { WordComparerService } from './services/word-comparer/word-comparer.service';
import { UserModule } from '../user/user.module';
import { AuthorizationModule } from '../authorization/authorization.module';
import { WordMapper } from './services/word-mapper/word-mapper';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';
import { SummaryGuardService } from './services/summary-guard/summage-guard';
import { SummaryComponent } from './components/summary/summary.component';
import { FiszkaSideComponent } from './components/fiszka-side/fiszka-side.component';
import { FiszkaService } from './components/fiszka/services/fiszka/fiszka.service';
import { SummaryService } from './components/summary/services/summary/summary.service';
import { InsertService } from './components/insert/service/insert/insert.service';
import { InsertComponent } from './components/insert/insert.component';
import { TimerService } from './services/stoper/stoper2.service';
import { StoperService } from './components/stoper/services/stoper/stoper.service';
import { ResultsComponent } from './components/results/results.component';
import { ResultsService } from './components/results/services/results/results.service';

@NgModule({
  declarations: [
    FiszkaComponent,
    InsertComponent,
    StoperComponent,
    SummaryComponent,
    FiszkaSideComponent,
    ResultsComponent,
  ],
  imports: [
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
    LessonRoutingModule,
    HttpClientModule,
    ShareModule,
    UserModule,
    AuthorizationModule,
    ButtonModule,
    CheckboxModule,
    StoreModule.forFeature('lessonState', reducer),
    EffectsModule.forFeature([LessonEffects]),
  ],
  providers: [
    Store,
    { provide: LessonHttpBaseService, useClass: environment.mockServer ? LessonHttpService : LessonHttpMockService },
    WordComparerService,
    WordMapper,
    SummaryGuardService,
    FiszkaService,
    SummaryService,
    InsertService,
    TimerService,
    StoperService,
    ResultsService
  ]
})
export class LessonModule {
}
