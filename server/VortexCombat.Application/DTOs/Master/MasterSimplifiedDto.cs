using VortexCombat.Domain.Entities;

namespace VortexCombat.Application.DTOs.Master;

public class MasterSimplifiedDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Belt Belt { get; set; }
}