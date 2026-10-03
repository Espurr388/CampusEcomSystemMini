namespace CampusEcomSystemMini.Application.Users.Preferences;

public record UpdatePreferencesResponse(
    Guid Id,
    Guid UserId,
    string? InterestedSubjects,
    string? Habits,
    string? Goals,
    string? PreferredRentalArea,
    decimal? MonthlyRentalBudget,
    DateTime CreatedAt,
    DateTime UpdatedAt
);