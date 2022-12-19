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
    public class PaseDeListasController : Controller
    {
        private readonly GestionDeEstudiantesContext _context;

        public PaseDeListasController(GestionDeEstudiantesContext context)
        {
            _context = context;
        }

        // GET: PaseDeListas
        public async Task<IActionResult> Index()
        {
            var gestionDeEstudiantesContext = _context.PaseDeLista.Include(p => p.IdEstudianteNavigation).Include(x=> x.IdOpcionNavigation);
            return View(await gestionDeEstudiantesContext.ToListAsync());
        }

        // GET: PaseDeListas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PaseDeLista == null)
            {
                return NotFound();
            }

            var paseDeLista = await _context.PaseDeLista
                .Include(p => p.IdEstudianteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paseDeLista == null)
            {
                return NotFound();
            }

            return View(paseDeLista);
        }

        // GET: PaseDeListas/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Estudiantes"] = await _context.Estudiantes.Where(x => x.Activo).ToListAsync();
            ViewData["IdOpcion"] = new SelectList(_context.PaseDeListaOpciones.Where(x=> x.Activo), "Id", "Nombre");

            return View();
        }

        // POST: PaseDeListas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdEstudiante,Fecha,Presente,FechaCreacion")] PaseDeLista paseDeLista)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paseDeLista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Apellido1", paseDeLista.IdEstudiante);
            return View(paseDeLista);
        }

        // GET: PaseDeListas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PaseDeLista == null)
            {
                return NotFound();
            }

            var paseDeLista = await _context.PaseDeLista.FindAsync(id);
            if (paseDeLista == null)
            {
                return NotFound();
            }
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Apellido1", paseDeLista.IdEstudiante);
            return View(paseDeLista);
        }

        // POST: PaseDeListas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEstudiante,Fecha,Presente,FechaCreacion")] PaseDeLista paseDeLista)
        {
            if (id != paseDeLista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paseDeLista);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaseDeListaExists(paseDeLista.Id))
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
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Apellido1", paseDeLista.IdEstudiante);
            return View(paseDeLista);
        }

        // GET: PaseDeListas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PaseDeLista == null)
            {
                return NotFound();
            }

            var paseDeLista = await _context.PaseDeLista
                .Include(p => p.IdEstudianteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paseDeLista == null)
            {
                return NotFound();
            }

            return View(paseDeLista);
        }

        // POST: PaseDeListas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PaseDeLista == null)
            {
                return Problem("Entity set 'GestionDeEstudiantesContext.PaseDeLista'  is null.");
            }
            var paseDeLista = await _context.PaseDeLista.FindAsync(id);
            if (paseDeLista != null)
            {
                _context.PaseDeLista.Remove(paseDeLista);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaseDeListaExists(int id)
        {
          return _context.PaseDeLista.Any(e => e.Id == id);
        }

        [HttpPost]
        public JsonResult PasarLista(int IdEstudiante, int IdOption, DateTime? Fecha)
        {
            DateTime fecha = Fecha ?? DateTime.Now;

            var p = new PaseDeLista { IdEstudiante = IdEstudiante, IdOpcion = IdOption, Fecha = fecha, FechaCreacion = DateTime.Now };
            _context.PaseDeLista.Add(p);
            var result = _context.SaveChanges() > 0;

            return Json(result);
        }
    }
}
