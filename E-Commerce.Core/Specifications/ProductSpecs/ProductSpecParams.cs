﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Specifications.ProductSpecs
{
	public class ProductSpecParams
	{
		private const int maxPageSize = 10;
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
		public int PageIndex { get; set; } = 1;

        private int pageSize = 5;
		public int PageSize
		{
			get{return pageSize;}
			set{pageSize = value> maxPageSize? maxPageSize:value;}
		}

		private string? search;

		public string? Search
		{
			get { return search; }
			set { search = value?.ToLower(); }
		}


	}
}
