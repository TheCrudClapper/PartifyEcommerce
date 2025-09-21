namespace CSOS.Core.Domain.Entities
{
    /// <summary>
    /// Class that defines base properties for all entities in database
    /// </summary>
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEdited { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
