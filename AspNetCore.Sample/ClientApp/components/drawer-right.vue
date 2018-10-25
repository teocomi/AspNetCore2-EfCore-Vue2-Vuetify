<template>
  <v-navigation-drawer fixed
                       :value="value"
                       class="grey lighten-4"
                       right
                       @input="$emit('value',$event)"
                       clipped
                       app>
    <!--<v-toolbar flat class="transparent">
      <v-list class="pa-0">
        <v-list-tile avatar>
          <v-list-tile-avatar>
            <v-icon large>account_circle</v-icon>
          </v-list-tile-avatar>

          <v-list-tile-content>
            <v-list-tile-title>{{ user.first }} {{ user.last }}</v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
      </v-list>
    </v-toolbar>-->
    <v-list class="grey lighten-4 pt-0">
      <v-divider></v-divider>
      <template v-for="(item, i) in menu">
        <v-layout v-if="item.heading"
                  :key="i"
                  row
                  align-center>
          <v-flex xs6>
            <v-subheader v-if="item.heading">
              {{ item.heading }}
            </v-subheader>
          </v-flex>
        </v-layout>
        <v-divider v-else-if="item.divider"
                   :key="i"
                   dark
                   class="my-3"></v-divider>
        <v-list-tile v-else
                     exact
                     :key="i"
                     @click.stop.prevent="$emit('openDialog')">
            <v-list-tile-action>
              <v-icon>{{ item.icon }}</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title class="grey--text">
                {{ item.text }}
              </v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
</template>
    </v-list>
  </v-navigation-drawer>
</template>

<script>

    export default {
        props: ['value'],
        data() {
            return {
                //user: user,
              menu: [
                { icon: 'add', text: 'New Room', path:'/brief' },
                //{ icon: 'exit_to_app', text: 'Log out', href: '/account/signout' }
              ],

            }
        },
        methods: {
        },
        mounted: function () {
        }
        
    }
</script>

<style>
   
</style>
