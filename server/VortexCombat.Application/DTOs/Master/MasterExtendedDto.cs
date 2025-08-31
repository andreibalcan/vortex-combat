using VortexCombat.Domain.Entities;

namespace VortexCombat.Application.DTOs.Master;

public class MasterExtendedDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Address? Address { get; set; }
    public EGender Gender { get; set; }
    public DateTime Birthday { get; set; }
    public Belt Belt { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public bool HasTrainerCertificate { get; set; }
}