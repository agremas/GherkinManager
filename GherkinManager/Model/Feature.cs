using System.ComponentModel.DataAnnotations;

namespace GherkinManager.Model
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        public int ProjectId { get; set; }
    }
}
