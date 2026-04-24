using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Profiles
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Products
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            // Category
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
        }
    }
}
