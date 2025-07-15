import axios from './axios'

// Get all menu items
export const getAllMenuItems = async () => {
  const res = await axios.get('/menu')
  return res.data
}

// Search menu items by location
export const getMenuByLocation = async (location) => {
  const res = await axios.get(`/menu/search/location?location=${location}`)
  return res.data
}

// Get menu items by restaurant name
export const getMenuByRestaurantName = async (name) => {
  const res = await axios.get('/menu')
  return res.data.filter(item => item.restaurant?.name?.toLowerCase() === name.toLowerCase())
}


// Get menu items by category name
export const getMenuByCategoryName = async (name) => {
  const res = await axios.get('/menu')
  return res.data.filter(item => item.menuCategory?.name?.toLowerCase() === name.toLowerCase())
}

// Get all restaurants
export const getAllRestaurants = async () => {
  const res = await axios.get('/restaurant')
  return res.data
}

// Get all categories
export const getAllCategories = async () => {
  const res = await axios.get('/menu/categories')
  return res.data
}
