"use client"
import { useEffect, useState } from "react"
import {
  getAllMenuItems,
  getMenuByLocation,
  getMenuByCategoryName,
  getMenuByRestaurantName,
  getAllCategories,
  getAllRestaurants,
} from "../../api/menuService"
import { addToCart } from "../../api/cartService"
import { toast } from "react-hot-toast"
import UserNavbar from "../../components/UserNavbar"
import "./UserHome.css"

const UserHome = () => {
  const [menu, setMenu] = useState([])
  const [location, setLocation] = useState("")
  const [restaurant, setRestaurant] = useState("")
  const [category, setCategory] = useState("")
  const [menuName, setMenuName] = useState("")
  const [allRestaurants, setAllRestaurants] = useState([])
  const [allCategories, setAllCategories] = useState([])
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    fetchMenu()
    fetchFilters()
  }, [])

  const fetchMenu = async () => {
    try {
      setLoading(true)
      const data = await getAllMenuItems()
      setMenu(data)
    } catch {
      toast.error("Failed to load menu")
    } finally {
      setLoading(false)
    }
  }

  const fetchFilters = async () => {
    try {
      const restaurantsRes = await getAllRestaurants()
      const categoriesRes = await getAllCategories()
      setAllRestaurants(restaurantsRes)
      setAllCategories(categoriesRes)
    } catch {
      toast.error("Failed to load filter options")
    }
  }

  const handleAddToCart = async (menuItemId) => {
    const item = menu.find((m) => m.id === menuItemId)
    if (!item?.isAvailable) {
      toast.error("This item is currently unavailable.")
      return
    }

    try {
      await addToCart(menuItemId, 1)
      toast.success("Item added to cart successfully!")
    } catch (err) {
      toast.error(err.response?.data?.message || "Failed to add item")
    }
  }

  const handleSearch = async () => {
    try {
      setLoading(true)
      let data = []

      if (menuName.trim()) {
        const nameLower = menuName.trim().toLowerCase()
        const allMenu = await getAllMenuItems()
        data = allMenu.filter((item) =>
          item.name.toLowerCase().includes(nameLower)
        )
      } else if (location.trim()) {
        data = await getMenuByLocation(location.trim())
      } else if (category) {
        data = await getMenuByCategoryName(category)
      } else if (restaurant) {
        data = await getMenuByRestaurantName(restaurant)
      } else {
        data = await getAllMenuItems()
      }

      setMenu(data)
    } catch {
      toast.error("Search failed")
      setMenu([])
    } finally {
      setLoading(false)
    }
  }

  const handleClearSearch = async () => {
    setLocation("")
    setRestaurant("")
    setCategory("")
    setMenuName("")
    await fetchMenu()
  }

  return (
    <>
      <UserNavbar />
      <div className="user-home-container">
        <div className="container py-4">

                  {/* Hero Section */}
        <div className="hero-section mb-5">
          <div className="hero-content text-center">
            <h1 className="hero-title">Delicious Food Delivered</h1>
            <p className="hero-subtitle">
              Order your favorite meals from the best restaurants in your area
            </p>
          </div>
        </div>


          {/* Filter Section */}
          <div className="filter-card mb-5">
            <div className="filter-header">
              <h5 className="filter-title">
                <i className="bi bi-funnel me-2"></i>Find Your Perfect Meal
              </h5>
            </div>
            <div className="filter-body">
              <div className="row g-3">
                <div className="col-lg-3 col-md-6">
                  <label className="filter-label">
                    <i className="bi bi-search me-1"></i>Menu Item Name
                  </label>
                  <input
                    type="text"
                    className="form-control filter-input"
                    placeholder="e.g. Pizza"
                    value={menuName}
                    onChange={(e) => setMenuName(e.target.value)}
                  />
                </div>
                <div className="col-lg-3 col-md-6">
                  <label className="filter-label">
                    <i className="bi bi-geo-alt me-1"></i>Location
                  </label>
                  <input
                    type="text"
                    className="form-control filter-input"
                    placeholder="e.g. Chennai"
                    value={location}
                    onChange={(e) => setLocation(e.target.value)}
                  />
                </div>
                <div className="col-lg-3 col-md-6">
                  <label className="filter-label">
                    <i className="bi bi-shop me-1"></i>Restaurant
                  </label>
                  <select
                    className="form-select filter-input"
                    value={restaurant}
                    onChange={(e) => setRestaurant(e.target.value)}
                  >
                    <option value="">All Restaurants</option>
                    {allRestaurants.map((r) => (
                      <option key={r.id} value={r.name}>{r.name}</option>
                    ))}
                  </select>
                </div>
                <div className="col-lg-3 col-md-6">
                  <label className="filter-label">
                    <i className="bi bi-grid-3x3-gap me-1"></i>Category
                  </label>
                  <select
                    className="form-select filter-input"
                    value={category}
                    onChange={(e) => setCategory(e.target.value)}
                  >
                    <option value="">All Categories</option>
                    {allCategories.map((c) => (
                      <option key={c.id} value={c.name}>{c.name}</option>
                    ))}
                  </select>
                </div>
                <div className="col-12 d-flex justify-content-end gap-2 mt-2">
                  <button className="btn search-btn" onClick={handleSearch}>
                    <i className="bi bi-search me-1"></i>Search
                  </button>
                  <button className="btn clear-btn" onClick={handleClearSearch}>
                    <i className="bi bi-x-circle"></i>
                  </button>
                </div>
              </div>
            </div>
          </div>

          {/* Menu Items Section */}
          {loading ? (
            <div className="loading-section">
              <div className="d-flex justify-content-center">
                <div className="spinner-border text-primary" style={{ width: "3rem", height: "3rem" }}>
                  <span className="visually-hidden">Loading...</span>
                </div>
              </div>
              <p className="text-center mt-3 text-muted">Loading delicious meals...</p>
            </div>
          ) : menu.length === 0 ? (
            <div className="empty-menu text-center">
              <i className="bi bi-search display-1 text-muted mb-3"></i>
              <h4 className="text-muted mb-2">No menu items found</h4>
              <p className="text-muted">Try adjusting your search filters or browse all available items.</p>
            </div>
          ) : (
            <div className="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-4">
              {menu.map((item) => (
                <div key={item.id} className="col-md-4">
                  <div className={`card h-100 position-relative ${!item.isAvailable ? "unavailable" : ""}`}>
                    {!item.isAvailable && (
                      <span className="badge bg-danger position-absolute top-0 end-0 m-2">
                        Unavailable
                      </span>
                    )}

                    <div className="menu-image-container">
                      {item.imageUrl ? (
                        <img src={item.imageUrl} alt={item.name} className="card-img-top" />
                      ) : (
                        <div className="menu-image-placeholder text-center py-4">No Image</div>
                      )}
                    </div>

                    <div className="card-body d-flex flex-column">
                      <h5 className="card-title">{item.name}</h5>
                      <p className="card-text text-muted">{item.description}</p>
                      <div className="mb-2">
                        <span className="badge bg-secondary me-2">
                          <i className="bi bi-tag me-1"></i>{item.menuCategory?.name || "N/A"}
                        </span>
                        <span className="badge bg-info ">
                          <i className="bi bi-shop me-1"></i>{item.restaurant?.name || "N/A"}
                        </span>
                      </div>
                      <div className="mt-auto d-flex justify-content-between align-items-center">
                        <span className="fw-bold fs-5">₹{item.price}</span>
                        <button
                          className="btn btn-success"
                          onClick={() => handleAddToCart(item.id)}
                          disabled={!item.isAvailable}
                        >
                          <i className="bi bi-cart-plus me-1"></i>
                          {item.isAvailable ? "Add to Cart" : "Unavailable"}
                        </button>
                      </div>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </>
  )
}

export default UserHome
