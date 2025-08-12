import { computed } from 'vue'
import store from '../store'

export function useCategories() {
  // Get categories from store
  const categories = computed(() => store.state.categories)

  // Find category by ID
  const findCategory = (categoryId) => {
    return categories.value.find(c => c.categoryId === categoryId)
  }

  // Get category color
  const getCategoryColor = (categoryId) => {
    const category = findCategory(categoryId)
    return category ? category.color : 'grey'
  }

  // Get category icon
  const getCategoryIcon = (categoryId) => {
    const category = findCategory(categoryId)
    return category ? category.icon : 'mdi-food'
  }

  // Get category letter
  const getCategoryLetter = (categoryId) => {
    const category = findCategory(categoryId)
    return category ? category.letter : 'O'
  }

  // Get category abbreviation
  const getCategoryAbbreviation = (categoryId) => {
    const category = findCategory(categoryId)
    return category ? category.abbreviation : '..'
  }

  // Get category full name
  const getCategoryFullName = (categoryId) => {
    const category = findCategory(categoryId)
    return category ? category.name : 'Unknown Category'
  }

  // Get category description
  const getCategoryDescription = (categoryId) => {
    const category = findCategory(categoryId)
    return category ? category.description : ''
  }

  return {
    categories,
    findCategory,
    getCategoryColor,
    getCategoryIcon,
    getCategoryLetter,
    getCategoryAbbreviation,
    getCategoryFullName,
    getCategoryDescription
  }
}
