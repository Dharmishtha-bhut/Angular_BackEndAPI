using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Specification
{
    public class BrandListSpecification :BaseSpecification<Product,string>
    {
        public BrandListSpecification() : base(null)
        {
            AddSelect(p => p.Brand);
            ApplyDistinct();
        }
    }
}
