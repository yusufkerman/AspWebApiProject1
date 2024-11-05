
namespace Entities.Models
{
    public record ProjectListing
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        // Navigation property for related SkillRoles
        public List<SkillRole> RequiredRoles { get; set; }
    }
}
