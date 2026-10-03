namespace CampusEcomSystemMini.Domain.Entities;

// Vector nhu cầu / sở thích của người dùng.
// Module 2 sẽ đọc dữ liệu này cho Smart Matching.
public class Preference
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string? InterestedSubjects { get; set; }

    public string? Habits { get; set; }

    public string? Goals { get; set; }

    public string? PreferredRentalArea { get; set; }

    public decimal? MonthlyRentalBudget { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}