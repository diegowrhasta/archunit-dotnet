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

    private static readonly System.Reflection.Assembly WebAssembly = 
        System.Reflection.Assembly.Load("Wan.ArchUnitTests.CleanArchitecture.Web");
    
    private static readonly System.Reflection.Assembly InfrastructureAssembly =
        System.Reflection.Assembly.Load("Wan.ArchUnitTests.CleanArchitecture.Infrastructure");
    
    private static readonly Architecture Architecture =
        new ArchLoader().LoadAssemblies(
            CoreAssembly,
            ApplicationAssembly,
            InfrastructureAssembly,
            WebAssembly
        ).Build();

    private static readonly IObjectProvider<IType> CoreLayer =
        Types().That().ResideInAssembly(CoreAssembly).As("Core Layer");

    private static readonly IObjectProvider<IType> ApplicationLayer =
        Types().That().ResideInAssembly(ApplicationAssembly).As("Application Layer");
    
    private static readonly IObjectProvider<IType> InfrastructureLayer =
        Types().That().ResideInAssembly(InfrastructureAssembly).As("Infrastructure Layer");

    private static readonly IObjectProvider<IType> WebLayer =
        Types().That().ResideInAssembly(ApplicationAssembly).As("Web Layer");

    [Fact]
    public void Core_Should_Not_Depend_On_Any_Layer()
    {
        var webRule = Types()
            .That().Are(CoreLayer)
            .Should().NotDependOnAny(WebLayer)
            .Because("Core should not have dependencies on Web Layer");
        
        var infrastructureRule = Types()
            .That().Are(CoreLayer)
            .Should().NotDependOnAny(InfrastructureLayer)
            .Because("Core should not have dependencies on Infrastructure Layer");
        
        var applicationRule = Types()
            .That().Are(CoreLayer)
            .Should().NotDependOnAny(ApplicationLayer)
            .Because("Core should not have dependencies on Application layer");

        webRule
            .And(applicationRule)
            .And(infrastructureRule)
            .Check(Architecture);
    }
}