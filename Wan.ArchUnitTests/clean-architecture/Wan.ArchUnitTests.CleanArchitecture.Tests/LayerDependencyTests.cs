using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Wan.ArchUnitTests.CleanArchitecture.Tests;

public class LayerDependencyTests
{
    private static readonly System.Reflection.Assembly CoreAssembly =
        System.Reflection.Assembly.Load("Wan.ArchUnitTests.CleanArchitecture.Core");

    private static readonly System.Reflection.Assembly ApplicationAssembly =
        System.Reflection.Assembly.Load("Wan.ArchUnitTests.CleanArchitecture.Application");

    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssemblies(
            CoreAssembly,
            ApplicationAssembly
        ).Build();

    private static readonly IObjectProvider<IType> CoreLayer =
        Types().That().ResideInAssembly(CoreAssembly).As("Core Layer");

    private static readonly IObjectProvider<IType> ApplicationLayer =
        Types().That().ResideInAssembly(ApplicationAssembly).As("Application Layer");

    [Fact]
    public void Core_Should_Not_Depend_On_Application()
    {
        var rule = Types()
            .That().Are(CoreLayer)
            .Should().NotDependOnAny(ApplicationLayer)
            .Because("Core should not have dependencies on Application layer");

        rule.Check(Architecture);
    }
}