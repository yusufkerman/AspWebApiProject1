
namespace Entities.Models
{
    public record SkillRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        // Foreign key to the related ProjectListing
        public int ProjectListingId { get; set; }

        // Navigation property back to ProjectListing
        public ProjectListing ProjectListing { get; set; }
    }
}
