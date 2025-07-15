"use client"
import { useNavigate } from "react-router-dom"
import "./UserNavbar.css"

const UserNavbar = () => {
  const navigate = useNavigate()

  const handleLogout = () => {
    localStorage.clear()
    navigate("/")
  }

  return (
    <nav className="user-navbar">
      <div className="container-fluid">
        <div className="d-flex justify-content-between align-items-center py-3">
          {/* Brand Section */}
          <div className="brand-section" onClick={() => navigate("/home")}>
            <div className="brand-icon">
            <div className="logo-icon">🍔</div>
            </div>
            <div className="brand-text">
              <h4 className="brand-name mb-0">HotPot</h4>
              <small className="brand-tagline">Food Delivery </small>
            </div>
          </div>

          {/* Navigation Buttons */}
          <div className="nav-buttons d-none d-lg-flex">
            <button className="nav-btn nav-btn-home" onClick={() => navigate("/home")}>
              <i className="bi bi-house-door nav-btn-icon"></i>
              <span>Home</span>
            </button>

            <button className="nav-btn nav-btn-orders" onClick={() => navigate("/my-orders")}>
              <i className="bi bi-receipt nav-btn-icon"></i>
              <span>My Orders</span>
            </button>

            <button className="nav-btn nav-btn-cart" onClick={() => navigate("/cart")}>
              <i className="bi bi-cart3 nav-btn-icon"></i>
              <span>Cart</span>
            </button>

            <button className="nav-btn nav-btn-logout" onClick={handleLogout}>
              <i className="bi bi-box-arrow-right nav-btn-icon"></i>
              <span>Logout</span>
            </button>
       

        
        </div></div>
      </div>
    </nav>
  )
}

export default UserNavbar
