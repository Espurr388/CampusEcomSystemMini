using CampusEcomSystemMini.Application.Interfaces;
using MediatR;

namespace CampusEcomSystemMini.Application.Users.Preferences;

public class UpdatePreferencesHandler
    : IRequestHandler<UpdatePreferencesCommand, UpdatePreferencesResponse?>
{
    private readonly IPreferenceRepository _preferenceRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdatePreferencesHandler(
        IPreferenceRepository preferenceRepository,
        ICurrentUserService currentUserService)
    {
        _preferenceRepository = preferenceRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdatePreferencesResponse?> Handle(
        UpdatePreferencesCommand request,
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

        preference.InterestedSubjects =
            PreferenceInput.NormalizeText(
                request.InterestedSubjects);
        preference.Habits =
            PreferenceInput.NormalizeText(request.Habits);
        preference.Goals =
            PreferenceInput.NormalizeText(request.Goals);
        preference.PreferredRentalArea =
            PreferenceInput.NormalizeText(
                request.PreferredRentalArea);
        preference.MonthlyRentalBudget =
            PreferenceInput.NormalizeBudget(
                request.MonthlyRentalBudget);
        preference.UpdatedAt = DateTime.UtcNow;

        _preferenceRepository.UpdateAsync(
            preference,
            cancellationToken);

        await _preferenceRepository.SaveChangesAsync(
            cancellationToken);

        return new UpdatePreferencesResponse(
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