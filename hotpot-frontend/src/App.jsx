import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'
import Login from './pages/Login/Login'
import Register from './pages/Login/Reg'
import Home from './pages/User/UserHome'
import Cart from './pages/User/Cart'
import AdminDashboard from "./pages/Admin/AdminDashboard";
import MyOrders from './pages/User/MyOrders'
import CreateRestaurant from './pages/Login/CreateRestaurant'
import RestaurantDashboard from './pages/Restaurant/RestaurantDashboard'
import RestaurantOrders from './pages/Restaurant/RestaurantOrders'
import ProtectedRoute from './components/ProtectedRoute'
import Footer from './components/Footer'

function App() {
  return (
    <Router>
      <div className="flex flex-col min-h-screen">
        <main className="flex-grow">
          <Routes>
            {/* Public Routes */}
            <Route path="/" element={<Login />} />
            <Route path="/register" element={<Register />} />

            {/* User Routes */}
            <Route
              path="/home"
              element={
                <ProtectedRoute role="User">
                  <Home />
                </ProtectedRoute>
              }
            />
            <Route
              path="/cart"
              element={
                <ProtectedRoute role="User">
                  <Cart />
                </ProtectedRoute>
              }
            />
            <Route
              path="/my-orders"
              element={
                <ProtectedRoute role="User">
                  <MyOrders />
                </ProtectedRoute>
              }
            />

            {/* Restaurant Routes */}
            <Route
              path="/restaurant/create"
              element={
                <ProtectedRoute role="Restaurant">
                  <CreateRestaurant />
                </ProtectedRoute>
              }
            />
            <Route
              path="/restaurant/dashboard"
              element={
                <ProtectedRoute role="Restaurant">
                  <RestaurantDashboard />
                </ProtectedRoute>
              }
            />
            <Route
              path="/restaurant/orders"
              element={
                <ProtectedRoute role="Restaurant">
                  <RestaurantOrders />
                </ProtectedRoute>
              }
            />

            {/* Admin Routes */}
            <Route
              path="/admin/dashboard"
              element={
                <ProtectedRoute role="Admin">
                  <AdminDashboard />
                </ProtectedRoute>
              }
            />

            {/* 404 fallback */}
            <Route
              path="*"
              element={
                <h1 className="text-center text-danger mt-5 fw-bold">
                  404 - Page Not Found
                </h1>
              }
            />
          </Routes>
        </main>

        {/* Always visible footer */}
        <Footer />
      </div>
    </Router>
  )
}

export default App
