using MediatR;

namespace CampusEcomSystemMini.Application.Users.Preferences;

public record CreatePreferencesCommand(
    string? InterestedSubjects,
    string? Habits,
    string? Goals,
    string? PreferredRentalArea,
    decimal? MonthlyRentalBudget
) : IRequest<CreatePreferencesResponse?>;