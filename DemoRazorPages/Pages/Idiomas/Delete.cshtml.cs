﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Infraestructure.UoW;
using Domains.Entities;

namespace DemoRazorPages.Pages.Idiomas {
    public class DeleteModel : PageModel
    {
        private readonly TiendaDbContext _context;

        public DeleteModel(TiendaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Culture Culture { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Culture = await _context.Cultures.FirstOrDefaultAsync(m => m.CultureId == id);

            if (Culture == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Culture = await _context.Cultures.FindAsync(id);

            if (Culture != null)
            {
                _context.Cultures.Remove(Culture);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
