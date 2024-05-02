<template>
  <v-layout>
    <v-app-bar color="primary" dense permanent>
      <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title><v-btn text color="white"  @click="$router.push({ name: 'Home' })">Cook Hub</v-btn></v-toolbar-title>
      <v-toolbar-items>
        <v-btn icon>
          <v-icon>mdi-plus</v-icon>
        </v-btn>
      </v-toolbar-items>
    </v-app-bar>

    <v-navigation-drawer location="left" v-model="drawer">
      <Categories />
    </v-navigation-drawer>

    <v-navigation-drawer location="right">
      <v-list>
        <v-list-item> 
      </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-main>
      <router-view />
    </v-main>
  </v-layout>
</template>

<script setup>
import { computed, ref } from "vue";
import store from "./store";
import Categories from "./views/Categories.vue";

const drawer = ref(true);
const recipes = computed(() => store.state.searchedRecipes);
</script>
