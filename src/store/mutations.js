
export function setRecipes(state, recipes) {
  state.searchedRecipes = recipes; // save data inside state
}

export function addRecipe (state, recipe) {
  state.recipes.push(recipe);
  state.searchedRecipes.push(recipe);
}

