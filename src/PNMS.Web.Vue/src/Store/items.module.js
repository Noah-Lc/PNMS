import { itemService } from '../Services';

export const items = {
    namespaced: true,
    state: {
        all: {}
    },
    actions: {
        getAll({ commit }) {
            itemService.getAll()
                .then(
                    items => commit('getAllSuccess', items),
                    error => commit('getAllFailure', error)
                );
        },
        create({ dispatch, commit }, { name, text, date, link }) {
            itemService.create(name, text, date, link, 1)
                .then(
                    item => {
                        //commit('AddItemSuccess', item);
                    },
                    error => {
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
        update({ dispatch, commit }, { id, name, text, date, link }) {
            itemService.update(id, name, text, date, link, 1)
                .then(
                    item => {
                        //commit('UpdateItemmSuccess', {id: id, name: name, text: text, date: date, link: link,categoryid: 1})
                    },
                    error => {
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
        delete({ dispatch, commit }, { id }) {
            commit('');
            itemService.deleteByID(id)
                .then(
                    item => {
                        //commit('DeleteItemSuccess', item)
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
        getAllSuccess(state, items) {
            state.all = { items: items };
        },
        getAllFailure(state, error) {
            state.all = { error };
        },
        AddItemSuccess(state, item) {
            state.all.items.push(item);
        },
        UpdateItemmSuccess(state, item) {
            const updatedItems = [...state.all.items];
            const indexItem = state.all.items.findIndex(i => item.id === i.id);
            updatedItems[indexItem] = item;
            state.all = { items: updatedItems };
            console.log(indexItem);

        },
        DeleteItemSuccess(state, item) {
            state.all = { update: item };
        }
    }
}
