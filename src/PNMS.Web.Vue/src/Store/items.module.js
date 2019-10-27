import { itemService } from '../Services';

export const items = {
    namespaced: true,
    state: {
        all: {}
    },
    actions: {
        getAll({ commit }) {
            commit('getAllRequest', items);
            itemService.getAll()
                .then(
                    
                    items => commit('getAllSuccess', items),
                    error => commit('getAllFailure', error)
                );
        },
        create({ dispatch, commit }, { name, text, date, link, categoryId }) {
            itemService.create(name, text, date, link, Number(categoryId))
                .then(
                    item => {
                        commit('addItemSuccess', item);
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
                        commit('updateItemSuccess', { id: id, name: name, text: text, date: date, link: link, categoryid: 1 })
                    },
                    error => {
                        dispatch('alert/error', error, { root: true });
                    }
                );
        },
        delete({ dispatch, commit }, id) {
            itemService.deleteByID(id)
                .then(
                    item => {
                        commit('deleteItemSuccess', id)
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
        addItemSuccess(state, item) {
            state.all.items.push(item);
        },
        updateItemSuccess(state, item) {
            const updatedItems = [...state.all.items];
            const indexItem = state.all.items.findIndex(i => item.id === i.Id);

            //Update item data
            updatedItems[indexItem].Name = item.name;
            updatedItems[indexItem].Text = item.text;
            updatedItems[indexItem].Date = item.date;
            updatedItems[indexItem].LinkUrl = item.link;

            state.all = { items: updatedItems };
        },
        deleteItemSuccess(state, id) {
            //Delete item by ID
            const newLists = state.all.items.filter(x => {
                return x.Id != id;
            })

            state.all = { items: newLists };
        }
    }
}
