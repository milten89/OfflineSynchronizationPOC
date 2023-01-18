namespace OfflineSynchronizationPOC.EntityModel.Entities
{
    public class File
    {
        public uint Id { get; set; }
        public required Guid Uuid { get; set; }
        public required string Path { get; set; }
        public required string Mime { get; set; }
    }
}
