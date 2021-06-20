﻿using FoodDiary.Data;
using FoodDiary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodDiary.Repositories.Entities;
using Repositories.Abstract;
using FoodDiary.Repositories.Abstract;

namespace FoodDiary.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _repositoryProduct;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public ProductsController(IProductsRepository productsRepository, UserManager<AppUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _repositoryProduct = productsRepository;
            this._userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        public IActionResult Index(Guid diaryId)
        {
            if (diaryId == Guid.Empty)
            {
                var productsList = _applicationDbContext.ProductEntities.Where(x => x.Kcal > 0).ToList();
                var viewModel = new ProductViewModel()
                {
                    ProductEntities = productsList,
                    DiaryId = Guid.Empty
                };
                return View(viewModel);
            }

            var productViewModel = new ProductViewModel()
            {
                ProductEntities = _applicationDbContext.ProductEntities.Where(x => x.Kcal > 0).ToList(),
                DiaryId = diaryId
            };

            return View(productViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _repositoryProduct.GetProductById(id);
            return View(product);
        }

        public async Task<IActionResult> Update(ProductEntity product)
        {
            await _repositoryProduct.EditProductInDataBase(product, product.Id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _repositoryProduct.DeleteProductFromDataBase(id);
            return RedirectToAction("Index");
        }

        public IActionResult Add()
        {
            return View("Add");
        }

        public async Task<IActionResult> AddNew(ProductEntity productEntity)
        {
            await _repositoryProduct.AddProductToDataBase(productEntity);
            return RedirectToAction("Index");
        }
    }

    public class ProductViewModel
    {
        public List<ProductEntity> ProductEntities { get; set; }
        public Guid DiaryId { get; set; }
    }
}