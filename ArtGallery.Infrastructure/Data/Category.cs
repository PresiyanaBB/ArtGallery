using ArtGallery.Infrastructure.Data.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Infrastructure.Data
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    
        [Required]
        [StringLength(ValidationConstants.CategoryNameMaxLength)]
        private string Label { get; set; }
    }
}
