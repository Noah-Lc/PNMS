<template>
    <div>
        <div class="page">
            <!-- navbar-->
            <header class="header">
                <nav class="navbar">
                    <div class="container-fluid">
                        <div class="navbar-holder d-flex align-items-center justify-content-between">
                            <div class="navbar-header">
                                <a href="index.html" class="navbar-brand">
                                    <div class="brand-text d-none d-md-inline-block">
                                        <strong class="text-primary">Dashboard</strong></div>
                                </a>
                            </div>
                            <ul class="nav-menu list-unstyled d-flex flex-md-row align-items-md-center">
                                <!-- Log out-->
                                <li class="nav-item">
                                    <a href="/login" class="nav-link logout"> <span class="d-none d-sm-inline-block">Logout</span><i class="fa fa-sign-out"></i></a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </nav>
            </header>
            <!-- Breadcrumb-->
            <div class="breadcrumb-holder">
                <div class="container-fluid">
                    <ul class="breadcrumb">
                        <li class="breadcrumb-item"><a href="index.html">Home</a></li>
                        <li class="breadcrumb-item active">Tables</li>
                    </ul>
                </div>
            </div>
            <section>
                <div class="container-fluid">
                    <!-- Page Header-->
                    <header>
                        <h1 class="h3 display">Tables</h1>
                    </header>
                    <div class="row">
                        <div class="col-lg-8">
                            <div class="card">
                                <div class="card-header">
                                    <h4 class="float-left mt-2">Items Table</h4>
                                    <button class="mr-3 btn btn-primary float-right" data-toggle="modal" data-target="#modalItem">New Item</button>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive text-center">
                                        <em v-if="items.loading">Loading items...</em>
                                        <span v-if="items.error" class="text-danger">ERROR: Failed to fetch</span>
                                        <table class="table table-striped" v-if="items.items">
                                            <thead>
                                                <tr>
                                                    <th>#</th>
                                                    <th>Name</th>
                                                    <th>Text</th>
                                                    <th>Category</th>
                                                    <th>Date</th>
                                                    <th>Link url</th>
                                                    <th>Action</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr v-for="item in items.items" :key="item.Id">
                                                    <th scope="row">{{ item.Id }}</th>
                                                    <td>{{ showString(item.Name, 15) }}</td>
                                                    <td>{{ showString(item.Text, 20) }}</td>
                                                    <td>{{ item.CategoryName }}</td>
                                                    <td>{{ formatDate(item.Date) }}</td>
                                                    <td><a href="#">{{ item.LinkUrl }}</a></td>
                                                    <td>
                                                        <button class="mr-3 btn btn-danger p-1" v-on:click="deleteItem(item.Id)" data-toggle="modal" data-target="#confirmationItem">
                                                            <img src="../../public/src/img/delete.svg" alt="delete" class="icon-btn">
                                                        </button>
                                                        <button class="btn btn-info p-1" v-on:click="editItem(item.Id)" data-toggle="modal" data-target="#modalItem">
                                                            <img src="../../public/src/img/update.svg" alt="update" class="icon-btn">
                                                        </button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="card">
                                <div class="card-header">
                                    <h4 class="float-left mt-2">Categories Table</h4>
                                    <button class="mr-3 btn btn-primary float-right" data-toggle="modal" data-target="#modalCategory">New Category</button>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive text-center">
                                        <em v-if="categories.loading">Loading categories...</em>
                                        <span v-if="categories.error" class="text-danger">ERROR: Failed to fetch</span>
                                        <table class="table table-striped" v-if="categories.items">
                                            <thead>
                                                <tr>
                                                    <th>#</th>
                                                    <th>Name</th>
                                                    <th>Image</th>
                                                    <th>Action</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr v-for="category in categories.items" :key="category.Id">
                                                    <th scope="row">{{ category.Id }}</th>
                                                    <td>{{ category.Name }}</td>
                                                    <td>{{ showString(category.ImageUrl, 20) }}</td>
                                                    <td>
                                                        <button class="mr-3 btn btn-danger p-1" v-on:click="deleteCategory(category.Id)" data-toggle="modal" data-target="#confirmationCategory">
                                                            <img src="../../public/src/img/delete.svg" alt="delete" class="icon-btn">
                                                        </button>
                                                        <button class="btn btn-info p-1"  v-on:click="editCategory(category.Id)" data-toggle="modal" data-target="#modalCategory">
                                                            <img src="../../public/src/img/update.svg" alt="update" class="icon-btn">
                                                        </button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal" id="confirmationItem" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Delet</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <p>Are you sure you want to delete this item?</p>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                    <button type="button" class="btn btn-danger" v-on:click="confirmationDeleteItem()">Delete</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal" id="confirmationCategory" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Delet</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <p>Are you sure you want to delete this category?</p>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                    <button type="button" class="btn btn-danger" v-on:click="confirmationDeleteCategory()">Delete</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal" id="modalItem" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">New Item</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <p>Lorem ipsum dolor sit amet consectetur.</p>
                                    <form>
                                        <div class="form-group">
                                            <label>Name</label>
                                            <input type="text" v-model="item.name" placeholder="Name Category" class="form-control" :class="{ 'is-invalid': item.submitted && !item.name }">
                                        </div>
                                        <div class="form-group">
                                            <label>Category</label>
                                            <select class="form-control" v-model="item.categoryId">
                                                <option disabled value>Choose Category</option>
                                                <option v-for="category in categories.items" :key="category.Id" v-bind:value="category.Id" >{{ category.Name }}</option>
                                            </select>
                                        </div>
                                        <div class="form-group">
                                            <label>Date</label>
                                            <input type="date" v-model="item.date" placeholder="Date" class="form-control" :class="{ 'is-invalid': item.submitted && !item.date }">
                                        </div>
                                        <div class="form-group">
                                            <label>Text</label>
                                            <textarea type="text" v-model="item.text" placeholder="Text" class="form-control" :class="{ 'is-invalid': item.submitted && !item.text }"></textarea>
                                        </div>
                                        <div class="form-group">
                                            <label>Link</label>
                                            <input type="text" v-model="item.link" placeholder="Link" class="form-control" :class="{ 'is-invalid': item.submitted && !item.link }">
                                        </div>
                                    </form>
                                </div>
                                <div class="modal-footer">
                                    <button v-if="item.new" type="button" class="btn btn-primary" v-on:click="createItem()">Create</button>
                                    <button v-if="!item.new" type="button" class="btn btn-primary" v-on:click="updateItem()">Update</button>
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal" v-on:click="clearModal()">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal" id="modalCategory" tabindex="-1" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">New Item</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <p>Lorem ipsum dolor sit amet consectetur.</p>
                                    <form>
                                        <div class="form-group">
                                            <label>Name</label>
                                            <input type="text" v-model="category.name" placeholder="Name Category" class="form-control">
                                        </div>
                                        <div class="form-group">
                                            <label>Image</label>
                                            <div class="box">
                                                <input type="file" name="file-7[]" id="file-7" class="inputfile inputfile-6" data-multiple-caption="{count} files selected" multiple @change="onFileChanged" />
                                                <label for="file-7"><span class="form-control" v-html="fileName"></span> <strong class="btn btn-primary"><svg xmlns="http://www.w3.org/2000/svg" width="20" height="17" viewBox="0 0 20 17"><path d="M10 0l-5.2 4.9h3.3v5.1h3.8v-5.1h3.3l-5.2-4.9zm9.3 11.5l-3.2-2.1h-2l3.4 2.6h-3.5c-.1 0-.2.1-.2.1l-.8 2.3h-6l-.8-2.2c-.1-.1-.1-.2-.2-.2h-3.6l3.4-2.6h-2l-3.2 2.1c-.4.3-.7 1-.6 1.5l.6 3.1c.1.5.7.9 1.2.9h16.3c.6 0 1.1-.4 1.3-.9l.6-3.1c.1-.5-.2-1.2-.7-1.5z"/></svg></strong></label>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                                <div class="modal-footer">
                                    <button v-if="category.new" type="button" class="btn btn-primary" v-on:click="createCategory()">Create</button>
                                    <button v-if="!category.new" type="button" class="btn btn-primary" v-on:click="updateCategory()">Update</button>
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal" v-on:click="clearModal()">Close</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</template>

