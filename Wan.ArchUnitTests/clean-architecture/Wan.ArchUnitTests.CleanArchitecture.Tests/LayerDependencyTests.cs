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
        Types().That().ResideInAssembly(WebAssembly).As("Web Layer");

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
    
    [Fact]
    public void Application_Should_Only_Depend_On_Core()
    {
        var coreRule = Types()
            .That().Are(ApplicationLayer)
            .And().DependOnAny(CoreLayer)
            .Should().Exist()
            .Because("Application should depend only on Core");
        
        var infrastructureRule = Types()
            .That().Are(ApplicationLayer)
            .Should().NotDependOnAny(InfrastructureLayer)
            .Because("Application should not have dependencies on Infrastructure");
        
        var webRule = Types()
            .That().Are(ApplicationLayer)
            .Should().NotDependOnAny(WebLayer)
            .Because("Application should not have dependencies on Web");

        coreRule
            .And(webRule)
            .And(infrastructureRule)
            .Check(Architecture);
    }
    
    [Fact]
    public void Web_Should_Only_Depend_On_Application_And_Infrastructure()
    {
        var webRule = Types()
            .That().Are(WebLayer)
            .And().DependOnAny(ApplicationLayer)
            .Should().Exist()
            .Because("Web should depend on Application");
        
        var infrastructureRule = Types()
            .That().Are(WebLayer)
            .And().DependOnAny(InfrastructureLayer)
            .Should().Exist()
            .Because("Web should depend on Infrastructure");
        
        var coreRule = Types()
            .That().Are(WebLayer)
            .Should().NotDependOnAny(CoreLayer)
            .Because("Web should not depend on Core");

        webRule
            .And(coreRule)
            .And(infrastructureRule)
            .Check(Architecture);
    }
    
    [Fact]
    public void Infrastructure_Should_Only_Depend_On_Core_And_Application()
    {
        var coreRule = Types()
            .That().Are(InfrastructureLayer)
            .And().DependOnAny(CoreLayer)
            .Should().Exist()
            .Because("Infrastructure should depend on Core");
        
        var applicationRule = Types()
            .That().Are(InfrastructureLayer)
            .And().DependOnAny(ApplicationLayer)
            .Should().Exist()
            .Because("Infrastructure should depend on Application");
        
        var webRule = Types()
            .That().Are(InfrastructureLayer)
            .Should().NotDependOnAny(WebLayer)
            .Because("Infrastructure should not depend on Web");

        coreRule
            .And(webRule)
            .And(applicationRule)
            .Check(Architecture);
    }
}