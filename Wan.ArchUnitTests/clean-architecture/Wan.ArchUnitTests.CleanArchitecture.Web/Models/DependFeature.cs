using Wan.ArchUnitTests.CleanArchitecture.Application;
using Wan.ArchUnitTests.CleanArchitecture.Infrastructure;

namespace Wan.ArchUnitTests.CleanArchitecture.Web.Models;

public class DependFeature
{
    public ApplicationClass? ApplicationClass { get; set; }
    public InfrastructureClass? InfrastructureClass { get; set; }
}