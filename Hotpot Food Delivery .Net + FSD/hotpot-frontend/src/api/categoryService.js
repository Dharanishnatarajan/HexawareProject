import axios from './axios'

export const getAllMenuCategories = async () => {
  const res = await axios.get('/menu/categories')
  return res.data
}
