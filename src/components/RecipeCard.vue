<template>
  <v-card 
    class="recipe-card mx-auto" 
    width="100%" 
    max-width="350"
    height="400"
    elevation="4"
    hover
    @click="handleCardClick"
  >
    <v-img 
      :src="recipe.imageUrl" 
      :alt="recipe.name"
      height="200"
      cover
      class="recipe-image"
    >
      <template v-slot:placeholder>
        <div class="d-flex align-center justify-center fill-height">
          <v-progress-circular indeterminate color="primary"></v-progress-circular>
        </div>
      </template>
      <template v-slot:error>
        <div class="d-flex align-center justify-center fill-height recipe-fallback">
          <div class="fallback-content">
            <v-icon size="48" color="grey">mdi-food</v-icon>
            <div class="fallback-text">{{ recipe.name }}</div>
          </div>
        </div>
      </template>
    </v-img>
    
    <v-card-title class="recipe-title">
      <div class="d-flex align-center justify-space-between">
        <span>{{ recipe.name }}</span>
        <v-btn
          :color="isLiked ? 'red' : 'grey'"
          variant="text"
          size="small"
          @click.stop="handleLikeToggle"
          class="like-btn"
        >
          <v-icon left size="small">
            {{ isLiked ? 'mdi-heart' : 'mdi-heart-outline' }}
          </v-icon>
          {{ likeCount }}
        </v-btn>
      </div>
    </v-card-title>
    
    <v-card-text class="recipe-description">
      {{ recipe.description }}
    </v-card-text>

    <v-card-actions class="recipe-actions">
      <v-chip
        :color="getCategoryColor(recipe.categoryId)"
        size="small"
        class="category-chip"
      >
        {{ getCategoryName(recipe.categoryId) }}
      </v-chip>
      
      <v-spacer></v-spacer>
      
      <v-btn
        color="primary"
        variant="text"
        size="small"
        @click.stop="handleReadMore"
        class="read-btn"
      >
        Read More
        <v-icon right size="small">mdi-arrow-right</v-icon>
      </v-btn>
    </v-card-actions>


  </v-card>
</template>

<script setup>
import { ref, computed } from 'vue';
import { useCategories } from '../composables/useCategories';

const props = defineProps({
  recipe: {
    type: Object,
    required: true
  },
  initialLikeCount: {
    type: Number,
    default: 0
  },
  initialIsLiked: {
    type: Boolean,
    default: false
  }
});

const emit = defineEmits([
  'recipe-click',
  'read-more',
  'like-toggle'
]);

// Local state
const likeCount = ref(props.initialLikeCount);
const isLiked = ref(props.initialIsLiked);

// Composables
const { getCategoryColor, getCategoryFullName: getCategoryName } = useCategories();

// Computed properties
const recipeTitle = computed(() => {
  return props.recipe.name.toUpperCase();
});

const handleCardClick = () => {
  emit('recipe-click', {
    recipeId: props.recipe.recipeId,
    recipeName: props.recipe.name,
    timestamp: new Date().toISOString()
  });
};

const handleReadMore = () => {
  emit('read-more', {
    recipe: props.recipe,
    action: 'read-more'
  });
};

const handleLikeToggle = () => {
  isLiked.value = !isLiked.value;
  likeCount.value += isLiked.value ? 1 : -1;
  
  emit('like-toggle', {
    recipeId: props.recipe.recipeId,
    isLiked: isLiked.value,
    newLikeCount: likeCount.value,
    userAction: isLiked.value ? 'liked' : 'unliked'
  });
};


</script>

<style scoped>
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
  padding: 0 16px 8px;
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

.like-btn {
  font-weight: 600;
  text-transform: none;
  letter-spacing: 0.3px;
}


</style>
