import axios from './axios'

export const getRestaurantOrders = async (restaurantId) => {
  const res = await axios.get(`/orders/restaurant/${restaurantId}`)
  return res.data
}

export const placeOrder = async (address, phoneNumber) => {
  const res = await axios.post('/orders', { address, phoneNumber })
  return res.data
}

export const getUserOrders = async () => {
  const res = await axios.get('/orders/user')  // ✅ Removed userId from URL
  return res.data
}