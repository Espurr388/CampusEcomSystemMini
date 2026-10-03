using MediatR;

namespace CampusEcomSystemMini.Application.Users.Preferences;

public record UpdatePreferencesCommand(
    string? InterestedSubjects,
    string? Habits,
    string? Goals,
    string? PreferredRentalArea,
    decimal? MonthlyRentalBudget
) : IRequest<UpdatePreferencesResponse?>;