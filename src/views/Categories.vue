<template>
  <v-card>
    <v-card-title>Categories</v-card-title>
    <v-list dense>
      <v-list-item
        v-for="category in categories"
        :key="category.categoryId"
        @click="selectCategory(category.categoryId)"
      >
        <v-list-item-title>{{ category.name }}</v-list-item-title>
      </v-list-item>
    </v-list>
  </v-card>
</template>

<script setup>
  import { ref,onMounted } from 'vue';
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
    }
  });

  const selectCategory = (category) => {
    if(router.currentRoute.value.params) {
      router.push({ name: 'Home', params: { id: category } });
    }
    const request = list.get(`recipes/byCategory/${category}`);
    request.then((response) => {
        store.commit('setRecipes', response.data)
    })
  };
</script>
