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
                    res => {
                        commit('createSuccess', res.category);
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
                    res => {
                        commit('updateSuccess', res.category);
                    },
                    error => {
                        alert(error);
                        commit('createFailure', error);
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
        delete({ dispatch, commit }, id) {
            categoryService.DeleteByID(id)
                .then(
                    category => {
                        commit('deleteSuccess', id);
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
            state.all.items.push(category);
        },
        updateSuccess(state, category) {
            const updatedCategories = [...state.all.items];
            const indexCategory = state.all.items.findIndex(i => category.Id === i.Id);

            //Update data
            updatedCategories[indexCategory].Name = category.Name;
            updatedCategories[indexCategory].ImageUrl = category.ImageUrl;

            state.all = { items: updatedCategories };
        },
        deleteSuccess(state, id) {
            //Remove category by ID
            const newLists = state.all.items.filter(x => {
                return x.Id != id;
            })

            state.all = { items: newLists };
        }
    }
}
