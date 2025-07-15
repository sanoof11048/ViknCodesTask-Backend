using System.ComponentModel.DataAnnotations;

namespace ViknCodesTask.Common
{
    public class BaseEntity
    {
        [Required]
        public Guid CreatedBy { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public Guid? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
