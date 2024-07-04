using MediatR;
using ErrorOr;
using NSubstitute;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GymManagement.Domain.Gyms;
using GymManagement.TestCommon.Gyms;
using GymManagement.Application.Common.Behaviors;
using GymManagement.Application.Gyms.Commands.CreateGym;

namespace GymManagement.Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorTests
{
    private readonly IValidator<CreateGymCommand> _mockValidator;
    private readonly RequestHandlerDelegate<ErrorOr<Gym>> _mockNextBehavior;
    private readonly ValidationBehavior<CreateGymCommand, ErrorOr<Gym>> _validationBehavior;

    public ValidationBehaviorTests()
    {
        _mockValidator = Substitute.For<IValidator<CreateGymCommand>>();
        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<ErrorOr<Gym>>>();
        _validationBehavior = new ValidationBehavior<CreateGymCommand, ErrorOr<Gym>>([_mockValidator]);
    }
    
    
    [Fact]
    public async Task InvokeBehavior_WhenValidationResultIsValid_ShouldInvokeNextBehavior()
    {
        var createGymRequest = GymCommandFactory.CreateCreateGymCommand();
       
        var gym = GymFactory.CreateGym();
        _mockNextBehavior.Invoke().Returns(gym);        
        _mockValidator.ValidateAsync(createGymRequest, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());
        
        var result = await _validationBehavior.Handle(createGymRequest, _mockNextBehavior, default);

        result.IsError.Should().BeFalse();
        result.Value.Should().BeEquivalentTo(gym);
    }
    
    [Fact]
    public async Task InvokeBehavior_WhenValidationResultIsNotValid_ShouldReturnListOfErrors()
    {
        var createGymRequest = GymCommandFactory.CreateCreateGymCommand();

        List<ValidationFailure> validationFailures = [new(propertyName: "foo", errorMessage: "bad foo")];
        
        _mockValidator.ValidateAsync(createGymRequest, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(validationFailures));
        
        var result = await _validationBehavior.Handle(createGymRequest, _mockNextBehavior, default);

        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("foo");
        result.FirstError.Description.Should().Be("bad foo");
    }
}