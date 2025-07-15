"use client"
import { useNavigate } from "react-router-dom"
import "./AdminNavbar.css"

const AdminNavbar = () => {
  const navigate = useNavigate()

  const handleLogout = () => {
    localStorage.clear()
    navigate("/")
  }

  return (
    <nav className="admin-navbar">
      <div className="container-fluid">
        <div className="d-flex justify-content-between align-items-center py-3">
          {/* Brand Section */}
          <div className="admin-brand-section" onClick={() => navigate("/admin/dashboard")}>
            <div className="admin-brand-icon">
              <i className="bi bi-gear-fill"></i>
            </div>
            <div className="admin-brand-text">
              <h4 className="admin-brand-name mb-0">Admin Portal</h4>
              <small className="admin-brand-tagline">System Management</small>
            </div>
          </div>

          {/* Navigation Buttons */}
          <div className="admin-nav-buttons">
            <button className="admin-nav-btn admin-btn-dashboard" onClick={() => navigate("/admin/dashboard")}>
              <i className="bi bi-speedometer2 me-2"></i>
              <span>Dashboard</span>
            </button>

            <button className="admin-nav-btn admin-btn-logout" onClick={handleLogout}>
              <i className="bi bi-box-arrow-right me-2"></i>
              <span>Logout</span>
            </button>
          </div>
        </div>
      </div>
    </nav>
  )
}

export default AdminNavbar
