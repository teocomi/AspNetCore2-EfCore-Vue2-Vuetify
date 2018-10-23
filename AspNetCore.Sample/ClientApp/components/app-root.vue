<template>
  <v-app id="app" fixed>
    <toolbar @toggleLeftDrawer="toggleLeftDrawer"
             @toggleRightDrawer="toggleRightDrawer">
    </toolbar>
    <drawer-right v-model="drawerRight" @drawerRightInput="drawerRightInput" @openDialog="dialog=true" />
    <v-content class="grey lighten-4">
      <v-container fluid>
        <transition name="component-fade" mode="out-in">
          <router-view v-bind:dialog="dialog" @closeDialog="dialog=false" @openDialog="dialog=true"></router-view>

        </transition>
      </v-container>
    </v-content>
  </v-app>

</template>

<script>
  import Toolbar from './toolbar'
  import DrawerRight from './drawer-right'
  import Vue from 'vue'

  Vue.component('toolbar', Toolbar);
  Vue.component('drawer-right', DrawerRight);

  export default {
    data() {
      return {
        dialog: false,
        drawerRight: null,
        drawerRightOld: null,
        drawerLeft: null,
        drawerLeftOld: null,
      }
    },
    methods: {
      toggleLeftDrawer() {
        this.drawerLeft = !this.drawerLeft;
      },
      toggleRightDrawer() {
        this.drawerRight = !this.drawerRight;
      },
      drawerRightInput(event) {
        if (this.drawerRightOld == this.drawerRight)
          this.toggleRightDrawer()
        this.drawerRightOld = event;
      },
      drawerLeftInput(event) {
        if (this.drawerLeftOld == this.drawerLeft)
          this.toggleLeftDrawer()
        this.drawerLeftOld = event;
      },
      drawerLeftClose() {
        this.drawerLeft = false;
      }
    }
  }
</script>
<style>
  .v-navigation-drawer border {
    display: none
  }

  #toolbar, #toolbar i, #toolbar .title {
    color: white
  }

  .component-fade-enter-active, .component-fade-leave-active {
    transition: opacity .3s ease;
  }

  .component-fade-enter, .component-fade-leave-to
  /* .component-fade-leave-active below version 2.1.8 */ {
    opacity: 0;
  }

  p a[target="_blank"]:after,
  a:not( [href*='account/signout'] )[target="_blank"] .v-list__tile__title:after {
    font-family: 'Material Icons';
    content: " launch";
  }
</style>
