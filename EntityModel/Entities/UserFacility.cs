namespace OfflineSynchronizationPOC.EntityModel.Entities;

public class UserFacility
{
    public required uint UserId { get; set; }
    public User User { get; set; }

    public required uint FacilityId { get; set; }
    public Facility Facility { get; set; }

    public required FacilityRole Role { get; set; }
}