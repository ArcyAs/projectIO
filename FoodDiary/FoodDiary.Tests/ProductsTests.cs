﻿using FluentAssertions;
using FluentAssertions.Common;
using FoodDiary.Data;
using FoodDiary.Repositories.Entities;
using FoodDiary.Repositories.Implementations;
using FoodDiary.Services.Abstract;
using FoodDiary.Services.Implementation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FoodDiary.Tests
{
    public class ProductsTests
    {
        private ApplicationDbContext _context;
        private List<ProductEntity> productEntities = new List<ProductEntity>()
        {
            new ProductEntity()
            {
                ProductName ="Banana",
                Carb = 10,
                Protein = 10,
                Fat = 3,
                Kcal =  KcalCalculatorService.KcalCalculator(10, 10, 3)
            },
            new ProductEntity()
            {
                ProductName ="Apple",
                Carb = 10,
                Protein = 10,
                Fat = 3,
                Kcal = KcalCalculatorService.KcalCalculator(10,10,3)
            }
        };

        public ProductsTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;

            _context = new ApplicationDbContext(options);
            _context.ProductEntities.AddRange(productEntities);
            _context.SaveChanges();
        }


        [Fact]
        public async Task ShouldGetAllProductsFromContext()
        {
            var productsRepository = new ProductsRepository(_context);
            var result = (await productsRepository.GetAllProducts()).ToList();

            result.Should().HaveCount(2);
            result.Should().BeOfType<List<ProductEntity>>();
            result[0].Should().BeEquivalentTo(productEntities[0]);
            result[1].Should().BeEquivalentTo(productEntities[1]);
           
        }

        [Fact]
        public async Task ShouldGetSavedProduct()
        {
            var thirdObjest = new ProductEntity()
            {
                ProductName = "Orange",
                Carb = 10,
                Protein = 10,
                Fat = 3,
                Kcal = KcalCalculatorService.KcalCalculator(10, 10, 3)
            };

            var productsRepository = new ProductsRepository(_context);
            await productsRepository.AddProductToDataBase(thirdObjest);
            var products = await productsRepository.GetAllProducts();

            var allProducts = products.Should().HaveCount(3);
            products.LastOrDefault().Should().BeEquivalentTo(thirdObjest);
            products.LastOrDefault().Kcal.Should().Be(KcalCalculatorService.KcalCalculator(10, 10, 3));
        }
    }
}