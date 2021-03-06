﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FX.Context.IdentityDomain
{
    [Table("Groups")]
    public class Group
    {
        public Group()
        {
            this.Roles = new HashSet<Role>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ComId { get; set; }

        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        [MaxLength(128)]
        [Required]
        public string Descreption { get; set; }

        [MaxLength(128)]
        [Column(TypeName = "varchar")]
        public string CreateBy { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        [MaxLength(128)]
        [Column(TypeName = "varchar")]
        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}