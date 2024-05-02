<template>
  <v-container>
    <v-card>
      <v-toolbar class="px-2">
        <v-spacer></v-spacer>
        <v-btn color="primary" @click="editRecipe('edit')"
          >Edit recipe<v-icon>mdi-pencil</v-icon></v-btn
        >/
        <v-btn color="error" @click="deleteRecipe()"
          >Delete recipe<v-icon>mdi-delete</v-icon></v-btn
        >
      </v-toolbar>
      <div class="video-container">
        <iframe
          width="10%"
          height="200"
          :src="getEmbeddedVideoLink(recipe.videoUrl)"
          frameborder="0"
          allowfullscreen
        ></iframe>
      </div>
      <v-card-title>{{ recipe.name }}</v-card-title>
      <div class="d-flex justify-space-between">
        <v-card-subtitle>{{ recipe.area }}</v-card-subtitle>
        <v-chip size="small" color="pink" label v-if="recipe.tags">
          <v-icon icon="mdi-label" start></v-icon>
          <template v-for="(tag, index) in recipe.tags">
            <span>{{ tag }}</span>
            <span v-if="index < recipe.tags.length - 1">, </span>
          </template>
        </v-chip>
      </div>

      <v-card-text>
        <h3>Ingredients</h3>
        <ul>
          <li v-for="(ingredient, index) in recipe.ingredients" :key="index">
            {{ ingredient }}
          </li>
        </ul>
        <br />
        <h3>Instructions</h3>
        <p>{{ recipe.description }}</p>
      </v-card-text>
    </v-card>
    <v-dialog v-model="dialog" width="auto">
      <v-card>
        <v-card-title class="headline">Delete Recipe</v-card-title>
        <v-card-text>
          Are you sure you want to delete this recipe?
        </v-card-text>
        <v-card-actions>
          <v-btn color="primary" @click="dialog = false">Cancel</v-btn>
          <v-spacer></v-spacer>
          <v-btn color="error" @click="confirmDelete()">Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from "vue";
import list from "../axios";
import { useRoute, useRouter } from "vue-router";
import store from "../store";

const router = useRouter();

const $route = useRoute();

const recipe = ref({});
const descriptions = ref([]);
const dialog = ref(false);

const recipeId = $route.params.id;

onMounted(async () => {
  try {
    const response = await list.get(`recipes/${$route.params.id}`);
    const meal = response.data;
    recipe.value = meal;
    descriptions.value = meal.description;
  } catch (error) {
    console.error("Error fetching recipe:", error);
  }
});

const editRecipe = (mode) => {
  console.log(mode);
  router.push({
    name: "AddRecipe",
    params: { mode: mode, id: $route.params.id },
  });
};
const confirmDelete = async () => {
  try {
    await store.dispatch("deleteRecipe", recipeId);
    router.push("/"); // Redirect to recipes page after deletion
    dialog.value = false;
  } catch (error) {
    console.error("Error deleting recipe:", error);
  }
};
const deleteRecipe = async () => {
  dialog.value = true;
};

// Function to convert YouTube URL to embedded format
const getEmbeddedVideoLink = (youtubeUrl) => {
  if (!youtubeUrl) return "";
  const videoId = youtubeUrl.split("v=")[1];
  return `https://www.youtube.com/embed/${videoId}`;
};
</script>

<style>
.video-container {
  position: relative;
  padding-bottom: 36.25%; /* 16:9 aspect ratio */
  height: 0;
  overflow: hidden;
}

.video-container iframe {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
}

ul {
  list-style-type: none;
  padding: 0;
}
</style>
