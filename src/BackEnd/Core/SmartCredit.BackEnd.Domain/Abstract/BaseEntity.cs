using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCredit.BackEnd.Domain.Base
{
    public abstract class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
        public DateTime CreatedAt {get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt {get; set; }
        public DateTime? DeletedAt {get; set; }
    }
}