<template>
  <div class="recipe-details-container">
    <!-- Back Button -->
    <div class="back-section">
      <v-btn
        color="primary"
        variant="text"
        @click="goBack"
        class="back-btn"
      >
        <v-icon left>mdi-arrow-left</v-icon>
        Back to Recipes
      </v-btn>
    </div>

    <!-- Main Recipe Card -->
    <v-card class="recipe-card" elevation="4">
      <!-- Recipe Image -->
      <v-img
        :src="recipe.imageUrl"
        :alt="recipe.name"
        height="400"
        cover
        class="recipe-hero-image"
      >
        <template v-slot:placeholder>
          <div class="d-flex align-center justify-center fill-height">
            <v-progress-circular indeterminate color="primary"></v-progress-circular>
          </div>
        </template>
      </v-img>

      <!-- Recipe Header -->
      <div class="recipe-header">
        <div class="recipe-title-section">
          <h1 class="recipe-title">{{ recipe.name }}</h1>
          <p class="recipe-description">{{ recipe.description }}</p>
          
          <div class="recipe-meta">
            <v-chip
              :color="getCategoryColor(recipe.categoryId)"
              size="small"
              class="category-chip"
            >
              {{ getCategoryName(recipe.categoryId) }}
            </v-chip>
            
            <div class="nutrition-summary">
              <span class="nutrition-item">
                <v-icon size="small" color="orange">mdi-fire</v-icon>
                {{ recipe.calories }} cal
              </span>
              <span class="nutrition-item">
                <v-icon size="small" color="red">mdi-food-steak</v-icon>
                {{ recipe.protein }}g protein
              </span>
            </div>
          </div>
        </div>

        <!-- Action Buttons -->
        <div class="recipe-actions">
          <v-btn
            color="primary"
            variant="outlined"
            @click="editRecipe('edit')"
            class="action-btn"
          >
            <v-icon left>mdi-pencil</v-icon>
            Edit Recipe
          </v-btn>
          
          <v-btn
            color="error"
            variant="outlined"
            @click="deleteRecipe()"
            class="action-btn"
          >
            <v-icon left>mdi-delete</v-icon>
            Delete Recipe
          </v-btn>
        </div>
      </div>

      <!-- Recipe Content -->
      <div class="recipe-content">
        <v-row>
          <!-- Ingredients Section -->
          <v-col cols="12" md="6">
            <v-card class="ingredients-card" elevation="2">
              <v-card-title class="section-title">
                <v-icon left color="green">mdi-food-apple</v-icon>
                Ingredients
              </v-card-title>
              <v-card-text>
                <v-list class="ingredients-list">
                  <v-list-item
                    v-for="(ingredient, index) in recipe.ingredients"
                    :key="index"
                    class="ingredient-item"
                  >
                    <template v-slot:prepend>
                      <v-icon color="green" size="small">mdi-check-circle</v-icon>
                    </template>
                    <v-list-item-title>{{ ingredient }}</v-list-item-title>
                  </v-list-item>
                </v-list>
              </v-card-text>
            </v-card>
          </v-col>

          <!-- Instructions Section -->
          <v-col cols="12" md="6">
            <v-card class="instructions-card" elevation="2">
              <v-card-title class="section-title">
                <v-icon left color="blue">mdi-format-list-numbered</v-icon>
                Instructions
              </v-card-title>
              <v-card-text>
                <div class="instructions-text">
                  {{ recipe.instructions }}
                </div>
              </v-card-text>
            </v-card>
          </v-col>
        </v-row>

        <!-- Nutrition Information -->
        <v-card class="nutrition-card mt-6" elevation="2">
          <v-card-title class="section-title">
            <v-icon left color="orange">mdi-chart-line</v-icon>
            Nutrition Information
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="6" sm="3">
                <div class="nutrition-box">
                  <div class="nutrition-icon">🔥</div>
                  <div class="nutrition-value">{{ recipe.calories }}</div>
                  <div class="nutrition-label">Calories</div>
                </div>
              </v-col>
              <v-col cols="6" sm="3">
                <div class="nutrition-box">
                  <div class="nutrition-icon">🍔</div>
                  <div class="nutrition-value">{{ recipe.fat }}g</div>
                  <div class="nutrition-label">Fat</div>
                </div>
              </v-col>
              <v-col cols="6" sm="3">
                <div class="nutrition-box">
                  <div class="nutrition-icon">🍞</div>
                  <div class="nutrition-value">{{ recipe.carbs }}g</div>
                  <div class="nutrition-label">Carbs</div>
                </div>
              </v-col>
              <v-col cols="6" sm="3">
                <div class="nutrition-box">
                  <div class="nutrition-icon">🍗</div>
                  <div class="nutrition-value">{{ recipe.protein }}g</div>
                  <div class="nutrition-label">Protein</div>
                </div>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </div>
    </v-card>

    <!-- Delete Confirmation Dialog -->
    <v-dialog v-model="dialog" max-width="400">
      <v-card class="delete-dialog">
        <v-card-title class="delete-title">
          <v-icon left color="error">mdi-alert-circle</v-icon>
          Delete Recipe
        </v-card-title>
        <v-card-text class="delete-text">
          Are you sure you want to delete "{{ recipe.name }}"? This action cannot be undone.
        </v-card-text>
        <v-card-actions class="delete-actions">
          <v-btn
            color="grey"
            variant="outlined"
            @click="dialog = false"
            class="cancel-btn"
          >
            Cancel
          </v-btn>
          <v-spacer></v-spacer>
          <v-btn
            color="error"
            @click="confirmDelete()"
            class="confirm-btn"
          >
            <v-icon left>mdi-delete</v-icon>
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
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

