using E_Commerce.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specifications.ProductSpecs
{
    public class ProductWithFilterationForCountSpecificarions : BaseSpecifications<Product>
	{
		public ProductWithFilterationForCountSpecificarions(ProductSpecParams specParams)
		: base(P =>
			 (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search))
			  &&
			  (!specParams.BrandId.HasValue || P.BrandId == specParams.BrandId.Value)
			  &&
			  (!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
			  )
		{

		}
	}
}
