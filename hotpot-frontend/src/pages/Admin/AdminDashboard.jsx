"use client";
import { useEffect, useState } from "react";
import {
  getAllUsers,
  updateUser,
  deleteUser,
  getAllRestaurants,
  deleteRestaurant,
  updateRestaurant,
} from "../../api/adminService";
import { toast } from "react-hot-toast";
import AdminNavbar from "../../components/AdminNavbar";
import "./AdminDashboard.css";

const AdminDashboard = () => {
  const [users, setUsers] = useState([]);
  const [restaurants, setRestaurants] = useState([]);
  const [editUserId, setEditUserId] = useState(null);
  const [userForm, setUserForm] = useState({ name: "", email: "" });
  const [editRestaurantId, setEditRestaurantId] = useState(null);
  const [restaurantForm, setRestaurantForm] = useState({
    name: "",
    location: "",
    contactNumber: "",
  });
  const [loading, setLoading] = useState(true);
  const [activeTab, setActiveTab] = useState("users");
  const [searchTerm, setSearchTerm] = useState("");
  const [userFilter, setUserFilter] = useState("All");

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const usersData = await getAllUsers();
      const restData = await getAllRestaurants();
      setUsers(usersData);
      setRestaurants(restData);
    } catch {
      toast.error("Failed to load data");
    } finally {
      setLoading(false);
    }
  };

  const handleEditUser = (user) => {
    setEditUserId(user.id);
    setUserForm({ name: user.name, email: user.email });
  };

  const handleUpdateUser = async () => {
    try {
      await updateUser(editUserId, userForm);
      toast.success("User updated successfully!");
      setEditUserId(null);
      setUserForm({ name: "", email: "" });
      loadData();
    } catch {
      toast.error("Failed to update user");
    }
  };

  const handleDeleteUser = async (id) => {
    if (window.confirm("Delete this user?")) {
      try {
        await deleteUser(id);
        toast.success("User deleted!");
        loadData();
      } catch {
        toast.error("Failed to delete user");
      }
    }
  };

  const handleEditRestaurant = (restaurant) => {
    setEditRestaurantId(restaurant.id);
    setRestaurantForm({
      name: restaurant.name,
      location: restaurant.location,
      contactNumber: restaurant.contactNumber || "",
    });
  };

  const handleUpdateRestaurant = async () => {
    try {
      await updateRestaurant(editRestaurantId, restaurantForm);
      toast.success("Restaurant updated!");
      setEditRestaurantId(null);
      setRestaurantForm({ name: "", location: "", contactNumber: "" });
      loadData();
    } catch {
      toast.error("Failed to update restaurant");
    }
  };

  const handleDeleteRestaurant = async (id) => {
    if (window.confirm("Delete this restaurant?")) {
      try {
        await deleteRestaurant(id);
        toast.success("Restaurant deleted!");
        loadData();
      } catch {
        toast.error("Failed to delete restaurant");
      }
    }
  };

  const filteredUsers = users.filter((u) =>
    (u.name + u.email).toLowerCase().includes(searchTerm.toLowerCase()) &&
    (userFilter === "All" || u.role === userFilter)
  );

  const filteredRestaurants = restaurants.filter((r) =>
    (r.name + r.location).toLowerCase().includes(searchTerm.toLowerCase())
  );

  const stats = {
    totalUsers: users.length,
    totalRestaurants: restaurants.length,
    adminUsers: users.filter((u) => u.role === "Admin").length,
    regularUsers: users.filter((u) => u.role === "User").length,
  };

  return (
    <>
      <AdminNavbar />
      <div className="admin-dashboard-container">
        <div className="container py-4">
          {/* Dashboard Header */}
          <div className="dashboard-header">
            <div className="d-flex justify-content-between align-items-center">
              <div className="d-flex align-items-center">
                <div className="dashboard-icon">
                  <i className="bi bi-speedometer2"></i>
                </div>
                <div className="ms-3">
                  <h1 className="dashboard-title mb-1">Admin Dashboard</h1>
                  <p className="dashboard-subtitle mb-0">Manage users and restaurants across the platform</p>
                </div>
              </div>
              <div className="refresh-section">
                <button className="btn refresh-btn" onClick={loadData} disabled={loading}>
                  <i className="bi bi-arrow-clockwise me-2"></i>
                  {loading ? "Loading..." : "Refresh"}
                </button>
              </div>
            </div>
          </div>

          {/* Stats Cards */}
          <div className="stats-section mb-4">
            <div className="row g-3">
              <div className="col-md-4">
                <div className="stat-card stat-total">
                  <div className="stat-icon">
                    <i className="bi bi-people"></i>
                  </div>
                  <div className="stat-info">
                    <div className="stat-number">{stats.totalUsers}</div>
                    <div className="stat-label">Total Users</div>
                  </div>
                </div>
              </div>
              <div className="col-md-4">
                <div className="stat-card stat-admins">
                  <div className="stat-icon">
                    <i className="bi bi-shield-check"></i>
                  </div>
                  <div className="stat-info">
                    <div className="stat-number">{stats.adminUsers}</div>
                    <div className="stat-label">Admins</div>
                  </div>
                </div>
              </div>
              <div className="col-md-4">
                <div className="stat-card stat-restaurants">
                  <div className="stat-icon">
                    <i className="bi bi-shop"></i>
                  </div>
                  <div className="stat-info">
                    <div className="stat-number">{stats.totalRestaurants}</div>
                    <div className="stat-label">Restaurants</div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Tab Navigation */}
          <div className="tab-navigation mb-4">
            <div className="nav nav-pills">
              <button
                className={`nav-link ${activeTab === "users" ? "active" : ""}`}
                onClick={() => setActiveTab("users")}
              >
                <i className="bi bi-people me-2"></i>
                Users
              </button>
              <button
                className={`nav-link ${activeTab === "restaurants" ? "active" : ""}`}
                onClick={() => setActiveTab("restaurants")}
              >
                <i className="bi bi-shop me-2"></i>
                Restaurants
              </button>
            </div>
          </div>

          {/* Search and Filter Section */}
          <div className="search-filter-section mb-4">
            <div className="row g-3">
              <div className="col-md-6">
                <div className="search-box">
                  <input
                    type="text"
                    className="form-control search-input"
                    placeholder="Search..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                  />
                </div>
              </div>
              {activeTab === "users" && (
                <div className="col-md-3">
                  <select
                    className="form-select filter-select"
                    value={userFilter}
                    onChange={(e) => setUserFilter(e.target.value)}
                  >
                    <option value="All">All Roles</option>
                    <option value="Admin">Admin</option>
                    <option value="User">User</option>
                  </select>
                </div>
              )}
            </div>
          </div>

          {/* Users Table */}
          {activeTab === "users" && (
            <div className="data-table-card">
              <div className="table-header">
                <h4 className="table-title">
                  <i className="bi bi-people me-2"></i>
                  Users Management
                </h4>
                <p className="table-subtitle">Manage all platform users and their roles</p>
              </div>
              <div className="table-body">
                <div className="table-responsive">
                  <table className="table admin-table">
                    <thead>
                      <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Role</th>
                        <th>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      {filteredUsers.map((u) => (
                        <tr key={u.id}>
                          <td>
                            <div className="user-id">#{u.id}</div>
                          </td>
                          <td>
                            {editUserId === u.id ? (
                              <input
                                type="text"
                                className="form-control edit-input"
                                value={userForm.name}
                                onChange={(e) => setUserForm({ ...userForm, name: e.target.value })}
                              />
                            ) : (
                              <div className="user-name">{u.name}</div>
                            )}
                          </td>
                          <td>
                            {editUserId === u.id ? (
                              <input
                                type="email"
                                className="form-control edit-input"
                                value={userForm.email}
                                onChange={(e) => setUserForm({ ...userForm, email: e.target.value })}
                              />
                            ) : (
                              <div className="user-email">{u.email}</div>
                            )}
                          </td>
                          <td>
                            <span className={`role-badge role-${u.role.toLowerCase()}`}>{u.role}</span>
                          </td>
                          <td>
                            <div className="action-buttons">
                              {editUserId === u.id ? (
                                <>
                                  <button className="btn btn-sm action-btn save-btn" onClick={handleUpdateUser}>
                                    <i className="bi bi-check"></i>
                                  </button>
                                  <button className="btn btn-sm action-btn cancel-btn" onClick={() => setEditUserId(null)}>
                                    <i className="bi bi-x"></i>
                                  </button>
                                </>
                              ) : (
                                <>
                                  <button
                                    className="btn btn-sm action-btn edit-btn"
                                    onClick={() => handleEditUser(u)}
                                    title="Edit User"
                                  >
                                    <i className="bi bi-pencil"></i>
                                  </button>
                                  <button
                                    className="btn btn-sm action-btn delete-btn"
                                    onClick={() => handleDeleteUser(u.id)}
                                    title="Delete User"
                                  >
                                    <i className="bi bi-trash"></i>
                                  </button>
                                </>
                              )}
                            </div>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          )}

          {/* Restaurants Table */}
          {activeTab === "restaurants" && (
            <div className="data-table-card">
              <div className="table-header">
                <h4 className="table-title">
                  <i className="bi bi-shop me-2"></i>
                  Restaurant Management
                </h4>
                <p className="table-subtitle">Manage all restaurants and their information</p>
              </div>
              <div className="table-body">
                <div className="table-responsive">
                  <table className="table admin-table">
                    <thead>
                      <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>Location</th>
                        <th>Contact</th>
                        <th>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      {filteredRestaurants.map((r) => (
                        <tr key={r.id}>
                          <td>
                            <div className="restaurant-id">#{r.id}</div>
                          </td>
                          <td>
                            {editRestaurantId === r.id ? (
                              <input
                                type="text"
                                className="form-control edit-input"
                                value={restaurantForm.name}
                                onChange={(e) => setRestaurantForm({ ...restaurantForm, name: e.target.value })}
                              />
                            ) : (
                                  <div className="name-display">{r.name}</div>
                            )}
                          </td>
                          <td>
                            {editRestaurantId === r.id ? (
                              <input
                                type="text"
                                className="form-control edit-input"
                                value={restaurantForm.location}
                                onChange={(e) => setRestaurantForm({ ...restaurantForm, location: e.target.value })}
                              />
                            ) : (
                              <div className="restaurant-location">
                                <i className="bi bi-geo-alt me-1"></i>
                                {r.location}
                              </div>
                            )}
                          </td>
                          <td>
                            {editRestaurantId === r.id ? (
                              <input
                                type="text"
                                className="form-control edit-input"
                                value={restaurantForm.contactNumber}
                                onChange={(e) => setRestaurantForm({ ...restaurantForm, contactNumber: e.target.value })}
                              />
                            ) : (
                              <div className="contact-number">
                                <i className="bi bi-telephone me-1"></i>
                                {r.contactNumber || "N/A"}
                              </div>
                            )}
                          </td>
                          <td>
                            <div className="action-buttons">
                              {editRestaurantId === r.id ? (
                                <>
                                  <button className="btn btn-sm action-btn save-btn" onClick={handleUpdateRestaurant}>
                                    <i className="bi bi-check"></i>
                                  </button>
                                  <button className="btn btn-sm action-btn cancel-btn" onClick={() => setEditRestaurantId(null)}>
                                    <i className="bi bi-x"></i>
                                  </button>
                                </>
                              ) : (
                                <>
                                  <button
                                    className="btn btn-sm action-btn edit-btn"
                                    onClick={() => handleEditRestaurant(r)}
                                    title="Edit Restaurant"
                                  >
                                    <i className="bi bi-pencil"></i>
                                  </button>
                                  <button
                                    className="btn btn-sm action-btn delete-btn"
                                    onClick={() => handleDeleteRestaurant(r.id)}
                                    title="Delete Restaurant"
                                  >
                                    <i className="bi bi-trash"></i>
                                  </button>
                                </>
                              )}
                            </div>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </>
  );
};

export default AdminDashboard;
