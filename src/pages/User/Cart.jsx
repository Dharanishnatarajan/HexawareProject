"use client"
import { useEffect, useState } from "react"
import { getCart, updateCartItem, removeCartItem } from "../../api/cartService"
import { placeOrder } from "../../api/orderService"
import { toast } from "react-hot-toast"
import { useNavigate } from "react-router-dom"
import UserNavbar from "../../components/UserNavbar"
import "./Cart.css"

const Cart = () => {
  const [cartItems, setCartItems] = useState([])
  const [address, setAddress] = useState("")
  const [phoneNumber, setPhoneNumber] = useState("")
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()

  useEffect(() => {
    fetchCart()
  }, [])

  const fetchCart = async () => {
    try {
      const res = await getCart()
      setCartItems(res.items || [])
    } catch {
      toast.error("Failed to load cart")
    }
  }

  const getTotal = () => {
    return cartItems.reduce((acc, item) => acc + item.price * item.quantity, 0)
  }

  const handleQuantityChange = async (menuItemId, newQty) => {
    if (newQty < 1) return
    try {
      await updateCartItem(menuItemId, newQty)
      fetchCart()
      toast.success("Quantity updated!")
    } catch {
      toast.error("Failed to update quantity")
    }
  }

  const handleRemove = async (menuItemId) => {
    try {
      await removeCartItem(menuItemId)
      fetchCart()
      toast.success("Item removed from cart")
    } catch (err) {
      console.error("Remove failed:", err.response?.data || err.message)
      toast.error(err.response?.data?.message || "Failed to remove item")
    }
  }

  const handleCheckout = async () => {
    if (!address || !phoneNumber) {
      toast.error("Address and phone number are required")
      return
    }
    
    setLoading(true)
    try {
      await placeOrder(address, phoneNumber)
      toast.success("Order placed successfully!")
      navigate("/home")
    } catch (err) {
      toast.error(err.response?.data?.message || "Checkout failed")
    } finally {
      setLoading(false)
    }
  }

  return (
    <>
      <UserNavbar />
      <div className="cart-container">
        <div className="container py-4">
          {/* Cart Header */}
          <div className="cart-header">
            <div className="d-flex align-items-center">
              <div className="cart-icon">
                <i className="bi bi-cart3"></i>
              </div>
              <div className="ms-3">
                <h1 className="cart-title mb-1">Your Shopping Cart</h1>
                <p className="cart-subtitle mb-0">
                  {cartItems.length} {cartItems.length === 1 ? "item" : "items"} in your cart
                </p>
              </div>
            </div>
          </div>

          {cartItems.length === 0 ? (
            <div className="empty-cart">
              <div className="empty-cart-icon">
                <i className="bi bi-cart-x"></i>
              </div>
              <h3 className="empty-cart-title">Your cart is empty</h3>
              <p className="empty-cart-text">Looks like you haven't added any items to your cart yet.</p>
              <button className="btn browse-btn" onClick={() => navigate("/home")}>
                <i className="bi bi-arrow-left me-2"></i>
                Browse Menu
              </button>
            </div>
          ) : (
            <div className="row g-4">
              {/* Cart Items */}
              <div className="col-lg-8">
                <div className="cart-items-card">
                  <div className="cart-items-header">
                    <h4 className="mb-0">
                      <i className="bi bi-bag-check me-2"></i>
                      Order Items
                    </h4>
                  </div>
                  <div className="cart-items-body">
                    {cartItems.map((item, index) => (
                      <div key={item.menuItemId} className={`cart-item ${index !== cartItems.length - 1 ? "border-bottom" : ""}`}>
                        <div className="item-info">
                          <div className="item-image">
                            <i className="bi bi-image"></i>
                          </div>
                          <div className="item-details">
                            <h5 className="item-name">{item.menuItemName}</h5>
                            <div className="item-price-info">
                              <span className="unit-price">₹{item.price}</span>
                              <span className="multiply">×</span>
                              <span className="quantity">{item.quantity}</span>
                              <span className="equals">=</span>
                              <span className="total-price">₹{item.price * item.quantity}</span>
                            </div>
                          </div>
                        </div>

                        <div className="item-controls">
                          <div className="quantity-controls">
                            <button
                              className="btn quantity-btn minus"
                              onClick={() => handleQuantityChange(item.menuItemId, item.quantity - 1)}
                              disabled={item.quantity <= 1}
                            >
                              <i className="bi bi-dash"></i>
                            </button>
                            <span className="quantity-display">{item.quantity}</span>
                            <button
                              className="btn quantity-btn plus"
                              onClick={() => handleQuantityChange(item.menuItemId, item.quantity + 1)}
                            >
                              <i className="bi bi-plus"></i>
                            </button>
                          </div>
                          <button
                            className="btn remove-btn"
                            onClick={() => handleRemove(item.menuItemId)}
                            title="Remove item"
                          >
                            <i className="bi bi-trash"></i>
                          </button>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>
              </div>

              {/* Order Summary & Checkout */}
              <div className="col-lg-4">
                <div className="checkout-card">
                  <div className="checkout-header">
                    <h4 className="mb-0">
                      <i className="bi bi-receipt me-2"></i>
                      Order Summary
                    </h4>
                  </div>
                  <div className="checkout-body">
                    <div className="order-summary">
                      <div className="summary-row">
                        <span>Subtotal ({cartItems.length} items)</span>
                        <span>₹{getTotal()}</span>
                      </div>
                      <div className="summary-row">
                        <span>Delivery Fee</span>
                        <span className="text-success">Free</span>
                      </div>
                      <div className="summary-row total-row">
                        <span>Total Amount</span>
                        <span>₹{getTotal()}</span>
                      </div>
                    </div>

                    <div className="delivery-info">
                      <h5 className="delivery-title">
                        <i className="bi bi-truck me-2"></i>
                        Delivery Information
                      </h5>
                      
                      <div className="form-group">
                        <label className="form-label">
                          <i className="bi bi-geo-alt me-1"></i>
                          Delivery Address
                        </label>
                        <textarea
                          className="form-control delivery-input"
                          placeholder="Enter your complete delivery address"
                          value={address}
                          onChange={(e) => setAddress(e.target.value)}
                          rows="3"
                          required
                        />
                      </div>

                      <div className="form-group">
                        <label className="form-label">
                          <i className="bi bi-telephone me-1"></i>
                          Phone Number
                        </label>
                        <input
                          type="tel"
                          className="form-control delivery-input"
                          placeholder="Enter your phone number"
                          value={phoneNumber}
                          onChange={(e) => setPhoneNumber(e.target.value)}
                          required
                        />
                      </div>
                    </div>

                    <button
                      onClick={handleCheckout}
                      className="btn checkout-btn"
                      disabled={loading || !address || !phoneNumber}
                    >
                      {loading ? (
                        <>
                          <span className="spinner-border spinner-border-sm me-2"></span>
                          Processing...
                        </>
                      ) : (
                        <>
                          <i className="bi bi-credit-card me-2"></i>
                          Place Order - ₹{getTotal()}
                        </>
                      )}
                    </button>
                  </div>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </>
  )
}

export default Cart
