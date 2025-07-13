"use client"
import { useEffect, useState } from "react"
import { toast } from "react-hot-toast"
import axios from "../../api/axios"
import UserNavbar from "../../components/UserNavbar"
import "./MyOrders.css"

const MyOrders = () => {
  const [orders, setOrders] = useState([])
  const [loading, setLoading] = useState(true)
  const [filter, setFilter] = useState("All")

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        setLoading(true)
        const res = await axios.get("/orders/user")
        setOrders(res.data)
      } catch {
        toast.error("Failed to load orders")
      } finally {
        setLoading(false)
      }
    }
    fetchOrders()
  }, [])

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

  return (
    <>
      <UserNavbar />
      <div className="my-orders-container">
        <div className="container py-4">
          {/* Header Section */}
          <div className="orders-header">
            <div className="d-flex align-items-center justify-content-between">
              <div className="d-flex align-items-center">
                <div className="header-icon">
                  <i className="bi bi-receipt"></i>
                </div>
                <div className="ms-3">
                  <h2 className="header-title mb-1">My Orders</h2>
                  <p className="header-subtitle mb-0">Track and manage your food orders</p>
                </div>
              </div>

            </div>
          </div>
          
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

          {/* Orders List */}
          {loading ? (
            <div className="loading-state">
              <div className="spinner-border text-primary" role="status">
                <span className="visually-hidden">Loading...</span>
              </div>
              <p className="mt-3 text-muted">Loading your orders...</p>
            </div>
          ) : filteredOrders.length === 0 ? (
            <div className="empty-orders">
              <i className="bi bi-receipt display-1 text-muted mb-3"></i>
              <h4 className="text-muted mb-2">No orders found</h4>
              <p className="text-muted">
                {filter === "All"
                  ? "You haven't placed any orders yet. Start exploring our menu!"
                  : `No ${filter.toLowerCase()} orders found.`}
              </p>
              <button className="btn btn-primary" onClick={() => (window.location.href = "/home")}>
                <i className="bi bi-arrow-left me-2"></i>
                Browse Menu
              </button>
            </div>
          ) : (
            <div className="orders-list">
              {filteredOrders.map((order) => (
                <div key={order.id} className="order-card">
                  <div className="order-header">
                    <div className="order-info">
                      <h5 className="order-id">
                        <i className="bi bi-hash me-1"></i>
                        Order #{order.id}
                      </h5>
                      <div className="order-meta">
                        <span className="order-date">
                          <i className="bi bi-calendar me-1"></i>
                          {new Date(order.orderDate).toLocaleDateString()}
                        </span>
                        <span className="order-time">
                          <i className="bi bi-clock me-1"></i>
                          {new Date(order.orderDate).toLocaleTimeString()}
                        </span>
                      </div>
                    </div>
                    <div className="order-status">
                      <span className={`status-badge ${getStatusBadgeClass(order.status)}`}>{order.status}</span>
                    </div>
                  </div>

                  <div className="order-body">
                    <div className="order-items">
                      <h6 className="items-title">
                        <i className="bi bi-bag me-1"></i>
                        Order Items
                      </h6>
                      <div className="items-list">
                        {order.orderItems.map((item, idx) => (
                          <div key={idx} className="order-item">
                            <div className="item-icon">
                              <i className="bi bi-circle-fill"></i>
                            </div>
                            <div className="item-details">
                              <span className="item-name">{item.menuItem?.name ?? "Deleted Item"}</span>
                              <span className="item-quantity">× {item.quantity}</span>
                            </div>
                          </div>
                        ))}
                      </div>
                    </div>

                    <div className="order-total">
                      <div className="total-amount">
                        <span className="total-label">Total Amount</span>
                        <span className="total-value">₹{order.totalAmount}</span>
                      </div>
                    </div>
                  </div>

                  <div className="order-footer">
                    <div className="order-actions">
                      
                     
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

export default MyOrders
