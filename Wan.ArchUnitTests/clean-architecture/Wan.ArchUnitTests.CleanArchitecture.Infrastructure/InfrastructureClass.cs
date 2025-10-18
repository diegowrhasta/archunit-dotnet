using Wan.ArchUnitTests.CleanArchitecture.Application;
using Wan.ArchUnitTests.CleanArchitecture.Core;

namespace Wan.ArchUnitTests.CleanArchitecture.Infrastructure;

public class InfrastructureClass
{
    public CoreClass? CoreClass { get; set; }
    public ApplicationClass? ApplicationClass { get; set; }
}