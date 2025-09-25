using HeartSpace.Domain.Exception;

namespace HeartSpace.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateOnly? DateOfBirth { get; set; }
        public string? Identifier { get; set; } = null;
        public string? Avatar { get; set; }
        public bool? Gender { get; set; }
        public Role UserRole { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public enum Role
        {
            Admin = 1,
            Client = 2,
            Consultant = 3
        }

        // Navigation property - NOT a database column
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public virtual ClientProfile? ClientProfile { get; set; }

        public virtual ConsultantProfile? ConsultantProfile { get; set; }
        //consultant schedule
        public virtual ICollection<Schedule> ConsultantSchedules { get; set; } = new List<Schedule>();
        public virtual ICollection<Appointment> ClientAppointments { get; set; } = new List<Appointment>();
        public virtual ICollection<Appointment> ConsultantAppointments { get; set; } = new List<Appointment>();


        // Business Logic Methods (không có validation)
        public void Activate()
        {
            if (IsActive)
                throw new BusinessRuleViolationException("User is already active");

            IsActive = true;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new BusinessRuleViolationException("User is already inactive");

            IsActive = false;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void CheckCanLogin()
        {
            if (!IsActive)
                throw new UserInactiveException();
        }

        public bool IsAdult()
        {
            var age = DateTime.Today.Year - DateOfBirth.Value.Year;
            if (DateOfBirth.Value.DayOfYear > DateTime.Today.DayOfYear)
                age--;

            return age >= 18;
        }
    }
}
