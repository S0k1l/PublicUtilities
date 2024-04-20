using System.ComponentModel.DataAnnotations;

namespace PublicUtilities.Models
{
    public class GarbageRemoval
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
