<template>
  <div class="home-container">
    <!-- Hero Section with Carousel -->
    <v-card class="hero-section mb-6" elevation="0">
      <v-carousel
        height="500"
        show-arrows="hover"
        cycle
        hide-delimiter-background
        class="hero-carousel"
      >
        <v-carousel-item
          v-for="(slide, i) in ingredients.slice(0, 3)"
          :key="i"
        >
          <v-sheet height="100%" class="hero-slide">
            <div class="d-flex fill-height justify-center align-center position-relative">
              <img
                :src="slide.imageUrl"
                :alt="slide.name"
                class="hero-image"
                @click="router.push({ name: 'RecipeDetails', params: { id: slide.recipeId } })"
              />
              <div class="hero-overlay">
                <div class="hero-content">
                  <h1 class="hero-title">{{ slide.name }}</h1>
                  <p class="hero-subtitle">{{ slide.description }}</p>
                  <v-btn
                    color="white"
                    variant="outlined"
                    size="large"
                    @click="openRecipe(slide)"
                    class="hero-btn"
                  >
                    View Recipe
                  </v-btn>
                </div>
              </div>
            </div>
          </v-sheet>
        </v-carousel-item>
      </v-carousel>
    </v-card>

    <!-- Search and Add Section -->
    <v-card class="search-section mb-6" elevation="2">
      <v-toolbar class="px-4 py-3" color="white">
        <v-text-field
          v-model="search"
          density="comfortable"
          placeholder="Search for delicious recipes..."
          prepend-inner-icon="mdi-magnify"
          style="max-width: 400px"
          variant="outlined"
          clearable
          hide-details
          class="search-field"
        ></v-text-field>
        <v-spacer></v-spacer>
        <v-btn 
          color="primary" 
          size="large"
          @click="addRecipe('add')"
          class="add-recipe-btn"
          elevation="2"
        >
          <v-icon left>mdi-plus</v-icon>
          Add Recipe
        </v-btn>
      </v-toolbar>
    </v-card>


    <!-- Recipe Grid -->
    <v-card class="recipe-section" elevation="0">
      <div class="section-header mb-4">
        <h2 class="section-title">Popular Recipe Collections</h2>
        <p class="section-subtitle">Discover amazing recipes from around the world</p>
      </div>
      
      <v-data-iterator :items="ingredients" item-value="name" :search="search">
        <template v-slot:default="{ items, isExpanded, toggleExpand }">
          <v-container class="pa-0" fluid>
            <v-row dense>
              <v-col
                v-for="item in items"
                :key="item.raw.recipeId"
                cols="12"
                sm="6"
                md="4"
                lg="3"
              >

                <RecipeCard
                  :recipe="item.raw"
                  :initial-like-count="getRecipeLikeCount(item.raw.recipeId)"
                  :initial-is-liked="isRecipeLiked(item.raw.recipeId)"
                  @recipe-click="handleRecipeClick"
                  @read-more="handleReadMore"
                  @like-toggle="handleLikeToggle"

                />
              </v-col>
            </v-row>
          </v-container>
        </template>
      </v-data-iterator>
    </v-card>
  </div>
</template>

<script setup>
import { computed, ref, watch, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import store from "../store";
import list from "../axios";
import { useCategories } from "../composables/useCategories";
import RecipeCard from "../components/RecipeCard.vue";

const router = useRouter();
const route = useRoute();

const ingredients = computed(() => {
  return store.state.searchedRecipes;
});
const search = ref("");

const recipeLikes = ref(new Map());
const recipeLikedStatus = ref(new Map());
const userInteractions = ref([]);

// Load likes from localStorage on component mount
const loadLikesFromStorage = () => {
  try {
    const storedLikes = localStorage.getItem('recipeLikes');
    const storedLikedStatus = localStorage.getItem('recipeLikedStatus');
    
    if (storedLikes) {
      const likesData = JSON.parse(storedLikes);
      recipeLikes.value = new Map(Object.entries(likesData));
    }
    
    if (storedLikedStatus) {
      const likedStatusData = JSON.parse(storedLikedStatus);
      recipeLikedStatus.value = new Map(Object.entries(likedStatusData));
    }
  } catch (error) {
    if (import.meta.env.DEV) {
      console.error('Error loading likes from localStorage:', error);
    }
  }
};

const saveLikesToStorage = () => {
  try {
    const likesData = Object.fromEntries(recipeLikes.value);
    const likedStatusData = Object.fromEntries(recipeLikedStatus.value);
    
    localStorage.setItem('recipeLikes', JSON.stringify(likesData));
    localStorage.setItem('recipeLikedStatus', JSON.stringify(likedStatusData));
  } catch (error) {
    if (import.meta.env.DEV) {
      console.error('Error saving likes to localStorage:', error);
    }
  }
};

const openRecipe = (item) => {
  router.push({ name: "RecipeDetails", params: { id: item.recipeId } });
};

const addRecipe = (mode) => {
  router.push({ name: "AddRecipe", params: { mode: mode } });
};

const getRecipeLikeCount = (recipeId) => {
  return recipeLikes.value.get(recipeId) || 0;
};

const isRecipeLiked = (recipeId) => {
  return recipeLikedStatus.value.get(recipeId) || false;
};

// Event handlers for RecipeCard component emits
const handleRecipeClick = (eventData) => {
  // Only log in development mode
  if (import.meta.env.DEV) {
    console.log('Recipe clicked:', eventData);
  }
  
  userInteractions.value.push({
    type: 'recipe-click',
    data: eventData,
    timestamp: new Date()
  });
 
  router.push({ name: "RecipeDetails", params: { id: eventData.recipeId } });
};

const handleReadMore = (eventData) => {
  if (import.meta.env.DEV) {
    console.log('Read more clicked:', eventData);
  }
  
  userInteractions.value.push({
    type: 'read-more',
    data: eventData,
    timestamp: new Date()
  });
  
  router.push({ name: "RecipeDetails", params: { id: eventData.recipe.recipeId } });
};

const handleLikeToggle = (eventData) => {
  if (import.meta.env.DEV) {
    console.log('Like toggled:', eventData);
  }
  
  // Update local state
  recipeLikes.value.set(eventData.recipeId, eventData.newLikeCount);
  recipeLikedStatus.value.set(eventData.recipeId, eventData.isLiked);
  

  saveLikesToStorage();
  
  userInteractions.value.push({
    type: 'like-toggle',
    data: eventData,
    timestamp: new Date()
  });
};


onMounted(() => {
  loadLikesFromStorage();
  
  list.get('Category').then((response) => {
    store.commit("setCategories", response.data);
  }).catch((error) => {
    console.error('Error fetching categories:', error);
  });
});

const { getCategoryColor, getCategoryFullName: getCategoryName } = useCategories();

// Watch for route changes to handle category filtering
watch(() => route.query.category, (newCategory) => {
  if (newCategory) {
    // Fetch recipes by category
    list.get(`recipes/byCategory/${newCategory}`).then((response) => {
      store.commit("setRecipes", response.data);
    }).catch((error) => {
      console.error('Error fetching recipes by category:', error);
    });
  } else {
    list.get(`recipes`).then((response) => {
      store.commit("setRecipes", response.data);
    }).catch((error) => {
      console.error('Error fetching all recipes:', error);
    });
  }
}, { immediate: true });

</script>

<style scoped>
.home-container {
  max-width: 1400px;
  margin: 0 auto;
  padding: 20px;
}

.hero-section {
  border-radius: 16px;
  overflow: hidden;
}

.hero-carousel {
  border-radius: 16px;
}

.hero-slide {
  position: relative;
}

.hero-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  cursor: pointer;
  transition: transform 0.3s ease;
}

