import { categoryService } from '../Services';

export const categories = {
    namespaced: true,
    state: {
        all: {}
    },
    actions: {
        getAll({ commit }) {
            commit('getAllRequest');
            categoryService.getAll()
                .then(
                    categories => commit('getAllSuccess', categories),
                    error => commit('getAllFailure', error)
                );
        },
        create({ dispatch, commit }, { name, image }) {
            categoryService.create(name, image)
                .then(
                    category => {
                        commit('createSuccess', category);
                    },
                    error => {
                        commit('createFailure', error);
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
        update({ dispatch, commit }, { id, name, image }) {
            categoryService.update(id, name, image)
                .then(
                    category => {
                        commit('createSuccess', category);
                    },
                    error => {
                        commit('createFailure', error);
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
        delete({ dispatch, commit }, { id }) {
            categoryService.DeleteByID(id)
                .then(
                    category => {
                    },
                    error => {
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
    },
    mutations: {
        getAllRequest(state) {
            state.all = { loading: true };
        },
        getAllSuccess(state, categories) {
            state.all = { items: categories };
        },
        getAllFailure(state, error) {
            state.all = { error };
        },
        createSuccess(state, category) {
            state.status = { created: true };
            state.category = category;
        },
        createFailure(state) {
            state.status = {};
            state.category = null;
        },
    }
}
