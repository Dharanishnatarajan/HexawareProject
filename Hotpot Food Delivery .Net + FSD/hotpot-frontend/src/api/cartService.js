import axios from './axios'

export const addToCart = async (menuItemId, quantity = 1) => {
  return await axios.post('/cart/add', {
    menuItemId,
    quantity
  })
}

export const getCart = async () => {
  const res = await axios.get('/cart')
  return res.data
}

export const updateCartItem = (menuItemId, quantity) =>
  axios.put('/cart/update', {
    menuItemId,
    quantity
  })


export const removeCartItem = (menuItemId) =>
  axios.delete(`/cart/remove/${menuItemId}`)

