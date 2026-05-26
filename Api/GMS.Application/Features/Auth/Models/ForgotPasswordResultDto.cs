namespace GMS.Application.Features.Auth.Models;

public sealed record ForgotPasswordResultDto(bool RequestAccepted, string DeliveryChannel);
