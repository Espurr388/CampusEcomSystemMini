using CampusEcomSystemMini.Application.Interfaces;
using MediatR;

namespace CampusEcomSystemMini.Application.Users.Preferences;

public class GetPreferencesHandler
    : IRequestHandler<GetPreferencesQuery, GetPreferencesResponse?>
{
    private readonly IPreferenceRepository _preferenceRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetPreferencesHandler(
        IPreferenceRepository preferenceRepository,
        ICurrentUserService currentUserService)
    {
        _preferenceRepository = preferenceRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetPreferencesResponse?> Handle(
        GetPreferencesQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var preference =
            await _preferenceRepository.GetByUserIdAsync(
                userId,
                cancellationToken);

        if (preference is null)
        {
            return null;
        }

        return new GetPreferencesResponse(
            preference.Id,
            preference.UserId,
            preference.InterestedSubjects,
            preference.Habits,
            preference.Goals,
            preference.PreferredRentalArea,
            preference.MonthlyRentalBudget,
            preference.CreatedAt,
            preference.UpdatedAt);
    }
}