namespace Bakery2048.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsActive { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
            IsActive = true;
        }

        public void UpdateModifiedDate()
        {
            DateModified = DateTime.Now;
        }

        public void Activate()
        {
            IsActive = true;
            UpdateModifiedDate();
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdateModifiedDate();
        }
    }
}