const goBack = () => {
  router.push({ name: 'Home' });
};

const editRecipe = (mode) => {
  console.log(mode);
  router.push({
    name: "AddRecipe",
    params: { mode: mode, id: $route.params.id },
  });
};

const confirmDelete = async () => {
  try {
    const response = await store.dispatch("deleteRecipe", recipeId);
    if (response.status === 204) {
      router.push("/");
      dialog.value = false;
    } else {
      console.error("Unexpected response status:", response.status);
    }
  } catch (error) {
    console.error("Error deleting recipe:", error);
    console.error("Error details:", error.response?.data);
  }
};

const deleteRecipe = async () => {
  dialog.value = true;
};

const getCategoryName = (categoryId) => {
  const categories = {
    1: "Pasta",
    2: "Salads", 
    3: "Desserts",
    4: "Main Dishes",
    5: "Appetizers"
  };
  return categories[categoryId] || "Other";
};

const getCategoryColor = (categoryId) => {
  const colors = {
    1: "orange",
    2: "green", 
    3: "pink",
    4: "blue",
    5: "purple"
  };
  return colors[categoryId] || "grey";
};
</script>

<style scoped>
.recipe-details-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
}

.back-section {
  margin-bottom: 20px;
}

.back-btn {
  font-weight: 600;
  text-transform: none;
  letter-spacing: 0.5px;
}

.recipe-card {
  border-radius: 16px;
  overflow: hidden;
}

.recipe-hero-image {
  position: relative;
}

.recipe-header {
  padding: 32px;
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
}

.recipe-title-section {
  margin-bottom: 24px;
}

.recipe-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #333;
  margin-bottom: 12px;
  line-height: 1.2;
}

.recipe-description {
  font-size: 1.1rem;
  color: #666;
  line-height: 1.6;
  margin-bottom: 20px;
}

.recipe-meta {
  display: flex;
  align-items: center;
  gap: 16px;
  flex-wrap: wrap;
}

.category-chip {
  font-weight: 600;
}

.nutrition-summary {
  display: flex;
  gap: 16px;
}

.nutrition-item {
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 0.9rem;
  color: #666;
  font-weight: 500;
}

.recipe-actions {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
}

.action-btn {
  font-weight: 600;
  text-transform: none;
  letter-spacing: 0.5px;
  border-radius: 8px;
}

.recipe-content {
  padding: 32px;
}

.section-title {
  font-size: 1.3rem;
  font-weight: 600;
  color: #333;
  display: flex;
  align-items: center;
  padding-bottom: 16px;
}

.ingredients-card,
.instructions-card,
.nutrition-card {
  border-radius: 12px;
  height: 100%;
}

.ingredients-list {
  padding: 0;
}

.ingredient-item {
  padding: 8px 0;
  border-bottom: 1px solid #f0f0f0;
}

.ingredient-item:last-child {
  border-bottom: none;
}

.instructions-text {
  font-size: 1rem;
  line-height: 1.7;
  color: #555;
  white-space: pre-line;
}

.nutrition-box {
  text-align: center;
  padding: 16px 8px;
  border-radius: 8px;
  background: #f8f9fa;
  transition: transform 0.2s ease;
}

.nutrition-box:hover {
  transform: translateY(-2px);
}

.nutrition-icon {
  font-size: 2rem;
  margin-bottom: 8px;
}

.nutrition-value {
  font-size: 1.2rem;
  font-weight: 700;
  color: #1976d2;
  margin-bottom: 4px;
}

.nutrition-label {
  font-size: 0.8rem;
  color: #666;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

.delete-dialog {
  border-radius: 12px;
}

.delete-title {
  font-size: 1.3rem;
  font-weight: 600;
  color: #d32f2f;
  display: flex;
  align-items: center;
}

.delete-text {
  font-size: 1rem;
  line-height: 1.5;
  color: #666;
}

.delete-actions {
  padding: 16px 24px 24px;
}

.cancel-btn,
.confirm-btn {
  font-weight: 600;
  text-transform: none;
  letter-spacing: 0.5px;
  border-radius: 8px;
}

@media (max-width: 768px) {
  .recipe-details-container {
    padding: 10px;
  }
  
  .recipe-header {
    padding: 20px;
  }
  
  .recipe-title {
    font-size: 2rem;
  }
  
  .recipe-content {
    padding: 20px;
  }
  
  .recipe-meta {
    flex-direction: column;
    align-items: flex-start;
  }
  
  .recipe-actions {
    flex-direction: column;
  }
  
  .action-btn {
    width: 100%;
  }
}
</style>
