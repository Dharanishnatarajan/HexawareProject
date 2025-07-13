"use client"
import { useEffect, useState } from "react"
import RestaurantNavbar from "../../components/RestaurantNavbar"
import { getRestaurantOrders, updateOrderStatus } from "../../api/restaurantService"
import { toast } from "react-hot-toast"
import "./RestaurantOrders.css"

const RestaurantOrders = () => {
  const [orders, setOrders] = useState([])
  const [loading, setLoading] = useState(true)
  const [filter, setFilter] = useState("All")

  useEffect(() => {
    fetchOrders()
  }, [])

  const fetchOrders = async () => {
    try {
      setLoading(true)
      const data = await getRestaurantOrders()
      setOrders(data)
    } catch {
      toast.error("Failed to load orders")
    } finally {
      setLoading(false)
    }
  }

  const handleStatusChange = async (orderId, status) => {
    try {
      await updateOrderStatus(orderId, status)
      toast.success("Order status updated successfully!")
      fetchOrders()
    } catch {
      toast.error("Failed to update status")
    }
  }

  const getStatusBadgeClass = (status) => {
    switch (status) {
      case "Pending":
        return "status-pending"
      case "Preparing":
        return "status-preparing"
      case "Ready":
        return "status-ready"
      case "Delivered":
        return "status-delivered"
      default:
        return "status-pending"
    }
  }

  const filteredOrders = filter === "All" ? orders : orders.filter((order) => order.status === filter)

  const getOrderStats = () => {
    return {
      total: orders.length,
      pending: orders.filter((o) => o.status === "Pending").length,
      preparing: orders.filter((o) => o.status === "Preparing").length,
      ready: orders.filter((o) => o.status === "Ready").length,
      delivered: orders.filter((o) => o.status === "Delivered").length,
    }
  }

  const stats = getOrderStats()

  return (
    <>
      <RestaurantNavbar />

      <div className="orders-container">
        <div className="container">
          {/* Header Section */}
          <div className="orders-header">
            <div className="d-flex align-items-center justify-content-between">
              <div className="d-flex align-items-center">
                <div className="header-icon">
                  <i className="bi bi-box-seam"></i>
                </div>
                <div className="ms-3">
                  <h2 className="header-title mb-1">Restaurant Orders</h2>
                  <p className="header-subtitle mb-0">Manage and track all your orders</p>
                </div>
              </div>
              <div className="refresh-btn">
                <button className="btn btn-outline-primary" onClick={fetchOrders} disabled={loading}>
                  <i className="bi bi-arrow-clockwise me-2"></i>
                  {loading ? "Loading..." : "Refresh"}
                </button>
              </div>
            </div>
          </div>

          {/* Stats Cards */}
          <div className="stats-section mb-4">
            <div className="row g-3">
              <div className="col-md-2">
                <div className="stat-card stat-total">
                  <div className="stat-icon">
                    <i className="bi bi-list-ul"></i>
                  </div>
                  <div className="stat-info">
                    <div className="stat-number">{stats.total}</div>
                    <div className="stat-label">Total Orders</div>
                  </div>
                </div>
              </div>
              <div className="col-md-2">
                <div className="stat-card stat-pending">
                  <div className="stat-icon">
                    <i className="bi bi-clock"></i>
                  </div>
                  <div className="stat-info">
                    <div className="stat-number">{stats.pending}</div>
                    <div className="stat-label">Pending</div>
                  </div>
                </div>
              </div>
              <div className="col-md-2">
                <div className="stat-card stat-preparing">
                  <div className="stat-icon">
                    <i className="bi bi-fire"></i>
                  </div>
                  <div className="stat-info">
                    <div className="stat-number">{stats.preparing}</div>
                    <div className="stat-label">Preparing</div>
                  </div>
                </div>
              </div>
              <div className="col-md-2">
                <div className="stat-card stat-ready">
                  <div className="stat-icon">
                    <i className="bi bi-check-circle"></i>
                  </div>
                  <div className="stat-info">
                    <div className="stat-number">{stats.ready}</div>
                    <div className="stat-label">Ready</div>
                  </div>
                </div>
              </div>
              <div className="col-md-2">
                <div className="stat-card stat-delivered">
                  <div className="stat-icon">
                    <i className="bi bi-truck"></i>
                  </div>
                  <div className="stat-info">
                    <div className="stat-number">{stats.delivered}</div>
                    <div className="stat-label">Delivered</div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          {/* Filter Section */}
          <div className="filter-section mb-4">
            <div className="d-flex gap-2 flex-wrap">
              {["All", "Pending", "Preparing", "Ready", "Delivered"].map((status) => (
                <button
                  key={status}
                  className={`filter-btn ${filter === status ? "active" : ""}`}
                  onClick={() => setFilter(status)}
                >
                  {status}
                </button>
              ))}
            </div>
          </div>

          {/* Orders Table */}
          <div className="orders-table-card">
            {loading ? (
              <div className="loading-state">
                <div className="spinner-border text-primary" role="status">
                  <span className="visually-hidden">Loading...</span>
                </div>
                <p className="mt-3 text-muted">Loading orders...</p>
              </div>
            ) : filteredOrders.length === 0 ? (
              <div className="empty-state">
                <i className="bi bi-inbox display-1 text-muted mb-3"></i>
                <h5 className="text-muted mb-2">No orders found</h5>
                <p className="text-muted">
                  {filter === "All"
                    ? "You haven't received any orders yet."
                    : `No ${filter.toLowerCase()} orders found.`}
                </p>
              </div>
            ) : (
              <div className="table-responsive">
                <table className="table orders-table">
                  <thead>
                    <tr>
                      <th>Order Details</th>
                      <th>Customer Info</th>
                      <th>Items</th>
                      <th>Total</th>
                      <th>Status</th>
                      <th>Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {filteredOrders.map((order) => (
                      <tr key={order.id} className="order-row">
                        <td>
                          <div className="order-id">
                            <strong>#{order.id}</strong>
                          </div>
                          
                        </td>
                        <td>
                          <div className="customer-info">
                            <div className="customer-name">
                              <i className="bi bi-person me-1"></i>
                              <strong>{order.customerName}</strong>
                            </div>
                            <div className="customer-phone">
                              <i className="bi bi-telephone me-1"></i>
                              {order.phoneNumber}
                            </div>
                            <div className="customer-address">
                              <i className="bi bi-geo-alt me-1"></i>
                              {order.address}
                            </div>
                          </div>
                        </td>
                        <td>
                          <div className="order-items">
                            {order.items.map((item, index) => (
                              <div key={index} className="order-item">
                                <span className="item-name">{item.name}</span>
                                <span className="item-quantity">× {item.quantity}</span>
                              </div>
                            ))}
                          </div>
                        </td>
                        <td>
                          <div className="order-total">₹{order.totalAmount}</div>
                        </td>
                        <td>
                          <span className={`status-badge ${getStatusBadgeClass(order.status)}`}>{order.status}</span>
                        </td>
                        <td>
                          <select
                            className="form-select status-select"
                            value={order.status}
                            onChange={(e) => handleStatusChange(order.id, e.target.value)}
                          >
                            <option value="Pending">Pending</option>
                            <option value="Preparing">Preparing</option>
                            <option value="Ready">Ready</option>
                            <option value="Delivered">Delivered</option>
                          </select>
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
    </>
  )
}

export default RestaurantOrders
