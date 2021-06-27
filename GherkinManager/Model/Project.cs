using System;
using System.ComponentModel.DataAnnotations;

namespace GherkinManager.Model
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Owner { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Project()
        {
            StartDate = DateTime.Today;
            EndDate = new DateTime(2051, 12, 31);
        }
    }
}