<script>
import moment from 'moment'

export default {
    data () {
        return {
            fileName: '',
            item :{
                id: '',
                name: '',
                text: '',
                date: '',
                link: '',
                categoryId: '',
                submitted: false,
                new: true
            },
            category :{
                id: '',
                name: '',
                image: null,
                submitted: false,
                new: true
            }
        }
    },
    computed: {
        user () {
            return this.$store.state.authentication.user;
        },
        categories () {
            return this.$store.state.categories.all;
        },
        items () {
            return this.$store.state.items.all;
        }
    },
    // define methods under the `methods` object
    methods: {
        onFileChanged (event) {
            this.category.image = event.target.files[0]
            this.fileName = this.category.image.name;
        },
        showString(text, max){
            return text.length > max ? text.substring(0,max)+".." : text;
        },
        formatDate(date){
            return moment(String(date)).format('MM/DD/YYYY');
        },
        editItem: function (id) {
            // `id` is the id of Item
            if (id) {
                let item = this.items.items.find(item => item.Id === id);
                
                this.item.id = item.Id;
                this.item.name = item.Name;
                this.item.text = item.Text;
                this.item.categoryId = item.CategoryID;
                this.item.date = moment(String(item.Date)).format('YYYY-MM-DD');
                this.item.link = item.LinkUrl;
                this.item.new = false;
            }
        },
        updateItem: function () {
            // `id` is the id of Item
            if (this.item.id) {
                this.category.submitted = true;
                const { id, name, text, date, link, categoryId } = this.item;
                const { dispatch } = this.$store;
                
                if (name && text && date && link, categoryId) {
                    dispatch('items/update', { id, name, text, date, link, categoryId });
                    $('#modalItem').modal('toggle');
                    this.clearModal();
                }
            }
        },
        deleteItem: function (id) {
            // `id` is the id of Item
            this.item.id = id;
        },
        confirmationDeleteItem: function () {
            // `id` is the id of Item
            const id = this.item.id;
            const { dispatch } = this.$store;
            if (id) {
                dispatch('items/delete', id);
                $('#confirmationItem').modal('toggle');
            }
        },
        editCategory: function (id) {
            // `id` is the id of Item
            if (id) {
                let category = this.categories.items.find(category => category.Id === id);
                this.category.id = category.Id;
                this.category.name = category.Name;
                this.fileName = category.ImageUrl;
                this.category.new = false;
            }
        },
        updateCategory(){
            if (this.category.id) {
                this.category.submitted = true;
                const { id, name, image } = this.category;
                const { dispatch } = this.$store;
                
                if (name) {
                    dispatch('categories/update', { id, name, image });
                    $('#modalCategory').modal('toggle');
                    this.clearModal();
                }
            }
        },
        deleteCategory: function (id) {
            // `id` is the id of Item
            if (id) {
                 this.category.id = id;
            }
        },
        confirmationDeleteCategory: function () {
            // `id` is the id of Item
            const id = this.category.id;
            const { dispatch } = this.$store;
            if (id) {
                dispatch('categories/delete', id);
                $('#confirmationCategory').modal('toggle');
            }
        },
        createItem(){ 
            this.category.submitted = true;
            const { name, text, date, link, categoryId } = this.item;
            const { dispatch } = this.$store;
            
            if (name && text && date && link && categoryId) {
                dispatch('items/create', { name, text, date, link, categoryId });
                $('#modalItem').modal('toggle');
            }
        },
        createCategory(){
            this.category.submitted = true;
            const { name, image } = this.category;
            const { dispatch } = this.$store;
            
            if (name, image) {
                dispatch('categories/create', { name, image });
                $('#modalCategory').modal('toggle');
            }
        },
        clearModal(){
            this.category = {name: '', image: '', submitted: false, new: true};
            this.item = {name: '', text: '', date: '', link: '', categoryId: '', submitted: false, new: true};
            this.fileName = '';
        }
    },
    created () {
        this.$store.dispatch('categories/getAll');
        this.$store.dispatch('items/getAll');
    }
}
</script>

<style>
.inputfile {
    width: 0.1px;
    height: 0.1px;
    opacity: 0;
    overflow: hidden;
    position: absolute;
    z-index: -1;
}

.inputfile + label {
    font-size: 1.25rem;
    font-weight: 700;
    text-overflow: ellipsis;
    white-space: nowrap;
    cursor: pointer;
}

.no-js .inputfile + label {
    display: none;
}

.inputfile:focus + label,
.inputfile.has-focus + label {
    outline: 1px dotted #000;
    outline: -webkit-focus-ring-color auto 5px;
}

.inputfile + label svg {
    width: 1em;
    height: 1em;
    vertical-align: middle;
    fill: currentColor;
}

.inputfile-6 + label span,
.inputfile-6 + label strong {
    padding: 9px 13px;
}

.inputfile-6 + label strong {
    margin-left: -6px;
}

.inputfile-6 + label span {
    width: 200px;
    display: inline-block;
    text-overflow: ellipsis;
    white-space: nowrap;
    overflow: hidden;
    vertical-align: top;
}

.inputfile-6 + label strong {
    height: 100%;
    display: inline-block;
}

.icon-btn{
    width: 25px;
}
textarea{
    height: 240px !important;
    resize: none;
}
</style>