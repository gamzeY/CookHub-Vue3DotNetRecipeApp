<template>
  <v-container fluid class="pa-5 text-center" background-color="dark-grey">
    <h1>{{ mode === "edit" ? "Edit" : "Add" }} Recipe</h1>
    <v-sheet class="mx-auto" width="500">
      <v-form
        @submit.prevent="submitForm"
        densty="consistent"
        density="compact"
      >
        <v-text-field v-model="name" label="Recipe Name"></v-text-field>
        <v-text-field
          v-model="category"
          label="Category"
          :rules="[(v) => !!v || 'Category Id is required']"
        ></v-text-field>
        <v-textarea v-model="description" label="Description"></v-textarea>
        <v-text-field v-model="imageUrl" label="Image URL"></v-text-field>
        <v-text-field v-model="videoUrl" label="Video URL"></v-text-field>
        <v-textarea
          v-model="ingredients"
          label="Ingredients (separate with commas)"
        ></v-textarea>
        <v-text-field
          v-model="calories"
          label="Calories"
          type="number"
        ></v-text-field>
        <v-text-field v-model="area" label="Area"></v-text-field>
        <v-text-field
          v-model="carbs"
          label="Carbs"
          type="number"
        ></v-text-field>
        <v-text-field v-model="fat" label="Fat" type="number"></v-text-field>
        <v-text-field
          v-model="protein"
          label="Protein"
          type="number"
        ></v-text-field>
        <div class="text-right">
          <v-btn
            id="submit-btn"
            type="submit"
            color="primary"
            :disabled="!category"
            >{{ mode === "edit" ? "Update" : "Save" }} Recipe</v-btn
          >
        </div>
      </v-form>
    </v-sheet>
    <v-snackbar
      v-model="alert.show"
      :color="alert.type"
      timeout="2000"
      position="fixed"
      location="top"
    >
      {{ alert.message }}
    </v-snackbar>
  </v-container>
</template>
<script setup>
import { ref, onMounted } from "vue";
import store from "@/store";
import { useRouter, useRoute } from "vue-router";

const router = useRouter();

const route = useRoute();

const name = ref("");
const category = ref("");
const description = ref("");
const imageUrl = ref("");
const videoUrl = ref("");
const ingredients = ref("");
const calories = ref("");
const carbs = ref("");
const fat = ref("");
const protein = ref("");
const area = ref("");

const mode = route.params.mode || "add";

const alert = ref({ show: false, type: "", message: "" });
const isSubmitting = ref(false);

onMounted(async () => {
  if (mode === "edit") {
    const recipeId = route.params.id;
    try {
      const recipeData = await store.dispatch("getRecipeById", recipeId);
      name.value = recipeData.name;
      category.value = recipeData.categoryId;
      description.value = recipeData.description;
      imageUrl.value = recipeData.imageUrl;
      videoUrl.value = recipeData.videoUrl;
      ingredients.value = recipeData.ingredients.join(", ");
      calories.value = recipeData.calories;
      carbs.value = recipeData.carbs;
      fat.value = recipeData.fat;
      protein.value = recipeData.protein;
    } catch (error) {
      console.error("Error fetching recipe:", error);
    }
  }
});
const showAlert = (type, message) => {
  alert.value.type = type;
  alert.value.message = message;
  alert.value.show = true;
};
const submitForm = async () => {
  if (isSubmitting.value) return;
  isSubmitting.value = true;
  try {
    const ingredientsArray = ingredients.value
      .split(",")
      .map((item) => item.trim());
    const recipeData = {
      recipeId: route.params.id,
      name: name.value,
      categoryId: category.value,
      description: description.value,
      imageUrl: imageUrl.value,
      videoUrl: videoUrl.value,
      ingredients: ingredientsArray,
      calories: calories.value,
      carbs: carbs.value,
      fat: fat.value,
      protein: protein.value,
      area: area.value,
    };
    let response;
    if (mode === "edit") {
      recipeData.id = route.params.id;
      response = await store.dispatch("updateRecipe", recipeData);
    } else {
      response = await store.dispatch("addRecipe", recipeData);
    }
    if (response.status === 201 || response.status === 200) {
      showAlert("success", "Recipe saved successfully");
      setTimeout(() => {
        router.push({
          name: "RecipeDetails",
          params: { id: response.data.recipeId },
        });
      }, 2000);
    } else {
      showAlert("error", "Error saving recipe");
    }
  } catch (error) {
    console.error("Error submitting form:", error);
    showAlert("error", "Error submitting form. Please try again later.");
  }
  isSubmitting.value = false;
};
</script>
