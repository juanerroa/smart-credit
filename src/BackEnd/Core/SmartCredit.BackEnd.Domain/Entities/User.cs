using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SmartCredit.BackEnd.Domain.Base;

namespace SmartCredit.BackEnd.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string FullName { get; set; } // Nombre
        public string Address { get; set; } // Dirección
        public string City { get; set; } // Ciudad
        public string State { get; set; } // Estado
        public string Country { get; set; } // País

        [Required]
        [EmailAddress]
        public string Email { get; set; } // Correo electrónico
        
        //Navigation relationships
        public virtual ICollection<CreditCard> CreditCards { get; set;}
    }
}