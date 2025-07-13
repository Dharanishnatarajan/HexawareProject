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

  return (
    <div className="login-container">
      <div className="login-background">
        <div className="floating-shapes">
          <div className="shape shape-1"></div>
          <div className="shape shape-2"></div>
          <div className="shape shape-3"></div>
          <div className="shape shape-4"></div>
        </div>
      </div>

      <div className="login-card">
        <div className="login-header">
          <div className="brand-logo">
            <div className="logo-icon">🍔</div>
            <h2 className="brand-name">HotPot</h2>
          </div>
          <p className="login-subtitle">Welcome back! Please sign in to continue</p>
        </div>

        <form onSubmit={handleLogin} className="login-form">
          <div className="form-group">
            <div className="input-wrapper">
              <input
                type="text"
                className="form-control custom-input"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                required
              />
            </div>
          </div>

          <div className="form-group">
            <div className="input-wrapper">
              <input
                type="password"
                className="form-control custom-input"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
          </div>

          <button type="submit" className="btn login-btn" disabled={loading}>
            {loading ? (
              <>
                <span className="spinner-border spinner-border-sm me-2"></span>
                Signing in...
              </>
            ) : (
              <>
                <i className="bi bi-box-arrow-in-right me-2"></i>
                Sign In
              </>
            )}
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
