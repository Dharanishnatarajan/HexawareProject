"use client"
import { useState } from "react"
import { useNavigate, Link } from "react-router-dom"
import { registerUser } from "../../api/authService"
import { toast } from "react-hot-toast"
import "./Reg.css"

const Register = () => {
  const navigate = useNavigate()
  const [loading, setLoading] = useState(false)
  const [form, setForm] = useState({
    username: "",
    password: "",
    name: "",
    email: "",
    role: "User",
    gender: "",
    address: "",
  })

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)

    try {
      await registerUser(form)
      toast.success("Registration successful! You are now logged in.")
      if (form.role === "User") navigate("/home")
      else if (form.role === "Restaurant") navigate("/restaurant/create")
    } catch (err) {
      toast.error(err.message || "Registration failed")
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="register-container">
      <div className="register-background">
        
      </div>

      <div className="register-card">
        <div className="register-header">
          <div className="brand-section">
            <div className="brand-icon">🍲</div>
            <h2 className="brand-title">Join HotPot</h2>
          </div>
          <p className="register-subtitle">Create your account and start your food journey</p>
        </div>

        <form onSubmit={handleSubmit} className="register-form">
          <div className="row g-3">
            <div className="col-md-6">
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-person me-1"></i>Username
                </label>
                <input
                  name="username"
                  type="text"
                  placeholder="Choose a username"
                  className="form-control custom-input"
                  required
                  onChange={handleChange}
                />
              </div>
            </div>

            <div className="col-md-6">
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-lock me-1"></i>Password
                </label>
                <input
                  name="password"
                  type="password"
                  placeholder="Create a password"
                  className="form-control custom-input"
                  required
                  onChange={handleChange}
                />
              </div>
            </div>

            <div className="col-md-6">
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-person-badge me-1"></i>Full Name
                </label>
                <input
                  name="name"
                  type="text"
                  placeholder="Your full name"
                  className="form-control custom-input"
                  required
                  onChange={handleChange}
                />
              </div>
            </div>

            <div className="col-md-6">
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-envelope me-1"></i>Email
                </label>
                <input
                  name="email"
                  type="email"
                  placeholder="your@email.com"
                  className="form-control custom-input"
                  required
                  onChange={handleChange}
                />
              </div>
            </div>

            <div className="col-md-6">
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-gender-ambiguous me-1"></i>Gender
                </label>
                <select name="gender" className="form-select custom-input" required onChange={handleChange}>
                  <option value="">Select Gender</option>
                  <option value="Male">Male</option>
                  <option value="Female">Female</option>
                  <option value="Other">Other</option>
                </select>
              </div>
            </div>

            <div className="col-md-6">
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-person-check me-1"></i>Account Type
                </label>
                <select name="role" className="form-select custom-input" onChange={handleChange}>
                  <option value="User">Customer</option>
                  <option value="Restaurant">Restaurant Owner</option>
                </select>
              </div>
            </div>

            <div className="col-12">
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-geo-alt me-1"></i>Address
                </label>
                <textarea
                  name="address"
                  placeholder="Your complete address"
                  className="form-control custom-input"
                  rows="3"
                  required
                  onChange={handleChange}
                />
              </div>
            </div>
          </div>

          <button type="submit" className="btn register-btn" disabled={loading}>
            {loading ? (
              <>
                <span className="spinner-border spinner-border-sm me-2"></span>
                Creating Account...
              </>
            ) : (
              <>
                <i className="bi bi-person-plus me-2"></i>
                Create Account
              </>
            )}
          </button>
        </form>

        <div className="register-footer">
          <p className="login-link">
            Already have an account?{" "}
            <Link to="/" className="link-primary">
              Sign in here
            </Link>
          </p>
        </div>
      </div>
    </div>
  )
}

export default Register
