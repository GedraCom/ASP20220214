﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infraestructure.UoW;
using Domains.Entities;

namespace DemosMVC.Controllers
{
    public class ProductoShortDTO {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
    }

    [Route("api/productos")]
    [ApiController]
    public class ProductosApiController : ControllerBase
    {
        private readonly TiendaDbContext _context;

        public ProductosApiController(TiendaDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductosApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoShortDTO>>> GetProducts()
        {
            return await _context.Products.Select(p=> new ProductoShortDTO() {
                Name = p.Name, ProductId = p.ProductId,
                ProductNumber = p.ProductNumber
            }).ToListAsync();
        }

        // GET: api/ProductosApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/ProductosApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // PUT: api/ProductosApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/ProductosApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
