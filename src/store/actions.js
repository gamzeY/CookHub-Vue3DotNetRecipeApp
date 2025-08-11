import list from "../axios";

export function getRecipes({ commit }, keyword) {
  console.log("aaa", keyword);
  list.get(`byCategory/${keyword}`).then((response) => {
    commit("setRecipes", response.data);
  });
}

export async function addRecipe({ commit }, recipeData) {
  try {
    console.log('Sending recipe data to backend:', recipeData);
    const response = await list.post(`recipes`, recipeData);
    console.log('Add recipe response:', response.data);
    console.log('Response status:', response.status);
    
    // Backend'den dönen recipe'yi kontrol et
    let recipeToAdd;
    
    if (response.data && response.data.recipe) {
      recipeToAdd = response.data.recipe;
    } else if (response.data) {
      recipeToAdd = response.data;
    } else {
      recipeToAdd = null;
    }
    
    // Eğer backend'den gelen data boşsa veya eksikse, frontend verisini kullan
    if (!recipeToAdd || !recipeToAdd.name || recipeToAdd.name === "") {
      console.log('Backend returned empty data, using frontend data');
      recipeToAdd = {
        ...recipeData,
        recipeId: response.data?.recipeId || response.data?.recipe?.recipeId || Date.now()
      };
    }
    
    console.log('Adding recipe to store:', recipeToAdd);
    commit('addRecipe', recipeToAdd);
    
    return response;
  } catch (error) {
    console.error('Error adding recipe:', error);
    console.error('Error response:', error.response);
    console.error('Error status:', error.response?.status);
    console.error('Error data:', error.response?.data);
    throw error;
  }
}

export async function updateRecipe({ commit }, recipeData) {
  try {
    console.log("Updating recipe:", recipeData);
    const response = await list.put(`recipes/${recipeData.recipeId}`, recipeData);
    console.log("Update response:", response);
    return response;
  } catch (error) {
    console.error('Error updating recipe:', error);
    throw error;
  }
}

export async function getRecipeById({ commit }, id) {
  try {
    const response = await list.get(`recipes/${id}`);
    commit("setRecipes", response.data);
    return response.data;
  } catch (error) {
    console.error("Error getting recipe by ID:", error);
    throw error;
  }
}
export async function deleteRecipe({ commit }, id) {
  try {
    console.log("Deleting recipe with ID:", id);
    const response = await list.delete(`recipes/${id}`);
    console.log("Delete response:", response);
    // Don't commit response.data since DELETE returns 204 No Content
    return response;
  } catch (error) {
    console.error("Error deleting recipe:", error);
    throw error;
  }
}
