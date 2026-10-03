using CampusEcomSystemMini.Application.Interfaces;
using CampusEcomSystemMini.Domain.Entities;
using MediatR;

namespace CampusEcomSystemMini.Application.Users.Preferences;

public class CreatePreferencesHandler
    : IRequestHandler<CreatePreferencesCommand, CreatePreferencesResponse?>
{
    private readonly IPreferenceRepository _preferenceRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreatePreferencesHandler(
        IPreferenceRepository preferenceRepository,
        ICurrentUserService currentUserService)
    {
        _preferenceRepository = preferenceRepository;
        _currentUserService = currentUserService;
    }

    public async Task<CreatePreferencesResponse?> Handle(
        CreatePreferencesCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var existingPreference =
            await _preferenceRepository.GetByUserIdAsync(
                userId,
                cancellationToken);

        if (existingPreference is not null)
        {
            return null;
        }

        var now = DateTime.UtcNow;

        var preference = new Preference
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            InterestedSubjects =
                PreferenceInput.NormalizeText(
                    request.InterestedSubjects),
            Habits =
                PreferenceInput.NormalizeText(request.Habits),
            Goals =
                PreferenceInput.NormalizeText(request.Goals),
            PreferredRentalArea =
                PreferenceInput.NormalizeText(
                    request.PreferredRentalArea),
            MonthlyRentalBudget =
                PreferenceInput.NormalizeBudget(
                    request.MonthlyRentalBudget),
            CreatedAt = now,
            UpdatedAt = now
        };

        await _preferenceRepository.AddAsync(
            preference,
            cancellationToken);

        await _preferenceRepository.SaveChangesAsync(
            cancellationToken);

        return new CreatePreferencesResponse(
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