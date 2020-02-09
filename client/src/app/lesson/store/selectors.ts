import { createFeatureSelector, createSelector } from '@ngrx/store';
import { LessonState } from './reducer';

export const getLessonState = createFeatureSelector<LessonState>('lessonStore');

export const getFirstWord = createSelector(getLessonState, (state: LessonState) => state.words[0]);

export const isAnyWord = createSelector(getLessonState, (state: LessonState) => state.words.length > 0);

export const getLessonStateEnum = createSelector(getLessonState, (state: LessonState) => state.lessonState);

export const getLessonMode = createSelector(getLessonState, (state: LessonState) => state.lessonMode);

export const getLessonResult = createSelector(getLessonState, (state: LessonState) => state.result);

export const getLastAnswer = createSelector(getLessonState, (state: LessonState) => state.lastAnswer);