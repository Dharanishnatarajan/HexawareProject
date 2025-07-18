﻿namespace HotPot.DTOs
{
    public class LoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Role { get; set; } 
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }

    }
}