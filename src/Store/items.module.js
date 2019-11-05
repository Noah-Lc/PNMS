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
        update({ dispatch, commit }, { id, name, text, date, link, categoryId }) {
            itemService.update(id, name, text, date, link, categoryId)
                .then(
                    item => {
                        commit('updateItemSuccess', item)
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
            const indexItem = state.all.items.findIndex(i => item.Id === i.Id);
            
            //Update item data
            updatedItems[indexItem].Name = item.Name;
            updatedItems[indexItem].Text = item.Text;
            updatedItems[indexItem].Date = item.Date;
            updatedItems[indexItem].LinkUrl = item.LinkUrl;
            updatedItems[indexItem].CategoryID = item.CategoryID;
            updatedItems[indexItem].CategoryName = item.CategoryName;

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
