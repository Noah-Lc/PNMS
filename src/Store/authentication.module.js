import { userService } from '../Services';
import { router } from '../Routes';

const user = JSON.parse(localStorage.getItem('user'));
const initialState = user
    ? { status: { loggedIn: true }, user }
    : { status: {}, user: null };

export const authentication = {
    namespaced: true,
    state: initialState,
    actions: {
        register({ dispatch, commit }, { username, password, firstname, lastname, email }) {
            commit('registerRequest', { username });

            userService.register(username, password, firstname, lastname, email)
                .then(
                    user => {
                        commit('registerSuccess', user);
                        router.push('/Login');
                    },
                    error => {
                        commit('registerFailure', error);
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
        login({ dispatch, commit }, { username, password }) {
            commit('loginRequest', { username });

            userService.login(username, password)
                .then(
                    user => {
                        commit('loginSuccess', user);
                        router.push('/Dashboard');
                    },
                    error => {
                        commit('loginFailure', error);
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
        logout({ commit }) {
            userService.logout();
            commit('logout');
        }
    },
    mutations: {
        loginRequest(state, user) {
            state.status = { loggingIn: true };
            state.user = user;
        },
        loginSuccess(state, user) {
            state.status = { loggedIn: true };
            state.user = user;
        },
        loginFailure(state) {
            state.status = {};
            state.user = null;
        },
        registerRequest(state, user) {
            state.status = { registerIn: true };
            state.user = user;
        },
        registerSuccess(state, user) {
            state.status = { registerIn: true };
            state.user = user;
        },
        registerFailure(state) {
            state.status = {};
            state.user = null;
        },
        logout(state) {
            state.status = {};
            state.user = null;
        }
    }
}
