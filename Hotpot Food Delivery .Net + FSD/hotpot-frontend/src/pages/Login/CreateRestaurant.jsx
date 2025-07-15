"use client";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "../../api/axios";
import { toast } from "react-hot-toast";
import "./CreateRestaurant.css";

const CreateRestaurant = () => {
  const [form, setForm] = useState({
    name: "",
    description: "",
    location: "",
    contactNumber: "",
  });

  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  // 🔍 Check if restaurant already exists for logged-in user
  useEffect(() => {
    const checkExistingRestaurant = async () => {
      try {
        const res = await axios.get("/restaurant");
        const currentUserId = localStorage.getItem("userId");
        const existing = res.data.find(
          (r) => r.userId === Number(currentUserId)
        );
        if (existing) {
          toast.success("Restaurant already exists. Redirecting to dashboard...");
          localStorage.setItem("restaurantId", existing.id);
          navigate("/restaurant/dashboard");
        }
      } catch {
        toast.error("Failed to check restaurant");
      }
    };
    checkExistingRestaurant();
  }, [navigate]);

  // 🔄 Handle input change
  const handleChange = (e) => {
    const { name, value } = e.target;

    // ✨ Restrict contactNumber to digits only
    if (name === "contactNumber") {
      const digitsOnly = value.replace(/\D/g, ""); // Remove non-digit characters
      if (digitsOnly.length > 10) return; // Max 10 digits
      setForm({ ...form, [name]: digitsOnly });
    } else {
      setForm({ ...form, [name]: value });
    }
  };

  // ✅ 10-digit number validation (must start with 6,7,8, or 9)
  const isValidPhoneNumber = (number) => /^[6-9]\d{9}$/.test(number);

  // 🚀 Submit form
  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    // Validate contact number
    if (!isValidPhoneNumber(form.contactNumber)) {
      toast.error(
        "Invalid contact number. Must be a 10-digit number starting with 6, 7, 8, or 9."
      );
      setLoading(false);
      return;
    }

    try {
      const res = await axios.post("/restaurant", form);
      localStorage.setItem("restaurantId", res.data.id);
      toast.success("Restaurant created successfully!");
      navigate("/restaurant/dashboard");
    } catch (err) {
      toast.error(err.response?.data || "Failed to create restaurant");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="create-restaurant-container">
      <div className="create-restaurant-background"></div>

      <div className="create-restaurant-card">
        <div className="restaurant-header">
          <div className="restaurant-icon">
            <i className="bi bi-shop"></i>
          </div>
          <h2 className="restaurant-title">Create Your Restaurant</h2>
          <p className="restaurant-subtitle">
            Let's put your food business on the map!
          </p>
        </div>

        <form onSubmit={handleSubmit} className="restaurant-form">
          <div className="form-group">
            <label className="form-label">
              <i className="bi bi-shop me-2"></i>Restaurant Name
            </label>
            <input
              type="text"
              name="name"
              className="form-control custom-input"
              placeholder="e.g. McDonald's"
              value={form.name}
              onChange={handleChange}
              required
            />
          </div>

          <div className="form-group">
            <label className="form-label">
              <i className="bi bi-card-text me-2"></i>Description
            </label>
            <textarea
              name="description"
              className="form-control custom-input"
              placeholder="A brief description of your restaurant"
              rows="3"
              value={form.description}
              onChange={handleChange}
              required
            />
          </div>

          <div className="form-group">
            <label className="form-label">
              <i className="bi bi-geo-alt me-2"></i>Location
            </label>
            <input
              type="text"
              name="location"
              className="form-control custom-input"
              placeholder="City, Area"
              value={form.location}
              onChange={handleChange}
              required
            />
          </div>

          <div className="form-group">
            <label className="form-label">
              <i className="bi bi-telephone me-2"></i>Contact Number
            </label>
            <input
              type="text"
              name="contactNumber"
              className="form-control custom-input"
              placeholder="e.g. 9876543210"
              value={form.contactNumber}
              onChange={handleChange}
              maxLength="10"
              required
            />
          </div>

          <button type="submit" className="btn create-btn" disabled={loading}>
            {loading ? (
              <>
                <span className="spinner-border spinner-border-sm me-2"></span>
                Creating Restaurant...
              </>
            ) : (
              <>
                <i className="bi bi-plus-circle me-2"></i>
                Create Restaurant
              </>
            )}
          </button>
        </form>
      </div>
    </div>
  );
};

export default CreateRestaurant;
