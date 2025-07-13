import axios from './axios'

export const loginUser = async (username, password) => {
  try {
    const res = await axios.post('/auth/login', {
      username,
      password
    })
    return res.data // returns { token, role, username, userId }
  } catch (err) {
    throw err.response?.data || { message: 'Login failed' }
  }
}

export const registerUser = async (data) => {
  try {
    const res = await axios.post('/auth/register', data)
    localStorage.setItem('token', res.data.token)
    localStorage.setItem('role', res.data.role)
    localStorage.setItem('username', res.data.username)
    localStorage.setItem('userId', res.data.userId)
    return res.data
  } catch (err) {
    throw err.response?.data || { message: 'Registration failed' }
  }
}
