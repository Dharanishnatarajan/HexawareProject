"use client"
import { useEffect, useState } from "react"
import RestaurantNavbar from "../../components/RestaurantNavbar"
import {
  getMyMenuItems,
  createMenuItem,
  updateMenuItem,
  deleteMenuItem,
  getAllMenuCategories,
  toggleMenuAvailability,
  getMyRestaurantDetails
} from "../../api/restaurantService"
import { toast } from "react-hot-toast"
import "./RestaurantDashboard.css"

const RestaurantDashboard = () => {
  const [menuItems, setMenuItems] = useState([])
  const [categories, setCategories] = useState([])
  const [restaurant, setRestaurant] = useState(null)
  const [form, setForm] = useState({
    name: "",
    price: "",
    description: "",
    menuCategoryId: "",
  })
  const [imageFile, setImageFile] = useState(null)
  const [editId, setEditId] = useState(null)
  const [isLoading, setIsLoading] = useState(false)

  useEffect(() => {
    loadMenu()
    loadCategories()
    loadRestaurant()
  }, [])

  const loadMenu = async () => {
    try {
      const data = await getMyMenuItems()
      setMenuItems(data)
    } catch {
      toast.error("Failed to fetch menu items")
    }
  }

  const loadCategories = async () => {
    try {
      const data = await getAllMenuCategories()
      setCategories(data)
    } catch {
      toast.error("Failed to fetch categories")
    }
  }

  const loadRestaurant = async () => {
    try {
      const data = await getMyRestaurantDetails()
      setRestaurant(data)
    } catch {
      toast.error("Failed to fetch restaurant info")
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setIsLoading(true)
    const data = new FormData()
    data.append("Name", form.name)
    data.append("Price", form.price)
    data.append("Description", form.description || "")
    data.append("MenuCategoryId", Number.parseInt(form.menuCategoryId))
    if (imageFile) {
      data.append("ImageFile", imageFile)
    }

    try {
      if (editId) {
        await updateMenuItem(editId, data)
        toast.success("✅ Menu item updated successfully!")
      } else {
        await createMenuItem(data)
        toast.success("✅ Menu item created successfully!")
      }
      resetForm()
      loadMenu()
    } catch {
      toast.error("❌ Failed to save menu item")
    } finally {
      setIsLoading(false)
    }
  }

  const handleEdit = (item) => {
    setEditId(item.id)
    setForm({
      name: item.name,
      price: item.price,
      description: item.description,
      menuCategoryId: item.menuCategoryId?.toString() || "",
    })
    setImageFile(null)
    window.scrollTo({ top: 0, behavior: "smooth" })
  }

  const handleDelete = async (id) => {
    if (window.confirm("Are you sure you want to delete this menu item?")) {
      try {
        await deleteMenuItem(id)
        toast.success("🗑️ Menu item deleted successfully")
        loadMenu()
      } catch {
        toast.error("❌ Failed to delete menu item")
      }
    }
  }

  const handleToggleAvailability = async (item) => {
    try {
      await toggleMenuAvailability(item.id, !item.isAvailable)
      toast.success(
        `${item.name} is now marked as ${!item.isAvailable ? "available" : "unavailable"}`
      )
      loadMenu()
    } catch {
      toast.error("❌ Failed to update availability status")
    }
  }

  const resetForm = () => {
    setForm({ name: "", price: "", description: "", menuCategoryId: "" })
    setImageFile(null)
    setEditId(null)
  }

  const getCategoryName = (categoryId) => {
    return categories.find((cat) => cat.id === categoryId)?.name || "Unknown"
  }

  const stats = {
    totalItems: menuItems.length,
    availableItems: menuItems.filter(item => item.isAvailable).length,
    unavailableItems: menuItems.filter(item => !item.isAvailable).length,
    categories: categories.length
  }

  return (
    <>
      <RestaurantNavbar />
      
      <div className="dashboard-container">
        {/* Restaurant Header */}
        {restaurant && (
  <div className="restaurant-header">
    <div className="container">
      <div className="restaurant-info-card">
        <div className="restaurant-icon">
          <i className="bi bi-shop"></i>
        </div>
        <div className="restaurant-details">
          <h2 className="restaurant-name">{restaurant.name}</h2>
          <p className="restaurant-description">
            <i className="bi bi-info-circle me-2"></i>
            {restaurant.description || "No description provided."}
          </p>
          <p className="restaurant-address">
            <i className="bi bi-geo-alt me-2"></i>
            {restaurant.location}
          </p>
          <p className="restaurant-contact">
            <i className="bi bi-telephone me-2"></i>
            {restaurant.contactNumber}
          </p>
        </div>
        <div className="restaurant-badge">
          <span className="badge-label">Restaurant ID</span>
          <span className="badge-value">#{restaurant.id}</span>
        </div>
      </div>
    </div>
  </div>
)}


        <div className="main-content">
          <div className="container">
            {/* Stats Cards */}
            <div className="stats-section">
              <div className="row g-4 mb-5">
                <div className="col-lg-3 col-md-6">
                  <div className="stat-card stat-card-primary">
                    <div className="stat-icon">
                      <i className="bi bi-list-ul"></i>
                    </div>
                    <div className="stat-content">
                      <h3 className="stat-number">{stats.totalItems}</h3>
                      <p className="stat-label">Total Items</p>
                    </div>
                  </div>
                </div>
                <div className="col-lg-3 col-md-6">
                  <div className="stat-card stat-card-success">
                    <div className="stat-icon">
                      <i className="bi bi-check-circle"></i>
                    </div>
                    <div className="stat-content">
                      <h3 className="stat-number">{stats.availableItems}</h3>
                      <p className="stat-label">Available</p>
                    </div>
                  </div>
                </div>
                <div className="col-lg-3 col-md-6">
                  <div className="stat-card stat-card-warning">
                    <div className="stat-icon">
                      <i className="bi bi-x-circle"></i>
                    </div>
                    <div className="stat-content">
                      <h3 className="stat-number">{stats.unavailableItems}</h3>
                      <p className="stat-label">Unavailable</p>
                    </div>
                  </div>
                </div>
                <div className="col-lg-3 col-md-6">
                  <div className="stat-card stat-card-info">
                    <div className="stat-icon">
                      <i className="bi bi-tags"></i>
                    </div>
                    <div className="stat-content">
                      <h3 className="stat-number">{stats.categories}</h3>
                      <p className="stat-label">Categories</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            {/* Add/Edit Form */}
            <div className="form-section">
              <div className="form-card">
                <div className="form-header">
                  <div className="form-header-content">
                    <div className="form-icon">
                      <i className={`bi ${editId ? "bi-pencil-square" : "bi-plus-circle"}`}></i>
                    </div>
                    <div className="form-title">
                      <h4>{editId ? "Edit Menu Item" : "Add New Menu Item"}</h4>
                      <p>{editId ? "Update your menu item details" : "Create a new delicious item for your menu"}</p>
                    </div>
                  </div>
                </div>
                <div className="form-body">
                  <form onSubmit={handleSubmit}>
                    <div className="row g-4">
                      <div className="col-md-6">
                        <div className="form-group">
                          <label className="form-label">
                            <i className="bi bi-card-text me-2"></i>
                            Item Name *
                          </label>
                          <input
                            type="text"
                            className="form-control modern-input"
                            placeholder="e.g., Margherita Pizza"
                            value={form.name}
                            onChange={(e) => setForm({ ...form, name: e.target.value })}
                            required
                          />
                        </div>
                      </div>
                      <div className="col-md-6">
                        <div className="form-group">
                          <label className="form-label">
                            <i className="bi bi-currency-rupee me-2"></i>
                            Price (₹) *
                          </label>
                          <input
                            type="number"
                            className="form-control modern-input"
                            placeholder="299"
                            value={form.price}
                            onChange={(e) => setForm({ ...form, price: e.target.value })}
                            required
                          />
                        </div>
                      </div>
                      <div className="col-md-6">
                        <div className="form-group">
                          <label className="form-label">
                            <i className="bi bi-tags me-2"></i>
                            Category *
                          </label>
                          <select
                            className="form-select modern-input"
                            value={form.menuCategoryId}
                            onChange={(e) => setForm({ ...form, menuCategoryId: e.target.value })}
                            required
                          >
                            <option value="">Select a category</option>
                            {categories.map((c) => (
                              <option key={c.id} value={c.id}>
                                {c.name}
                              </option>
                            ))}
                          </select>
                        </div>
                      </div>
                      <div className="col-md-6">
                        <div className="form-group">
                          <label className="form-label">
                            <i className="bi bi-image me-2"></i>
                            Item Image
                          </label>
                          <input
                            type="file"
                            className="form-control modern-input"
                            accept="image/*"
                            onChange={(e) => setImageFile(e.target.files[0])}
                          />
                          {imageFile && (
                            <div className="file-preview">
                              <i className="bi bi-check-circle text-success me-2"></i>
                              {imageFile.name}
                            </div>
                          )}
                        </div>
                      </div>
                      <div className="col-12">
                        <div className="form-group">
                          <label className="form-label">
                            <i className="bi bi-text-paragraph me-2"></i>
                            Description
                          </label>
                          <textarea
                            className="form-control modern-input"
                            rows="3"
                            placeholder="Brief description of your delicious item..."
                            value={form.description}
                            onChange={(e) => setForm({ ...form, description: e.target.value })}
                          />
                        </div>
                      </div>
                      <div className="col-12">
                        <div className="form-actions">
                          <button 
                            className="btn btn-primary modern-btn" 
                            type="submit" 
                            disabled={isLoading}
                          >
                            {isLoading ? (
                              <>
                                <span className="spinner-border spinner-border-sm me-2"></span>
                                Processing...
                              </>
                            ) : (
                              <>
                                <i className={`bi ${editId ? "bi-check-lg" : "bi-plus-lg"} me-2`}></i>
                                {editId ? "Update Item" : "Add Item"}
                              </>
                            )}
                          </button>
                          {editId && (
                            <button 
                              type="button" 
                              className="btn btn-secondary modern-btn" 
                              onClick={resetForm}
                            >
                              <i className="bi bi-x-lg me-2"></i>
                              Cancel
                            </button>
                          )}
                        </div>
                      </div>
                    </div>
                  </form>
                </div>
              </div>
            </div>

            {/* Menu Items Table */}
            <div className="table-section">
              <div className="table-card">
                <div className="table-header">
                  <div className="table-title">
                    <div className="table-icon">
                      <i className="bi bi-list-ul"></i>
                    </div>
                    <div>
                      <h4>Menu Items ({menuItems.length})</h4>
                      <p>Manage your restaurant menu items</p>
                    </div>
                  </div>
                </div>
                <div className="table-body">
                  {menuItems.length === 0 ? (
                    <div className="empty-state">
                      <div className="empty-icon">
                        <i className="bi bi-basket"></i>
                      </div>
                      <h5>No menu items yet</h5>
                      <p>Start by adding your first delicious item to the menu above.</p>
                    </div>
                  ) : (
                    <div className="table-responsive">
                      <table className="table modern-table">
                        <thead>
                          <tr>
                            <th>Image</th>
                            <th>Item Details</th>
                            <th>Category</th>
                            <th>Price</th>
                            <th>Status</th>
                            <th>Actions</th>
                          </tr>
                        </thead>
                        <tbody>
                          {menuItems.map((item) => (
                            <tr key={item.id}>
                              <td>
                                <div className="item-image">
                                  {item.imageUrl ? (
                                    <img
                                      src={item.imageUrl || "/placeholder.svg"}
                                      alt={item.name}
                                      className="menu-image"
                                    />
                                  ) : (
                                    <div className="image-placeholder">
                                      <i className="bi bi-image"></i>
                                    </div>
                                  )}
                                </div>
                              </td>
                              <td>
                                <div className="item-details">
                                  <h6 className="item-name">{item.name}</h6>
                                  <p className="item-description">{item.description || "No description"}</p>
                                </div>
                              </td>
                              <td>
                                <span className="category-badge">
                                  {getCategoryName(item.menuCategoryId)}
                                </span>
                              </td>
                              <td>
                                <span className="price-tag">₹{item.price}</span>
                              </td>
                              <td>
                                <div className="availability-toggle">
                                  <div className="form-check form-switch">
                                    <input
                                      className="form-check-input"
                                      type="checkbox"
                                      checked={item.isAvailable}
                                      onChange={() => handleToggleAvailability(item)}
                                    />
                                    <label className="form-check-label">
                                      <span className={`status-badge ${item.isAvailable ? 'status-available' : 'status-unavailable'}`}>
                                        {item.isAvailable ? "Available" : "Unavailable"}
                                      </span>
                                    </label>
                                  </div>
                                </div>
                              </td>
                              <td>
                                <div className="action-buttons">
                                  <button
                                    className="btn btn-sm btn-outline-primary action-btn"
                                    onClick={() => handleEdit(item)}
                                    title="Edit Item"
                                  >
                                    <i className="bi bi-pencil"></i>
                                  </button>
                                  <button
                                    className="btn btn-sm btn-outline-danger action-btn"
                                    onClick={() => handleDelete(item.id)}
                                    title="Delete Item"
                                  >
                                    <i className="bi bi-trash"></i>
                                  </button>
                                </div>
                              </td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  )
}

export default RestaurantDashboard
