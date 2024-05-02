import { createRouter, createWebHistory } from "vue-router";


const routes = [
  {
    path: "/",
    name: "Home",
    component: () => import("../views/Home.vue"),
  },
  {
    path: "/recipe/:id",
    name: "RecipeDetails",
    component: () => import("../views/RecipeDetails.vue"),
  },
  {
    path: "/add-recipe/:mode/:id?",
    name: "AddRecipe",
    component: () => import("../views/AddRecipe.vue"),
  }
 
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
