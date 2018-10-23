<template>
  <div>
    <v-dialog v-model="dialog" max-width="500px" persistent>
      <v-card>
        <v-card-title>
          <span class="headline">{{ formTitle }}</span>
        </v-card-title>
        <v-card-text>
          <v-container grid-list-md>
            <v-layout wrap>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="editedItem.number" label="Number"></v-text-field>
              </v-flex>
              <v-flex sm12 md8>
                <v-text-field v-model="editedItem.name" label="Name"></v-text-field>
              </v-flex>
              <v-flex xs12 sm6 md4>
                <v-text-field v-model="editedItem.area" label="Area"></v-text-field>
              </v-flex>
            </v-layout>
          </v-container>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" flat @click.native="close">Cancel</v-btn>
          <v-btn color="blue darken-1" flat @click.native="save">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>


    <v-data-table :headers="headers" :items="rooms" hide-actions class="elevation-1">
      <template slot="items" slot-scope="props">
        <td class="text-xs-center">{{ props.item.number }}</td>
        <td>{{ props.item.name }}</td>
        <td class="text-xs-center">{{ props.item.area }}</td>

        <td class="justify-center layout px-0">
          <v-btn icon class="mx-0" @click="editItem(props.item)">
            <v-icon color="teal">edit</v-icon>
          </v-btn>
        </td>
      </template>
    </v-data-table>

  </div>

</template>

<script>
  let api = '/api/rooms/';

  export default {
    props: ['dialog'],
    data() {
      return {
        errors: [],
        error: "",
        showAlert: false,
        alerttext: "",
        headers: [

          { text: 'Number', value: 'number', align: 'center' },
          { text: 'Room Name', value: 'name' },
          { text: 'Area', value: 'area', align: 'center' },
          { text: 'Actions', value: 'name', sortable: false, align: 'center' }
        ],
        rooms: [],
        levels: [],
        editedIndex: -1,
        editedItem: {
          name: '',
          area: 0,
          id: 0
        },
        defaultItem: {
          name: '',
          area: 0,
          id: 0
        },

      }
    },
    mounted: function () {

      this.model, this.filteredModel = [];
      this.$http.get(api).then((response) => {
        if (response.status === 200) {
          this.rooms = response.data;
        }
      }).catch(err => {
        this.showError(err)
      });

    },
    methods: {
      editItem(item) {
        this.editedIndex = this.rooms.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.$emit('openDialog')
      },

      deleteItem(item) {
        const index = this.rooms.indexOf(item)
        confirm('Are you sure you want to delete this item?') && this.rooms.splice(index, 1);
      },
      close() {
        this.$emit('closeDialog')
        setTimeout(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        }, 300)
      },

      save() {
        //edit
        if (this.editedIndex > -1) {
          this.rooms.splice(this.editedIndex, 1, this.editedItem);

          this.$http.put(api + this.editedItem.id, this.editedItem).then((response) => {
            if (response.status === 200) {
            }
          }).catch(err => {
            console.log(err)
            this.showError(err)
          });

        }
        //add
        else {
          this.rooms.push(this.editedItem);
          console.log(this.editedItem)
          this.$http.post(api, this.editedItem).then((response) => {

            if (response.status === 200) {
            }
          }).catch(err => {
            this.showError(err)
          });
        }

        this.close()
      },
      showError: function (error) {
        // horrible logic, to fix
        try {
          var jsonerrors = JSON.parse(error);
          if (jsonerrors.errors != null) {
            this.errors = jsonerrors.errors;
            this.error = ""
            this.showAlert = true;
            return;
          }
        }
        catch (err) { }

        this.error = error;
        this.showAlert = true;
      },

    },
    computed: {
      formTitle() {
        return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
      }
    },

  }
</script>

<style>
</style>
