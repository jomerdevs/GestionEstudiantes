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
    public class MateriasController : Controller
    {
        private readonly GestionDeEstudiantesContext _context;

        public MateriasController(GestionDeEstudiantesContext context)
        {
            _context = context;
        }

        // GET: Materias
        public async Task<IActionResult> Index()
        {
            var gestionDeEstudiantesContext = _context.Materias.Include(m => m.IdUsuarioNavigation);
            return View(await gestionDeEstudiantesContext.ToListAsync());
        }

        // GET: Materias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Materias == null)
            {
                return NotFound();
            }

            var materias = await _context.Materias
                .Include(m => m.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materias == null)
            {
                return NotFound();
            }

            return View(materias);
        }

        // GET: Materias/Create
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Clave");
            return View();
        }

        // POST: Materias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdUsuario,Nombre,Description,Activo,FechaCreacion")] Materias materias)
        {
            if (ModelState.IsValid)
            {
                materias.IdUsuario = 1;
                materias.Activo = true;
                materias.FechaCreacion = DateTime.Now;

                _context.Add(materias);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Clave", materias.IdUsuario);
            return View(materias);
        }

        // GET: Materias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Materias == null)
            {
                return NotFound();
            }

            var materias = await _context.Materias.FindAsync(id);
            if (materias == null)
            {
                return NotFound();
            }

            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Clave", materias.IdUsuario);
            return View(materias);
        }

        // POST: Materias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdUsuario,Nombre,Description,Activo,FechaCreacion")] Materias materias)
        {
            if (id != materias.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materias);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriasExists(materias.Id))
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
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "Id", "Clave", materias.IdUsuario);
            return View(materias);
        }

        // GET: Materias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Materias == null)
            {
                return NotFound();
            }

            var materias = await _context.Materias
                .Include(m => m.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (materias == null)
            {
                return NotFound();
            }

            return View(materias);
        }

        // POST: Materias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Materias == null)
            {
                return Problem("Entity set 'GestionDeEstudiantesContext.Materias'  is null.");
            }
            var materias = await _context.Materias.FindAsync(id);
            if (materias != null)
            {
                _context.Materias.Remove(materias);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MateriasExists(int id)
        {
          return _context.Materias.Any(e => e.Id == id);
        }
    }
}
