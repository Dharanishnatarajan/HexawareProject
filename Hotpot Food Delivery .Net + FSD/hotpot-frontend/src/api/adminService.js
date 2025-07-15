import axios from './axios'

// USERS
export const getAllUsers = () => axios.get('/admin/users').then(res => res.data)
export const updateUser = (id, data) => axios.put(`/admin/users/${id}`, data)
export const deleteUser = (id) => axios.delete(`/admin/users/${id}`)

// RESTAURANTS
export const getAllRestaurants = () => axios.get('/admin/restaurants').then(res => res.data)
export const updateRestaurant = (id, data) => axios.put(`/restaurant/${id}`, data)
export const deleteRestaurant = (id) => axios.delete(`/admin/restaurants/${id}`)
