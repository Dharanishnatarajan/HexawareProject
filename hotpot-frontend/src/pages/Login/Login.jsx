"use client"
import { useState } from "react"
import { useNavigate, Link } from "react-router-dom"
import { loginUser } from "../../api/authService"
import { toast } from "react-hot-toast"
import "./Login.css"

const Login = () => {
  const [username, setUsername] = useState("")
  const [password, setPassword] = useState("")
  const [loading, setLoading] = useState(false)
  const [showLoginForm, setShowLoginForm] = useState(false)
  const navigate = useNavigate()

  const handleLogin = async (e) => {
    e.preventDefault()
    setLoading(true)

    try {
      const result = await loginUser(username, password)
      localStorage.setItem("token", result.token)
      localStorage.setItem("role", result.role)
      localStorage.setItem("username", result.username)
      localStorage.setItem("userId", result.userId)

      if (result.role === "Restaurant" && result.restaurantId) {
        localStorage.setItem("restaurantId", result.restaurantId)
      }

      toast.success("Login successful!")

      if (result.role === "User") navigate("/home")
      else if (result.role === "Restaurant") navigate("/restaurant/dashboard")
      else if (result.role === "Admin") navigate("/admin/dashboard")
    } catch (err) {
      toast.error(err.message || "Login failed")
    } finally {
      setLoading(false)
    }
  }

  const handleGetStarted = () => {
    setShowLoginForm(true)
  }

  const handleBackToWelcome = () => {
    setShowLoginForm(false)
    setUsername("")
    setPassword("")
  }

  if (!showLoginForm) {
    // Welcome Page
    return (
      <div className="login-container">
        <div className="welcome-card">
          <div className="welcome-header">
            <div className="brand-logo">
              <div className="logo-icon">🍔</div>
              <h1 className="brand-name">HotPot</h1>
            </div>
            <h2 className="welcome-title">Welcome to HotPot</h2>
            <p className="welcome-subtitle">Your favorite food delivery platform</p>
            <p className="welcome-description">
              Order delicious meals from top restaurants and get them delivered to your doorstep. Join thousands of food
              lovers who trust HotPot for their daily dining needs.
            </p>
          </div>

          <div className="welcome-features">
            <div className="feature-item">
              <span className="feature-icon">🏪</span>
              <span>Multiple Restaurants</span>
            </div>
            <div className="feature-item">
              <span className="feature-icon">⚡</span>
              <span>Fast Delivery</span>
            </div>
            <div className="feature-item">
              <span className="feature-icon">❤️</span>
              <span>Quality Food</span>
            </div>
          </div>

          <button className="get-started-btn" onClick={handleGetStarted}>
            Get Started
          </button>

          <div className="welcome-footer">
            <p className="register-link">
              New to HotPot?{" "}
              <Link to="/register" className="link-primary">
                Create an account
              </Link>
            </p>
          </div>
        </div>
      </div>
    )
  }

  // Login Form
  return (
    <div className="login-container">
      <div className="login-card">
        <div className="login-header">
          <button className="back-btn" onClick={handleBackToWelcome}>
            ← Back
          </button>
          <div className="brand-logo">
            <div className="logo-icon">🍔</div>
            <h1 className="brand-name">HotPot</h1>
          </div>
          <p className="login-subtitle">Welcome back! Please sign in to continue</p>
        </div>

        <form onSubmit={handleLogin} className="login-form">
          <div className="form-group">
            <input
              type="text"
              className="form-input"
              placeholder="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>

          <div className="form-group">
            <input
              type="password"
              className="form-input"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>

          <button type="submit" className="login-btn" disabled={loading}>
            {loading ? "Signing in..." : "Sign In"}
          </button>
        </form>

        <div className="login-footer">
          <p className="register-link">
            Don't have an account?{" "}
            <Link to="/register" className="link-primary">
              Create one here
            </Link>
          </p>
        </div>
      </div>
    </div>
  )
}

export default Login
