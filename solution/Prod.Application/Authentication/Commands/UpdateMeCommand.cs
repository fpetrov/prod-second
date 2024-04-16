using ErrorOr;
using MediatR;
using Prod.Application.Authentication.Common;
using Prod.Application.Common.Interfaces.Authentication;
using Prod.Application.Common.Interfaces.Persistence;
using Prod.Domain.Common.Errors;

namespace Prod.Application.Authentication.Commands;

public class UpdateMeCommandHandler(
    IUserRepository userRepository,
    ICountryRepository countryRepository,
    IJwtTokenGenerator tokenGenerator)
    : IRequestHandler<UpdateMeCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(
        UpdateMeCommand request,
        CancellationToken cancellationToken)
    {
        // If CountryCode not exists.
        if (request.CountryCode is not null)
        {
            request.CountryCode = request.CountryCode.ToUpper();
            
            var country = await countryRepository
                .GetByAlpha2(request.CountryCode, cancellationToken);
            
            if (country is null)
                return Errors.Country.InvalidRegions();
        }
        
        var currentUser = (await userRepository
            .GetByLogin(request.Login, cancellationToken))!;
            
        if (request.Image is not null)
            currentUser.Image = request.Image;

        if (request.Phone is not null)
        {
            var duplicatePhone =
                (await userRepository.GetByPhone(request.Phone, cancellationToken)) != null;
            
            if (duplicatePhone)
                return Errors.User.DuplicateData();
            
            currentUser.Phone = request.Phone;
        }
            
        if (request.IsPublic is not null)
            currentUser.IsPublic = request.IsPublic.Value;

        if (request.CountryCode is not null)
            currentUser.CountryCode = request.CountryCode;

        await userRepository.Update(currentUser, cancellationToken);
        
        var token = tokenGenerator.GenerateToken(currentUser);
        
        return new AuthenticationResult(
            currentUser.Login,
            currentUser.Email,
            currentUser.CountryCode,
            currentUser.IsPublic,
            currentUser.Phone,
            currentUser.Image,
            token);
    }
}

public class UpdateMeCommand : IRequest<ErrorOr<AuthenticationResult>>
{
    public required string Login { get; set;}
    public string? CountryCode { get; set; }
    public bool? IsPublic { get; set; }
    public string? Phone { get; set; }
    public string? Image { get; set; }
}