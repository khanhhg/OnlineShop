﻿using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using WebBanHangOnline.Data.IRepository;


namespace WebBanHangOnline.Controllers
{
    // Top == 0 Get all product
    // Top != 0  Take(Top) product
    public class ProductsController : Controller
    {
        IProductsRepository _productsRepository;
        IProductCategoriesRepository _IProductCategories;
     
        public ProductsController(IProductsRepository productsRepository, IProductCategoriesRepository iProductCategories)
        {
            _productsRepository = productsRepository;
            _IProductCategories = iProductCategories;
        }
        public async Task<ActionResult> Index(int id)
        {          
            var items = await _productsRepository.GetProduts_By_Caterory(id,0);
            ViewBag.CategoryId = id;
            if (id > 0)
            {
                var objCate = await _IProductCategories.Get(id);
                if (objCate != null)
                {
                    ViewBag.CategoryName = objCate.Title;
                }              
            }
            else
            {
                ViewBag.CategoryName = "All";
            }
            return View(items);
        }
		public async Task<ActionResult> Promotion(int id)
		{
            var items = await _productsRepository.GetProduts_Promotion(id,0);
            ViewBag.CategoryId = id;
            if (id > 0)
            {
                var objCate = await _IProductCategories.Get(id);
                if (objCate != null)
                {
                    ViewBag.CategoryName = objCate.Title;
                }
            }
            else
            {
                ViewBag.CategoryName = "All";
            }
            return View(items);
		}
		public async Task<ActionResult> Detail(int id)
        {
            var item = await _productsRepository.Get(id);
            if (item != null)
            {
                await _productsRepository.Update_ViewCount(item);
            }          
            return View(item);
        }
        public async Task<ActionResult> ProductCategory(int id )
        {
            var items = await _productsRepository.GetProduts_By_Caterory(id,0);
            return View(items);
        }
    }
}
