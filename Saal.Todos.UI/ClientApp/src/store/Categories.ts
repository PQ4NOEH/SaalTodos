import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface CategoriesState {
    isLoading: boolean;
    categories: Category[];
}

export interface Category {
    id: number;
    name: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestCategoriesAction {
    type: 'REQUEST_CATEGORIES';
}

interface ReceiveCategoriesAction {
    type: 'RECEIVE_CATEGORIES';
    categories: Category[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestCategoriesAction | ReceiveCategoriesAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestCategories: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.weatherForecasts) {
            fetch(`weatherforecast`)
                .then(response => response.json() as Promise<Category[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_CATEGORIES', categories: data });
                });

            dispatch({ type: 'REQUEST_CATEGORIES'});
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: CategoriesState = { categories: [], isLoading: false };

export const reducer: Reducer<CategoriesState> = (state: CategoriesState | undefined, incomingAction: Action): CategoriesState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_CATEGORIES':
            return {
                categories: state.categories,
                isLoading: true
            };
        case 'RECEIVE_CATEGORIES':
            return {

                categories: state.categories,
                isLoading: false
            };
        default:
            return state;
    }
};
