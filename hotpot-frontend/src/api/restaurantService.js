import axios from './axios'

const authHeader = () => ({
  headers: {
    Authorization: `Bearer ${localStorage.getItem('token')}`,
  },
})
export const getMyRestaurantDetails = async () => {
  const res = await axios.get("/restaurant/profile")
  return res.data
}

export const getMyMenuItems = () =>
  axios.get('/menu/my', authHeader()).then(res => res.data)

export const toggleMenuAvailability = (id, isAvailable) =>
  axios.patch(`/menu/${id}/availability`, { isAvailable }, authHeader())

export const createMenuItem = (formData) =>
  axios.post('/menu', formData, authHeader())

export const updateMenuItem = (id, formData) =>
  axios.put(`/menu/${id}`, formData, authHeader())

export const deleteMenuItem = (id) =>
  axios.delete(`/menu/${id}`, authHeader())

export const getAllMenuCategories = () =>
  axios.get('/menu/categories', authHeader()).then(res => res.data)

export const getRestaurantOrders = () =>
  axios.get('/orders/restaurant/orders', authHeader()).then(res => res.data)

export const updateOrderStatus = (orderId, status) =>
  axios.patch(`/orders/${orderId}/status`, { status }, authHeader())
