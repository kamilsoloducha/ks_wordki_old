import {
    LessonActions,
    LessonActionTypes
} from './actions';
import { WordRepeat } from '../models/word-repeat';
import { LessonStateEnum, LessonStep } from '../models/lesson-state';
import { LessonModeType } from '../models/lesson-mode';
import { LessonResult } from '../models/lesson-result';

export interface LessonState {
    words: WordRepeat[];
    lastAnswer: boolean;
    lessonState: LessonStep;
    lessonMode: LessonModeType;
    result: LessonResult;
}

const initialState: LessonState = {
    words: [],
    lastAnswer: false,
    lessonState: LessonStep.getLessonStep(LessonStateEnum.BeforeStart),
    lessonMode: LessonModeType.Unknown,
    result: null
};

export function reducer(state = initialState, action: LessonActions): LessonState {
    switch (action.type) {
        case LessonActionTypes.SetWords: return { ...state, words: [...state.words, ...action.words] };
        case LessonActionTypes.RemoveWord: return handleRemoveWord(state);
        case LessonActionTypes.SetLessonMode: return { ...state, lessonMode: action.payload.mode };
        case LessonActionTypes.SetLastAction: return { ...state, lastAnswer: action.payload.isCorrect };
        case LessonActionTypes.StartLesson: return handleStartLesson(state);
        case LessonActionTypes.CheckAnswer: return { ...state, lessonState: LessonStep.getLessonStep(LessonStateEnum.AnswerDisplay) };
        case LessonActionTypes.Answer: return handleAnswer(state, action.payload.result);
        case LessonActionTypes.FinishLesson: return handleFinishLesson(state);
        case LessonActionTypes.ResetStoreAction: return initialState;
        default: return state;
    }
}

export function handleRemoveWord(state: LessonState): LessonState {
    return { ...state, words: state.words.slice(1, state.words.length) };
}

export function handleStartLesson(state: LessonState): LessonState {
    const result = new LessonResult();
    result.startTime = new Date();
    return {
        ...state,
        lessonState: LessonStep.getLessonStep(LessonStateEnum.WordDisplay),
        result: result
    };
}

export function handleAnswer(state: LessonState, answer: number): LessonState {
    const result = state.result;
    if (answer > 0) {
        result.correct++;
    } else if (answer < 0) {
        result.wrong++;
    } else {
        result.accepted++;
    }
    return {
        ...state,
        lessonState: LessonStep.getLessonStep(LessonStateEnum.WordDisplay),
        result: result
    };
}

export function handleFinishLesson(state: LessonState): LessonState {
    const result = state.result;
    const time = new Date().getTime() - result.startTime.getTime();
    result.wholeTime = time;
    return {
        ...state,
        lessonState: LessonStep.getLessonStep(LessonStateEnum.AfterFinish),
        result: result
    };
}
