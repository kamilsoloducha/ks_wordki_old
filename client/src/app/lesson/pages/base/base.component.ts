import { OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { LessonState } from '../../store/reducer';
import { RouteParamsHandler } from '../../services/route-params.handler/route-params.handler';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { isAnyWord } from '../../store/selectors';
import { ResetStoreAction } from '../../store/actions';

export class BaseComponent implements OnInit, OnDestroy {
    protected routeParamSub: Subscription;
    protected nextWordSub: Subscription;

    constructor(protected lessonStore: Store<LessonState>,
        protected routeParamsHandler: RouteParamsHandler,
        protected route: ActivatedRoute,
        protected router: Router) {
    }

    ngOnInit(): void {
        this.routeParamSub = this.route.params.subscribe((params: Params) => this.routeParamsHandler.handle(params));
        this.nextWordSub = this.lessonStore.select(isAnyWord).subscribe((isAny: boolean) => this.handleIsAnyWord(isAny));
    }

    ngOnDestroy(): void {
        this.routeParamSub.unsubscribe();
        this.nextWordSub.unsubscribe();
        this.finishLesson();
    }

    protected handleIsAnyWord(isAny: boolean): void {
        if (!isAny) {
            this.router.navigate(['dashboard']);
        }
    }

    protected finishLesson(): void {
        this.lessonStore.dispatch(new ResetStoreAction());
    }
}
