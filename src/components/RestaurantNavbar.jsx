"use client"
import { useNavigate } from "react-router-dom"
import "./RestaurantNavbar.css"

const RestaurantNavbar = () => {
  const navigate = useNavigate()

  const handleLogout = () => {
    localStorage.clear()
    navigate("/")
  }

  return (
    <nav className="restaurant-navbar">
      <div className="container-fluid">
        <div className="d-flex justify-content-between align-items-center py-3">
          {/* Brand Section */}
          <div className="brand-section d-flex align-items-center" onClick={() => navigate("/restaurant/dashboard")}>
            <div className="brand-icon">
              <i className="bi bi-shop"></i>
            </div>
            <div className="brand-text ms-3">
              <h4 className="brand-title mb-0">Restaurant Portal</h4>
              <small className="brand-subtitle">Management Dashboard</small>
            </div>
          </div>

          {/* Navigation Buttons */}
          <div className="nav-buttons d-flex gap-2">
            <button className="nav-btn nav-btn-primary" onClick={() => navigate("/restaurant/dashboard")}>
              <i className="bi bi-house-door me-2"></i>
              <span className="btn-text">Dashboard</span>
            </button>

            <button className="nav-btn nav-btn-warning" onClick={() => navigate("/restaurant/orders")}>
              <i className="bi bi-box-seam me-2"></i>
              <span className="btn-text">Orders</span>
            </button>

            <button className="nav-btn nav-btn-danger" onClick={handleLogout}>
              <i className="bi bi-box-arrow-right me-2"></i>
              <span className="btn-text">Logout</span>
            </button>
          </div>
        </div>
      </div>
    </nav>
  )
}

export default RestaurantNavbar
