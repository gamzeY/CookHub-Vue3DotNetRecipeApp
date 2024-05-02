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
  return await list.put(`recipes/${recipe.id}`, recipe);
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
