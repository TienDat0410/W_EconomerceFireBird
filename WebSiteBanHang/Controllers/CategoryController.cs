using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebSiteBanHang.Interfaces;
using WebSiteBanHang.Models;

namespace WebSiteBanHang.Controllers {
    public class CategoryController : Controller {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(IProductRepository productRepository, ICategoryRepository categoryRepository) {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index() {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null) {
                return NotFound();
            }
            return View(categories);
        }

        // Hiển thị form thêm sản phẩm mới
        public async Task<IActionResult> AddAsync() {
            var categories = await _categoryRepository.GetAllAsync();
           ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Add(Category category) {
            //Kiểm tra xem ModelState là gì
            //if (ModelState.IsValid) {
            //    await _categoryRepository.AddAsync(category);
            //    return RedirectToAction(nameof(Index));
            //}
            await _categoryRepository.AddAsync(category);
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return RedirectToAction(nameof(Index));
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            
            //return View(category);
        }

        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int id) {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) {
                return NotFound();
            }
            return View(category);
        }

        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Edit(int id) {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) {
                return NotFound();
            }
            return View(category);
        }

        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category) {
            if (id != category.Id) {
                return NotFound();
            }

        //    if (ModelState.IsValid) {
                await _categoryRepository.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
         //   }
         //   return View(category);
        }

        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id) {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) {
                return NotFound();
            }
            return View(category);
        }

        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            await _categoryRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

