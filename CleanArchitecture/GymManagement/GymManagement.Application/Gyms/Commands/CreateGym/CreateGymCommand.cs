using ErrorOr;
using GymManagement.Application.Common.Authorization;
using MediatR;
using GymManagement.Domain.Gyms;

namespace GymManagement.Application.Gyms.Commands.CreateGym;

[Authorize(Permissions= "gyms:create")]
public record CreateGymCommand(string Name, Guid SubscriptionId) : IRequest<ErrorOr<Gym>>;