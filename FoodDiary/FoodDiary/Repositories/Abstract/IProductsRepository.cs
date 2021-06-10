﻿using FoodDiary.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDiary.Repositories.Abstract
{
    public interface IProductsRepository
    {
        Task AddProductToDataBase(ProductEntity productEntity);
        Task<List<ProductEntity>> GetAllProducts();
    }
    
}