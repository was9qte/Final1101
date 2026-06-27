using BookStore.APII.Data;
using BookStore.APII.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.APII.Controllers;

/// <summary>
/// Контроллер для управления товарами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _databaseContext;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера товаров.
    /// </summary>
    public ProductsController(AppDbContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    /// <summary>
    /// Возвращает список всех товаров.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            List<Product> products =
                await _databaseContext.Products.ToListAsync();

            return Ok(products);
        }
        catch
        {
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Возвращает товар по его идентификатору.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Product? product =
            await _databaseContext.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    /// <summary>
    /// Создает новый товар.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        _databaseContext.Products.Add(product);

        await _databaseContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    /// <summary>
    /// Обновляет существующий товар.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        var existing = await _databaseContext.Products.FindAsync(id);

        if (existing == null)
            return NotFound();

        existing.Article = product.Article;
        existing.Name = product.Name;
        existing.Unit = product.Unit;
        existing.Price = product.Price;
        existing.Author = product.Author;
        existing.ManufacturerId = product.ManufacturerId;
        existing.CategoryId = product.CategoryId;
        existing.Discount = product.Discount;
        existing.QuantityInStock = product.QuantityInStock;
        existing.Description = product.Description;
        existing.ImagePath = product.ImagePath;

        await _databaseContext.SaveChangesAsync();

        return Ok(existing);
    }

    /// <summary>
    /// Удаляет товар по его идентификатору.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Product? product =
            await _databaseContext.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        _databaseContext.Products.Remove(product);

        await _databaseContext.SaveChangesAsync();

        return Ok();
    }
}