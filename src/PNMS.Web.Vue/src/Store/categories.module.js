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
        }
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
        }
    }
}
