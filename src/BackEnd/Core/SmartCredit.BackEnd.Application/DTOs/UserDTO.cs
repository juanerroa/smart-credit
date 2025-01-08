using SmartCredit.BackEnd.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Application.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } // Nombre
        public string Address { get; set; } // Dirección
        public string City { get; set; } // Ciudad
        public string State { get; set; } // Estado
        public string Country { get; set; } // País
        public string Email { get; set; } // Correo electrónico

    }
}
