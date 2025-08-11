import list from "../axios";

export function getRecipes({ commit }, keyword) {
  console.log("aaa", keyword);
  list.get(`byCategory/${keyword}`).then((response) => {
    commit("setRecipes", response.data);
  });
}

export async function addRecipe({ commit }, recipe) {
  return await list.post(`recipes`, recipe);
}

export async function updateRecipe({ commit }, recipe) {
  console.log("Updating recipe:", recipe);
  const response = await list.put(`recipes/${recipe.recipeId}`, recipe);
  console.log("Update response:", response);
  return response;
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
    const response = await list.delete(`recipes/${id}`);
    commit("setRecipes", response.data);
    return response.data;
  } catch (error) {
    console.error("Error deleting recipe:", error);
    throw error;
  }
}
