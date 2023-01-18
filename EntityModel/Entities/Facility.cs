namespace OfflineSynchronizationPOC.EntityModel.Entities;

public class Facility
{
    public uint Id { get; set; }
    public required Guid Uuid { get; set; }
    public required string Name { get; set; }
    public File? Logo { get; set; }

    public ICollection<User> Users { get; set; }
    public List<UserFacility> UserFacilities { get; set; }
}