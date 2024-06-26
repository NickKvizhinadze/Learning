using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using Xunit.Abstractions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace RiverBooks.OrderProcessing.Tests;

public class InfrastructureDependencyTests(ITestOutputHelper outputHelper)
{
    private readonly ITestOutputHelper _outputHelper = outputHelper;

    private static readonly Architecture _architecture = new ArchLoader()
        .LoadAssemblies(typeof(OrderProcessing.AssemblyInfo).Assembly)
        .Build();

    [Fact]
    public void DomainTypesShouldNotReferenceInfrastructure()
    {
        var domainTypes = Types().That()
            .ResideInNamespace("RiverBooks.OrderProcessing.Domain.*", useRegularExpressions: true)
            .As("OrderProcessing domain types");

        var infrastructureTypes = Types().That()
            .ResideInNamespace("RiverBooks.OrderProcessing.Infrastructure.*", useRegularExpressions: true)
            .As("OrderProcessing infrastructure types");

        var rule = domainTypes.Should().NotDependOnAny(infrastructureTypes);

        PrintTypes(domainTypes, infrastructureTypes);

        rule.Check(_architecture);
    }

    private void PrintTypes(GivenTypesConjunctionWithDescription domainTypes,
        GivenTypesConjunctionWithDescription infrastructureTypes)
    {
        foreach (var domainClass in domainTypes.GetObjects(_architecture))
        {
            _outputHelper.WriteLine($"Domain Type:  {domainClass.FullName}");
            foreach (var dependency in domainClass.Dependencies)
            {
                var targetType = dependency.Target;
                if (infrastructureTypes.GetObjects(_architecture).Any(infraClass => infraClass == targetType))
                    _outputHelper.WriteLine($"Depends on Infrastructure: {targetType.FullName}");
            }
        }

        _outputHelper.WriteLine(infrastructureTypes.Description);
        foreach (var iType in infrastructureTypes.GetObjects(_architecture))
        {
            _outputHelper.WriteLine($"Infrastructure Types  {iType.FullName}");
        }
    }
}