.hero-image:hover {
  transform: scale(1.05);
}

.hero-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(135deg, rgba(0,0,0,0.7) 0%, rgba(0,0,0,0.3) 100%);
  display: flex;
  align-items: center;
  justify-content: center;
}

.hero-content {
  text-align: center;
  color: white;
  padding: 40px;
}

.hero-title {
  font-size: 3rem;
  font-weight: 700;
  margin-bottom: 16px;
  text-shadow: 2px 2px 4px rgba(0,0,0,0.5);
}

.hero-subtitle {
  font-size: 1.2rem;
  margin-bottom: 24px;
  opacity: 0.9;
}

.hero-btn {
  font-weight: 600;
  text-transform: none;
  letter-spacing: 0.5px;
}

.search-section {
  border-radius: 12px;
}

.search-field {
  border-radius: 8px;
}

.add-recipe-btn {
  border-radius: 8px;
  text-transform: none;
  font-weight: 600;
  letter-spacing: 0.5px;
}

.recipe-section {
  border-radius: 12px;
}

.section-header {
  text-align: center;
  padding: 20px 0;
}

.section-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #1976d2;
  margin-bottom: 8px;
}

.section-subtitle {
  font-size: 1.1rem;
  color: #666;
  margin: 0;
}

.recipe-card {
  border-radius: 16px;
  transition: all 0.3s ease;
  cursor: pointer;
  display: flex;
  flex-direction: column;
}

.recipe-card:hover {
  transform: translateY(-8px);
  box-shadow: 0 12px 24px rgba(0,0,0,0.15);
}

.recipe-image {
  border-radius: 16px 16px 0 0;
  flex-shrink: 0;
}

.recipe-fallback {
  background-color: #f5f5f5; 
  border-radius: 16px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 20px;
}

.fallback-content {
  text-align: center;
  color: #666;
}

.fallback-text {
  margin-top: 10px;
  font-size: 0.9rem;
  font-weight: 500;
}

.recipe-title {
  font-size: 1.3rem;
  font-weight: 600;
  color: #333;
  line-height: 1.3;
  padding-bottom: 8px;
  flex-shrink: 0;
}

.recipe-description {
  color: #666;
  font-size: 0.95rem;
  line-height: 1.5;
  padding-bottom: 16px;
  flex-grow: 1;
  overflow: hidden;
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
}

.recipe-actions {
  padding: 0 16px 16px;
  flex-shrink: 0;
}

.category-chip {
  font-weight: 500;
}

.read-btn {
  font-weight: 600;
  text-transform: none;
  letter-spacing: 0.3px;
}

.nutrition-section {
  background: #f8f9fa;
}

.nutrition-content {
  padding: 20px;
}

.nutrition-title {
  font-size: 1.1rem;
  font-weight: 600;
  color: #333;
  text-align: center;
}

.nutrition-item {
  text-align: center;
  padding: 8px;
}

.nutrition-icon {
  font-size: 1.5rem;
  margin-bottom: 4px;
}

.nutrition-value {
  font-size: 1.1rem;
  font-weight: 600;
  color: #1976d2;
  margin-bottom: 2px;
}

.nutrition-label {
  font-size: 0.8rem;
  color: #666;
  text-transform: uppercase;
  letter-spacing: 0.5px;
}

@media (max-width: 768px) {
  .home-container {
    padding: 10px;
  }
  
  .hero-title {
    font-size: 2rem;
  }
  
  .section-title {
    font-size: 2rem;
  }
  
  .recipe-card {
    margin-bottom: 20px;
  }
}
</style>
