"use client"
import { useEffect, useState } from "react"
import {
  getAllMenuItems,
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
  const [viewMode, setViewMode] = useState("menu") // "menu" or "restaurants"
  const [selectedRestaurant, setSelectedRestaurant] = useState(null)
  const [restaurantMenu, setRestaurantMenu] = useState([])

  const [suggestions, setSuggestions] = useState([])
  const [showSuggestions, setShowSuggestions] = useState(false)

  const [locationSuggestions, setLocationSuggestions] = useState([])
  const [showLocationSuggestions, setShowLocationSuggestions] = useState(false)

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
    const item =
      viewMode === "menu" ? menu.find((m) => m.id === menuItemId) : restaurantMenu.find((m) => m.id === menuItemId)

    if (!item?.isAvailable) {
      toast.error("This item is currently unavailable.")
      return
    }

    try {
      await addToCart(menuItemId, 1)
      toast.success("Item added to cart successfully!")
    } catch (err) {
      toast.error(err.response?.data?.message || "Cannot Add Multiple Restaurant Item")
    }
  }

  const handleSearch = async () => {
  try {
    setLoading(true)
    const allMenuItems = await getAllMenuItems()
    let filtered = allMenuItems

    if (menuName.trim()) {
      const nameLower = menuName.trim().toLowerCase()
      filtered = filtered.filter((item) =>
        item.name.toLowerCase().includes(nameLower)
      )
    }

    if (location.trim()) {
      const locationLower = location.trim().toLowerCase()
      filtered = filtered.filter(
        (item) => item.restaurant?.location.toLowerCase().includes(locationLower)
      )
    }

    if (restaurant) {
      filtered = filtered.filter(
        (item) => item.restaurant?.name === restaurant
      )
    }

    if (category) {
      filtered = filtered.filter(
        (item) => item.menuCategory?.name === category
      )
    }

    setMenu(filtered)
  } catch  {
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

  const handleViewRestaurant = async (restaurantData) => {
    try {
      setLoading(true)
      setSelectedRestaurant(restaurantData)
      const menuData = await getMenuByRestaurantName(restaurantData.name)
      setRestaurantMenu(menuData)
      setViewMode("restaurant-detail")
    } catch {
      toast.error("Failed to load restaurant menu")
    } finally {
      setLoading(false)
    }
  }

  const handleBackToRestaurants = () => {
    setViewMode("restaurants")
    setSelectedRestaurant(null)
    setRestaurantMenu([])
  }

  const getRestaurantStats = (restaurantName) => {
    const restaurantItems = menu.filter((item) => item.restaurant?.name === restaurantName)
    return {
      totalItems: restaurantItems.length,
      availableItems: restaurantItems.filter((item) => item.isAvailable).length,
      categories: [...new Set(restaurantItems.map((item) => item.menuCategory?.name))].filter(Boolean).length,
    }
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
              <p className="hero-subtitle">Order your favorite meals from the best restaurants in your area</p>
            </div>
          </div>

          {/* View Mode Toggle */}
          <div className="view-toggle-section mb-4">
            <div className="d-flex justify-content-center gap-3">
              <button
                className={`btn view-toggle-btn ${viewMode === "menu" ? "active" : ""}`}
                onClick={() => setViewMode("menu")}
              >
                <i className="bi bi-grid-3x3-gap me-2"></i>
                Browse Menu
              </button>
              <button
                className={`btn view-toggle-btn ${viewMode === "restaurants" ? "active" : ""}`}
                onClick={() => setViewMode("restaurants")}
              >
                <i className="bi bi-shop me-2"></i>
                Browse Restaurants
              </button>
            </div>
          </div>

          {/* Restaurant Detail View */}
          {viewMode === "restaurant-detail" && selectedRestaurant && (
            <>
              <div className="restaurant-detail-header mb-4">
                <div className="d-flex align-items-center mb-3">
                  <button className="btn btn-outline-primary me-3" onClick={handleBackToRestaurants}>
                    <i className="bi bi-arrow-left me-2"></i>Back to Restaurants
                  </button>
                </div>

                <div className="restaurant-detail-card">
                  <div className="restaurant-detail-content">
                    <div className="restaurant-detail-icon">
                      <i className="bi bi-shop"></i>
                    </div>
                    <div className="restaurant-detail-info">
                      <h2 className="restaurant-detail-name">{selectedRestaurant.name}</h2>
                      <div className="restaurant-detail-meta">
                        <span className="restaurant-detail-location">
                          <i className="bi bi-geo-alt me-2"></i>
                          {selectedRestaurant.location}
                        </span>
                        <span className="restaurant-detail-contact">
                          <i className="bi bi-telephone me-2"></i>
                          {selectedRestaurant.contactNumber}
                        </span>
                      </div>
                    </div>
                    <div className="restaurant-detail-stats">
                      <div className="stat-item">
                        <span className="stat-number">{restaurantMenu.length}</span>
                        <span className="stat-label">Menu Items</span>
                      </div>
                      <div className="stat-item">
                        <span className="stat-number">{restaurantMenu.filter((item) => item.isAvailable).length}</span>
                        <span className="stat-label">Available</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              {/* Restaurant Menu */}
              <div className="restaurant-menu-section">
                <h4 className="section-title mb-4">
                  <i className="bi bi-list-ul me-2"></i>
                  Menu Items ({restaurantMenu.length})
                </h4>

                {loading ? (
                  <div className="loading-section">
                    <div className="d-flex justify-content-center">
                      <div className="spinner-border text-primary" style={{ width: "3rem", height: "3rem" }}>
                        <span className="visually-hidden">Loading...</span>
                      </div>
                    </div>
                    <p className="text-center mt-3 text-muted">Loading restaurant menu...</p>
                  </div>
                ) : restaurantMenu.length === 0 ? (
                  <div className="empty-menu text-center">
                    <i className="bi bi-basket display-1 text-muted mb-3"></i>
                    <h4 className="text-muted mb-2">No menu items available</h4>
                    <p className="text-muted">This restaurant hasn't added any menu items yet.</p>
                  </div>
                ) : (
                  <div className="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-4">
                    {restaurantMenu.map((item) => (
                      <div key={item.id} className="col">
                        <div className={`menu-item-card ${!item.isAvailable ? "unavailable" : ""}`}>
                          {!item.isAvailable && <span className="unavailable-badge">Unavailable</span>}

                          <div className="menu-image-container">
                            {item.imageUrl ? (
                              <img src={item.imageUrl || "/placeholder.svg"} alt={item.name} className="menu-image" />
                            ) : (
                              <div className="menu-image-placeholder">
                                <i className="bi bi-image"></i>
                              </div>
                            )}
                          </div>

                          <div className="menu-content">
                            <div className="menu-header">
                              <h5 className="menu-title">{item.name}</h5>
                              <div className="menu-price">₹{item.price}</div>
                            </div>

                            <p className="menu-description">{item.description || "No description available"}</p>

                            <div className="menu-meta">
                              <span className="menu-category">
                                <i className="bi bi-tag me-1"></i>
                                {item.menuCategory?.name || "N/A"}
                              </span>
                            </div>

                            <button
                              className="btn add-to-cart-btn"
                              onClick={() => handleAddToCart(item.id)}
                              disabled={!item.isAvailable}
                            >
                              <i className="bi bi-cart-plus me-2"></i>
                              {item.isAvailable ? "Add to Cart" : "Unavailable"}
                            </button>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            </>
          )}

          {/* Restaurants View */}
          {viewMode === "restaurants" && (
            <div className="restaurants-section">
              <h4 className="section-title mb-4">
                <i className="bi bi-shop me-2"></i>
                All Restaurants ({allRestaurants.length})
              </h4>

              <div className="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                {allRestaurants.map((restaurantItem) => {
                  const stats = getRestaurantStats(restaurantItem.name)
                  return (
                    <div key={restaurantItem.id} className="col">
                      <div className="restaurant-card">
                        <div className="restaurant-card-header">
                          <div className="restaurant-card-icon">
                            <i className="bi bi-shop"></i>
                          </div>
                          <div className="restaurant-card-info">
                            <h5 className="restaurant-card-name">{restaurantItem.name}</h5>
                            <p className="restaurant-card-location">
                              <i className="bi bi-geo-alt me-1"></i>
                              {restaurantItem.location}
                            </p>
                          </div>
                        </div>

                        <div className="restaurant-card-stats">
                          <div className="stat-row">
                            <span className="stat-item">
                              <i className="bi bi-list-ul me-1"></i>
                              {stats.totalItems} Items
                            </span>
                            <span className="stat-item">
                              <i className="bi bi-check-circle me-1"></i>
                              {stats.availableItems} Available
                            </span>
                          </div>
                          <div className="stat-row">
                            <span className="stat-item">
                              <i className="bi bi-tags me-1"></i>
                              {stats.categories} Categories
                            </span>
                            <span className="stat-item">
                              <i className="bi bi-telephone me-1"></i>
                              {restaurantItem.contactNumber}
                            </span>
                          </div>
                        </div>

                        <div className="restaurant-card-actions">
                          <button
                            className="btn btn-primary restaurant-view-btn"
                            onClick={() => handleViewRestaurant(restaurantItem)}
                          >
                            <i className="bi bi-eye me-2"></i>
                            View Menu
                          </button>
                        </div>
                      </div>
                    </div>
                  )
                })}
              </div>
            </div>
          )}

          {/* Menu Items View */}
          {viewMode === "menu" && (
            <>
              {/* Filter Section */}
              <div className="filter-card mb-5">
                <div className="filter-header">
                  <h5 className="filter-title">
                    <i className="bi bi-funnel me-2"></i>Find Your Perfect Meal
                  </h5>
                </div>
                <div className="filter-body">
                  <div className="row g-3">
                    <div className="col-lg-3 col-md-6 position-relative">
                      <label className="filter-label">
                        <i className="bi bi-search me-1"></i>Menu Item Name
                      </label>
                      <input
                        type="text"
                        className="form-control filter-input"
                        placeholder="e.g. Pizza"
                        value={menuName}
                        onChange={(e) => {
                          const value = e.target.value
                          setMenuName(value)
                          if (value.trim()) {
                            const filtered = menu
                              .filter((item) => item.name.toLowerCase().includes(value.toLowerCase()))
                              .map((item) => item.name)
                              .filter((val, i, arr) => arr.indexOf(val) === i)
                              .slice(0, 5)
                            setSuggestions(filtered)
                            setShowSuggestions(true)
                          } else {
                            setShowSuggestions(false)
                          }
                        }}
                        onFocus={() => menuName && setShowSuggestions(true)}
                        onBlur={() => setTimeout(() => setShowSuggestions(false), 100)}
                      />
                      {showSuggestions && suggestions.length > 0 && (
                        <ul className="list-group position-absolute w-100 z-3" style={{ top: "100%" }}>
                          {suggestions.map((s, i) => (
                            <li
                              key={i}
                              className="list-group-item list-group-item-action"
                              style={{ cursor: "pointer" }}
                              onClick={() => {
                                setMenuName(s)
                                setShowSuggestions(false)
                              }}
                            >
                              {s}
                            </li>
                          ))}
                        </ul>
                      )}
                    </div>
                    <div className="col-lg-3 col-md-6 position-relative">
                      <label className="filter-label">
                        <i className="bi bi-geo-alt me-1"></i>Location
                      </label>
                      <input
                        type="text"
                        className="form-control filter-input"
                        placeholder="e.g. Chennai"
                        value={location}
                        onChange={(e) => {
                          const value = e.target.value
                          setLocation(value)
                          if (value.trim()) {
                            const filtered = allRestaurants
                              .filter((r) => r.location.toLowerCase().includes(value.toLowerCase()))
                              .map((r) => r.location)
                              .filter((val, i, arr) => arr.indexOf(val) === i)
                              .slice(0, 5)
                            setLocationSuggestions(filtered)
                            setShowLocationSuggestions(true)
                          } else {
                            setShowLocationSuggestions(false)
                          }
                        }}
                        onFocus={() => location && setShowLocationSuggestions(true)}
                        onBlur={() => setTimeout(() => setShowLocationSuggestions(false), 100)}
                      />
                      {showLocationSuggestions && locationSuggestions.length > 0 && (
                        <ul className="list-group position-absolute w-100 z-3" style={{ top: "100%" }}>
                          {locationSuggestions.map((s, i) => (
                            <li
                              key={i}
                              className="list-group-item list-group-item-action"
                              style={{ cursor: "pointer" }}
                              onClick={() => {
                                setLocation(s)
                                setShowLocationSuggestions(false)
                              }}
                            >
                              {s}
                            </li>
                          ))}
                        </ul>
                      )}
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
                          <option key={r.id} value={r.name}>
                            {r.name}
                          </option>
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
                          <option key={c.id} value={c.name}>
                            {c.name}
                          </option>
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
                <div className="menu-grid">
                  {menu.map((item) => (
                    <div key={item.id} className={`menu-item-card ${!item.isAvailable ? "unavailable" : ""}`}>
                      {!item.isAvailable && <span className="unavailable-badge">Unavailable</span>}

                      <div className="menu-image-container">
                        {item.imageUrl ? (
                          <img src={item.imageUrl || "/placeholder.svg"} alt={item.name} className="menu-image" />
                        ) : (
                          <div className="menu-image-placeholder">
                            <i className="bi bi-image"></i>
                          </div>
                        )}

                      </div>

                      <div className="menu-content">
                        <div className="menu-header">
                          <h5 className="menu-title">{item.name}</h5>
                          <div className="menu-price">₹{item.price}</div>
                        </div>

                        <p className="menu-description">{item.description}</p>

                        <div className="menu-meta">
                          <span className="menu-category">
                            <i className="bi bi-tag me-1"></i>
                            {item.menuCategory?.name || "N/A"}
                          </span>
                          <span className="menu-restaurant">
                            <i className="bi bi-shop me-1"></i>
                            {item.restaurant?.name || "N/A"}
                          </span>
                        </div>

                        <button
                          className="btn add-to-cart-btn"
                          onClick={() => handleAddToCart(item.id)}
                          disabled={!item.isAvailable}
                        >
                          <i className="bi bi-cart-plus me-2"></i>
                          {item.isAvailable ? "Add to Cart" : "Unavailable"}
                        </button>
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </>
          )}
        </div>
      </div>
    </>
  )
}

export default UserHome
