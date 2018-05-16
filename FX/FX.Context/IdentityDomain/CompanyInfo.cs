using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FX.Context.IdentityDomain
{
    [Table("Company")]
    public class CompanyInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(500)]
        [Required]
        public string Name { get; set; }

        [MaxLength(1024)]
        [Required]
        public string ImageIcon { get; set; }
    }
}