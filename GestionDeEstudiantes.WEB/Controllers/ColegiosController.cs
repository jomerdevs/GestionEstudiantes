using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionDeEstudiantes.WEB.Data;
using GestionDeEstudiantes.WEB.Entities;

namespace GestionDeEstudiantes.WEB.Controllers
{
    public class ColegiosController : Controller
    {
        private readonly GestionDeEstudiantesContext _context;

        public ColegiosController(GestionDeEstudiantesContext context)
        {
            _context = context;
        }

        // GET: Colegios
        public async Task<IActionResult> Index()
        {
              return View(await _context.Colegios.ToListAsync());
        }

        // GET: Colegios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Colegios == null)
            {
                return NotFound();
            }

            var colegios = await _context.Colegios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colegios == null)
            {
                return NotFound();
            }

            return View(colegios);
        }

        // GET: Colegios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Colegios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Description,Activo,FechaCreacion")] Colegios colegios)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colegios);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(colegios);
        }

        // GET: Colegios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Colegios == null)
            {
                return NotFound();
            }

            var colegios = await _context.Colegios.FindAsync(id);
            if (colegios == null)
            {
                return NotFound();
            }
            return View(colegios);
        }

        // POST: Colegios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Description,Activo,FechaCreacion")] Colegios colegios)
        {
            if (id != colegios.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colegios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColegiosExists(colegios.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(colegios);
        }

        // GET: Colegios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Colegios == null)
            {
                return NotFound();
            }

            var colegios = await _context.Colegios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (colegios == null)
            {
                return NotFound();
            }

            return View(colegios);
        }

        // POST: Colegios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Colegios == null)
            {
                return Problem("Entity set 'GestionDeEstudiantesContext.Colegios'  is null.");
            }
            var colegios = await _context.Colegios.FindAsync(id);
            if (colegios != null)
            {
                _context.Colegios.Remove(colegios);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColegiosExists(int id)
        {
          return _context.Colegios.Any(e => e.Id == id);
        }
    }
}
