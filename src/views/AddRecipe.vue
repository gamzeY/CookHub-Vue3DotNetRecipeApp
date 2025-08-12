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
        <v-text-field v-model="instructions" label="Instructions"></v-text-field>
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
const instructions = ref("");
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
      category.value = recipeData.categoryId.toString();
      description.value = recipeData.description;
      instructions.value = recipeData.instructions;
      imageUrl.value = recipeData.imageUrl;
      videoUrl.value = recipeData.videoUrl || "";
      ingredients.value = recipeData.ingredients.join(", ");
      calories.value = recipeData.calories.toString();
      carbs.value = recipeData.carbs.toString();
      fat.value = recipeData.fat.toString();
      protein.value = recipeData.protein.toString();
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
      name: name.value,
      categoryId: parseInt(category.value),
      description: description.value,
      instructions: instructions.value,
      imageUrl: imageUrl.value,
      videoUrl: videoUrl.value,
      ingredients: ingredientsArray,
      calories: parseInt(calories.value) || 0,
      carbs: parseFloat(carbs.value) || 0,
      fat: parseFloat(fat.value) || 0,
      protein: parseFloat(protein.value) || 0,
      area: area.value,
    };
    
    if (mode === "edit") {
      recipeData.recipeId = parseInt(route.params.id);
    }
    
    console.log('Sending recipe data:', recipeData);
    let response;
    if (mode === "edit") {
      response = await store.dispatch("updateRecipe", recipeData);
      if (response.status === 204) {
        showAlert("success", "Recipe updated successfully");
        setTimeout(() => {
          router.push({
            name: "RecipeDetails",
            params: { id: route.params.id },
          });
        }, 2000);
      } else {
        showAlert("error", "Error updating recipe");
      }
    } else {
      response = await store.dispatch("addRecipe", recipeData);
      console.log('Add recipe response in component:', response);
      if (response.status === 201) {
        showAlert("success", "Recipe created successfully");
        setTimeout(() => {
     
          router.push({ name: "Home" });
        }, 2000);
      } else {
        showAlert("error", "Error creating recipe");
      }
    }
      } catch (error) {
      
      console.error("Error response:", error.response);
    
      let errorMessage = "Error submitting form";
      if (error.response?.data) {
        if (typeof error.response.data === 'string') {
          errorMessage = error.response.data;
        } else if (error.response.data.message) {
          errorMessage = error.response.data.message;
        } else if (error.response.data.errors) {
          errorMessage = Object.values(error.response.data.errors).flat().join(', ');
        }
      } else if (error.message) {
        errorMessage = error.message;
      }
      
      showAlert("error", errorMessage);
    }
  isSubmitting.value = false;
};
</script>
