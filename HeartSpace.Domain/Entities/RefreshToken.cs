namespace HeartSpace.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Foreign Key
        public Guid UserId { get; set; }
        public User User { get; set; }

        // Computed properties
        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public bool IsActive => !IsRevoked && !IsExpired;

    }
}
