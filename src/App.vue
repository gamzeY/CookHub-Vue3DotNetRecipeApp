<template>
  <v-layout>
    <v-app-bar color="primary" dense permanent>
      <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title><v-btn text color="white"  @click="$router.push({ name: 'Home' })">Cook Hub</v-btn></v-toolbar-title>
      <v-spacer></v-spacer>
      <v-toolbar-items>
        <v-btn 
          text 
          color="white" 
          @click="$router.push({ name: 'AddRecipe', params: { mode: 'add' } })"
          class="mx-2"
        >
          <v-icon left>mdi-plus</v-icon>
          Add Recipe
        </v-btn>
      </v-toolbar-items>
    </v-app-bar>

    <v-navigation-drawer location="left" v-model="drawer" width="280">
      <v-list>
        <v-list-item @click="$router.push({ name: 'Home' })">
          <template v-slot:prepend>
            <v-icon>mdi-home</v-icon>
          </template>
          <v-list-item-title>Home</v-list-item-title>
        </v-list-item>
      </v-list>
      
      <v-divider></v-divider>
      
      <!-- Categories Section in Drawer -->
      <div class="categories-drawer-section">
        <v-list-subheader class="categories-header">
          <v-icon left size="small">mdi-food-fork-drink</v-icon>
          Recipe Categories
        </v-list-subheader>
        
        <v-list>
          <v-list-item
            v-for="category in categories"
            :key="category.categoryId"
            @click="selectCategory(category.categoryId)"
            class="category-drawer-item"
          >
            <template v-slot:prepend>
              <v-avatar 
                :color="getCategoryColor(category.categoryId)" 
                size="32"
                class="category-drawer-avatar"
              >
                <v-icon size="16" color="white">
                  {{ getCategoryIcon(category.categoryId) }}
                </v-icon>
              </v-avatar>
            </template>
            
            <v-list-item-title class="category-drawer-name">
              {{ getCategoryFullName(category.categoryId) }}
            </v-list-item-title>
          </v-list-item>
        </v-list>
      </div>
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
import { computed, ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import store from "./store";
import list from "./axios";

const drawer = ref(true);
const categories = ref([]);
const router = useRouter();
const recipes = computed(() => store.state.searchedRecipes);

onMounted(async () => {
  try {
    const response = await list.get('Category');
    categories.value = response.data || [];
    //save to store for other components
    store.commit('setCategories', response.data);
  } catch (error) {
    console.error('Error fetching categories:', error);
    categories.value = [];
  }
});

const selectCategory = (categoryId) => {
  // Navigate to home with category filter
  router.push({ name: 'Home', query: { category: categoryId } });
  
  // Fetch recipes by category
  list.get(`recipes/byCategory/${categoryId}`).then((response) => {
    store.commit('setRecipes', response.data);
  }).catch((error) => {
    console.error('Error fetching recipes by category:', error);
  });
};

const getCategoryColor = (categoryId) => {
  const category = categories.value.find(c => c.categoryId === categoryId);
  return category ? category.color : 'grey';
};

const getCategoryIcon = (categoryId) => {
  const category = categories.value.find(c => c.categoryId === categoryId);
  return category ? category.icon : 'mdi-food';
};

const getCategoryLetter = (categoryId) => {
  const category = categories.value.find(c => c.categoryId === categoryId);
  return category ? category.letter : 'O';
};

const getCategoryAbbreviation = (categoryId) => {
  const category = categories.value.find(c => c.categoryId === categoryId);
  return category ? category.abbreviation : '..';
};

const getCategoryFullName = (categoryId) => {
  const category = categories.value.find(c => c.categoryId === categoryId);
  return category ? category.name : 'Unknown Category';
};
</script>

<style scoped>
.categories-drawer-section {
  margin-top: 16px;
}

.categories-header {
  font-size: 0.9rem;
  font-weight: 600;
  color: #1976d2;
  padding: 16px 16px 8px;
  display: flex;
  align-items: center;
}

.category-drawer-item {
  padding: 12px 16px;
  margin: 4px 8px;
  border-radius: 8px;
  transition: all 0.3s ease;
  cursor: pointer;
  min-height: 48px;
}

.category-drawer-item:hover {
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  transform: translateX(4px);
  box-shadow: 0 2px 8px rgba(25, 118, 210, 0.1);
}

.category-drawer-avatar {
  margin-right: 12px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.1);
  flex-shrink: 0;
}

.category-drawer-name {
  font-size: 0.95rem;
  font-weight: 500;
  color: #333;
  line-height: 1.2;
}

.category-letter {
  font-size: 1rem;
  font-weight: 700;
  color: #333;
}

.category-abbreviation {
  font-size: 0.8rem;
  color: #666;
  font-weight: 400;
}
</style>
