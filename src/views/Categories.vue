<template>
  <div class="categories-container">
    <v-card class="categories-card" elevation="4">
      <v-card-title class="categories-title">
        <v-icon left size="large" color="primary">mdi-food-fork-drink</v-icon>
        sdasd
      </v-card-title>
      
      <v-card-text class="categories-subtitle">
        Explore recipes by category and discover new culinary adventures
      </v-card-text>
      
      <v-list class="categories-list">
        <v-list-item
          v-for="category in categories"
          :key="category.categoryId"
          @click="selectCategory(category.categoryId)"
          class="category-item"
          :class="{ 'category-item-hover': true }"
        >
          <template v-slot:prepend>
            <v-avatar 
              :color="getCategoryColor(category.categoryId)" 
              size="48"
              class="category-avatar"
            >
              <v-icon size="24" color="white">
                {{ getCategoryIcon(category.categoryId) }}
              </v-icon>
            </v-avatar>
          </template>
          
          <v-list-item-title class="category-name">
            <span class="category-letter">{{ getCategoryLetter(category.categoryId) }}</span>
            <span class="category-abbreviation">{{ getCategoryAbbreviation(category.categoryId) }}</span>
          </v-list-item-title>
          
          <template v-slot:append>
            <v-icon color="primary" class="category-arrow">
              mdi-chevron-right
            </v-icon>
          </template>
        </v-list-item>
      </v-list>
      
      <v-card-actions class="categories-actions">
        <v-btn
          color="primary"
          variant="outlined"
          @click="goToHome"
          class="back-btn"
        >
          <v-icon left>mdi-arrow-left</v-icon>
          Back to Home
        </v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import list from '../axios';
import store from '@/store';

const categories = ref([]);
const router = useRouter();

onMounted(async () => {
  try {
    const response = await list.get('/category');
    categories.value = response.data || [];
  } catch (error) {
    console.error('Error fetching categories:', error);
    // Fallback to default categories if API fails
    categories.value = [
      { categoryId: 1, name: 'Pasta & Italian', description: 'Delicious pasta dishes' },
      { categoryId: 2, name: 'Salads & Fresh', description: 'Healthy and fresh salads' },
      { categoryId: 3, name: 'Desserts & Sweets', description: 'Sweet treats and desserts' },
      { categoryId: 4, name: 'Main Dishes', description: 'Main course meals' },
      { categoryId: 5, name: 'Appetizers & Snacks', description: 'Starters and snacks' }
    ];
  }
});
const selectCategory = (categoryId) => {
  // Navigate to home with category filter
  router.push({ name: 'Home', query: { category: categoryId } });
  
  // Fetch recipes by category
  list.get(`recipes/byCategory/${categoryId}`).then((response) => {
  console.log('Recipes fetched for category:', categoryId, response.data);
    store.commit('setRecipes', response.data);
  }).catch((error) => {
    console.error('Error fetching recipes by category:', error);
  });
};

const goToHome = () => {
  router.push({ name: 'Home' });
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

const getCategoryIcon = (categoryId) => {
  const icons = {
    1: "mdi-pasta",
    2: "mdi-food-salad", 
    3: "mdi-cake-variant",
    4: "mdi-food-steak",
    5: "mdi-food-apple"
  };
  return icons[categoryId] || "mdi-food";
};

const getCategoryLetter = (categoryId) => {
  const letters = {
    1: "P",
    2: "S", 
    3: "D",
    4: "N",
    5: "A"
  };
  return letters[categoryId] || "O";
};

const getCategoryAbbreviation = (categoryId) => {
  const abbreviations = {
    1: "I..",
    2: "F.", 
    3: "S.",
    4: "M",
    5: "S."
  };
  return abbreviations[categoryId] || "..";
};
</script>

<style scoped>
.categories-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.categories-card {
  border-radius: 16px;
  overflow: hidden;
}

.categories-title {
  font-size: 2rem;
  font-weight: 700;
  color: #1976d2;
  padding: 24px 24px 8px;
  display: flex;
  align-items: center;
}

.categories-subtitle {
  font-size: 1.1rem;
  color: #666;
  padding: 0 24px 24px;
  line-height: 1.5;
}

.categories-list {
  padding: 0;
}

.category-item {
  padding: 16px 24px;
  margin: 8px 16px;
  border-radius: 12px;
  transition: all 0.3s ease;
  cursor: pointer;
  border: 2px solid transparent;
}

.category-item:hover {
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  transform: translateX(8px);
  border-color: #1976d2;
  box-shadow: 0 4px 12px rgba(25, 118, 210, 0.15);
}

.category-avatar {
  margin-right: 16px;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.category-name {
  display: flex;
  align-items: center;
  gap: 8px;
}

.category-letter {
  font-size: 1.2rem;
  font-weight: 700;
  color: #333;
}

.category-abbreviation {
  font-size: 0.9rem;
  color: #666;
  font-weight: 400;
}

.category-arrow {
  transition: transform 0.3s ease;
}

.category-item:hover .category-arrow {
  transform: translateX(4px);
}

.categories-actions {
  padding: 24px;
  justify-content: center;
}

.back-btn {
  font-weight: 600;
  text-transform: none;
  letter-spacing: 0.5px;
  border-radius: 8px;
}

@media (max-width: 768px) {
  .categories-container {
    padding: 10px;
  }
  
  .categories-title {
    font-size: 1.5rem;
    padding: 16px 16px 8px;
  }
  
  .categories-subtitle {
    padding: 0 16px 16px;
  }
  
  .category-item {
    margin: 4px 8px;
    padding: 12px 16px;
  }
  
  .category-letter {
    font-size: 1.1rem;
  }
}
</style>
