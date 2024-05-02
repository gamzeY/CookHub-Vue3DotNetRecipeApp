<template>
  <v-card>
    <v-data-iterator :items="ingredients" item-value="name" :search="search">
      <template v-slot:header>
        <v-toolbar class="px-2">
          <v-text-field
            v-model="search"
            density="comfortable"
            placeholder="Search"
            prepend-inner-icon="mdi-magnify"
            style="max-width: 300px"
            variant="solo"
            clearable
            hide-details
          ></v-text-field>
          <v-spacer></v-spacer>
          <v-btn color="primary" @click="addRecipe('add')"
            >Add recipe<v-icon>mdi-plus</v-icon></v-btn
          >
        </v-toolbar>
        <v-carousel
          height="400"
          show-arrows="hover"
          cycle
          hide-delimiter-background
        >
          <v-carousel-item
            v-for="(slide, i) in ingredients.slice(3, 6)"
            :key="i"
          >
            <v-sheet height="100%">
              <div class="d-flex fill-height justify-center align-center">
                <img
                  :src="slide.imageUrl"
                  :alt="slide.name"
                  class="bg-grey-lighten-2"
                  max-height="125"
                  @click="router.push({ name: 'RecipeDetails', params: { id: slide.recipeId } })"
                />
                <div class="caption">
                  {{ slide.name }}
                </div>
              </div>
            </v-sheet>
          </v-carousel-item>
        </v-carousel>
      </template>
   
      <template v-slot:default="{ items, isExpanded, toggleExpand }">
      <div class="d-flex align-center bg-grey-lighten-2 pa-2 text-h6">
        <h3>Popular recipe collections...</h3>
      </div>
        <v-container class="pa-2" fluid>
          <v-row dense>
            <v-col
              v-for="item in items"
              :key="item.raw.recipeId"
              cols="auto"
              md="4"
            >
              <v-card class="pb-1" width="300" flat>
                <v-img :src="item.raw.imageUrl" :alt="item.raw.name"></v-img>
                <v-list-item :subtitle="item.raw.name" class="mb-2">
                  <template v-slot:title>
                    <strong class="text-h6 mb-2">{{ item.raw.name }}</strong>
                  </template>
                </v-list-item>

                <div class="pl-2 d-flex justify-space-between">
                  <!-- V-Switch component -->
                  <v-switch
                    :label="`${isExpanded(item) ? 'Hide' : 'Show'} details`"
                    :model-value="isExpanded(item)"
                    density="compact"
                    inset
                    @click="() => toggleExpand(item)"
                  ></v-switch>

                  <v-btn
                    class="text-none ml-1"
                    size="small"
                    text="Read"
                    flat
                    color="primary"
                    @click="() => openRecipe(item.raw)"
                  ></v-btn>
                </div>
                <v-divider></v-divider>
                <v-expand-transition>
                  <div v-if="isExpanded(item)">
                    <v-list :lines="false" density="compact">
                      <v-list-item
                        :title="`🔥 Calories: ${item.raw.calories}`"
                        active
                      ></v-list-item>
                      <v-list-item
                        :title="`🍔 Fat: ${item.raw.fat}`"
                      ></v-list-item>
                      <v-list-item
                        :title="`🍞 Carbs: ${item.raw.carbs}`"
                      ></v-list-item>
                      <v-list-item
                        :title="`🍗 Protein: ${item.raw.protein}`"
                      ></v-list-item>
                    </v-list>
                  </div>
                </v-expand-transition>
              </v-card>
            </v-col>
          </v-row>
        </v-container>
      </template>
    </v-data-iterator>
  </v-card>
</template>

<script setup>
import { computed, onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import store from "../store";
import list from "../axios";

const router = useRouter();

const ingredients = computed(() => store.state.searchedRecipes);
const search = ref("");

const openRecipe = (item) => {
  router.push({ name: "RecipeDetails", params: { id: item.recipeId } });
};
const addRecipe = (mode) => {
  router.push({ name: "AddRecipe", params: { mode: mode } });
};
onMounted(() => {
  list.get(`recipes`).then((response) => {
    store.commit("setRecipes", response.data);
  });
});
</script>
<style scoped>
.caption {
  position: absolute;
  bottom: 10px; 
  transform: translateX(-50%);
  color: white; 
  background-color: rgba(0, 0, 0, 0.5);
  padding: 5px 10px;
  border-radius: 5px;
}
</style>
