import { itemService } from '../Services';

export const items = {
    namespaced: true,
    state: {
        all: {}
    },
    actions: {
        getAll({ commit }) {
            commit('getAllRequest');
            itemService.getAll()
                .then(
                    items => commit('getAllSuccess', items),
                    error => commit('getAllFailure', error)
                );
        },
        create({ dispatch, commit }, { name, text, date, link }) {
            itemService.create(name, text, date, link)
                .then(
                    item => {
                        commit('createSuccess', item);
                    },
                    error => {
                        commit('createFailure', error);
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
    },
    mutations: {
        getAllRequest(state) {
            state.all = { loading: true };
        },
        getAllSuccess(state, items) {
            state.all = { items: items };
        },
        getAllFailure(state, error) {
            state.all = { error };
        },
        createSuccess(state, item) {
            state.status = { created: true };
            state.item = item;
        },
        createFailure(state) {
            state.status = {};
            state.user = null;
        },
    }
}
