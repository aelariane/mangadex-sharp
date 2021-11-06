using System;

namespace MangaDexSharp
{
    /// <summary>
    /// Represents base class for common used MangaDex resources (entities) which have additinal information about them
    /// </summary>
    public class MangaDexAuditableResource : MangaDexResource
    {
        /// <summary>
        /// Timestamp when resource was created
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Timestamp when resource was last updated
        /// </summary>
        public DateTime UpdatedAt { get; }
        
        internal MangaDexAuditableResource(
            MangaDexClient client,
            Guid id, 
            DateTime createdAt,
            DateTime updatedAt)
            : base(client, id)
        {
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
