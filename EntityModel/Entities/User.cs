namespace OfflineSynchronizationPOC.EntityModel.Entities
{
    public class User
    {
        public uint Id { get; set; }
        public required Guid Uuid { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public File? Avatar { get; set; }
        
        public ICollection<Facility> Facilities { get; set; }
        public List<UserFacility> UserFacilities { get; set; }
    }
}
