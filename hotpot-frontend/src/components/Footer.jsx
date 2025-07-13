import {
  FaInstagram,
  FaFacebookF,
  FaTwitter,
  FaYoutube,
  FaPhoneAlt,
  FaMapMarkerAlt,
  FaEnvelope,
  FaApple,
  FaGooglePlay,
} from "react-icons/fa"
import "./Footer.css"

const Footer = () => {
  return (
    <footer className="custom-footer">
      <div className="footer-main">
        <div className="container">
          <div className="row g-4">
            {/* Brand Info */}
            <div className="col-lg-4 col-md-6">
              <div className="footer-brand">
                  <h2 className="brand-name">
                  HOTPOT 
                  </h2>
                <p className="brand-tagline">"Delivering happiness to your doorstep"</p>

                <div className="contact-info">
                  <div className="contact-item">
                    <div className="contact-icon">
                      <FaMapMarkerAlt />
                    </div>
                    <span>Tamil Nadu, India</span>
                  </div>
                  <div className="contact-item">
                    <div className="contact-icon">
                      <FaPhoneAlt />
                    </div>
                    <span>+91 98765 43210</span>
                  </div>
                  <div className="contact-item">
                    <div className="contact-icon">
                      <FaEnvelope />
                    </div>
                    <span>support@hotpot.app</span>
                  </div>
                </div>
              </div>
            </div>

            {/* Quick Links */}
            <div className="col-lg-2 col-md-3 col-6">
              <div className="footer-section">
                <h5 className="section-title">Quick Links</h5>
                <ul className="footer-links">
                  <li>
                    <a href="#home">Home</a>
                  </li>
                  <li>
                    <a href="#menu">Menu</a>
                  </li>
                  <li>
                    <a href="#track">Track Order</a>
                  </li>
                  <li>
                    <a href="#offers">Special Offers</a>
                  </li>
                  <li>
                    <a href="#about">About Us</a>
                  </li>
                </ul>
              </div>
            </div>

            {/* Help Section */}
            <div className="col-lg-2 col-md-3 col-6">
              <div className="footer-section">
                <h5 className="section-title">Help & Support</h5>
                <ul className="footer-links">
                  <li>
                    <a href="#faq">FAQ</a>
                  </li>
                  <li>
                    <a href="#contact">Contact Us</a>
                  </li>
                  <li>
                    <a href="#privacy">Privacy Policy</a>
                  </li>
                  <li>
                    <a href="#terms">Terms of Service</a>
                  </li>
                  <li>
                    <a href="#refund">Refund Policy</a>
                  </li>
                </ul>
              </div>
            </div>

            {/* Social & App */}
            <div className="col-lg-4 col-md-12">
              <div className="footer-section">
                <h5 className="section-title">Connect With Us</h5>
                <div className="social-links">
                  <a href="#facebook" className="social-link facebook">
                    <FaFacebookF />
                  </a>
                  <a href="#instagram" className="social-link instagram">
                    <FaInstagram />
                  </a>
                  <a href="#twitter" className="social-link twitter">
                    <FaTwitter />
                  </a>
                  <a href="#youtube" className="social-link youtube">
                    <FaYoutube />
                  </a>
                </div>

                <div className="app-download">
                  <h6 className="download-title">Download Our App</h6>
                  <div className="download-buttons">
                    <a href="#appstore" className="download-btn app-store">
                      <FaApple className="download-icon" />
                      <div className="download-text">
                        <small>Download on the</small>
                        <strong>App Store</strong>
                      </div>
                    </a>
                    <a href="#playstore" className="download-btn google-play">
                      <FaGooglePlay className="download-icon" />
                      <div className="download-text">
                        <small>Get it on</small>
                        <strong>Google Play</strong>
                      </div>
                    </a>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      {/* Footer Bottom */}
      <div className="footer-bottom">
        <div className="container">
          <div className="row align-items-center">
            <div className="col-md-6">
              <p className="copyright">&copy; {new Date().getFullYear()} HOTPOT. All Rights Reserved.</p>
            </div>
            <div className="col-md-6 text-md-end">
              <p className="crafted">
                Crafted with <span className="heart">❤️</span> in Tamil Nadu
              </p>
            </div>
          </div>
        </div>
      </div>
    </footer>
  )
}

export default Footer
