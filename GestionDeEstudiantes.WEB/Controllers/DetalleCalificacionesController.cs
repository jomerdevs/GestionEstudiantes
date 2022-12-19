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
    public class DetalleCalificacionesController : Controller
    {
        private readonly GestionDeEstudiantesContext _context;

        public DetalleCalificacionesController(GestionDeEstudiantesContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var gestionDeEstudiantesContext = _context.DetalleCalificaciones.Include(d => d.IdCalificacionNavigation).Include(d => d.IdEstudianteNavigation);
            return View(await gestionDeEstudiantesContext.ToListAsync());
        }

        // GET: DetalleCalificaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DetalleCalificaciones == null)
            {
                return NotFound();
            }

            var detalleCalificaciones = await _context.DetalleCalificaciones
                .Include(d => d.IdCalificacionNavigation)
                .Include(d => d.IdEstudianteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleCalificaciones == null)
            {
                return NotFound();
            }

            return View(detalleCalificaciones);
        }

        // GET: DetalleCalificaciones/Create
        public IActionResult Create()
        {
            ViewData["IdCalificacion"] = new SelectList(_context.Calificaciones, "Id", "Codigo");
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Nombre");
            return View();
        }

        // POST: DetalleCalificaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCalificacion,IdEstudiante,Calificacion,CalificacionLiteral,FechaCreacion")] DetalleCalificaciones detalleCalificaciones)
        {
            if (ModelState.IsValid)
            {
                if (detalleCalificaciones.Calificacion >= 90 && detalleCalificaciones.Calificacion <= 100)
                {
                    detalleCalificaciones.CalificacionLiteral = "A";
                }
                else if (detalleCalificaciones.Calificacion >= 80 && detalleCalificaciones.Calificacion < 90)
                {
                    detalleCalificaciones.CalificacionLiteral = "B";
                }
                else if (detalleCalificaciones.Calificacion >= 70 && detalleCalificaciones.Calificacion < 80)
                {
                    detalleCalificaciones.CalificacionLiteral = "C";
                }
                else
                {
                    detalleCalificaciones.CalificacionLiteral = "F";
                }
                _context.Add(detalleCalificaciones);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }             
            
            ViewData["IdCalificacion"] = new SelectList(_context.Calificaciones, "Id", "Codigo", detalleCalificaciones.IdCalificacion);
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Apellido1", detalleCalificaciones.IdEstudiante);
            return View(detalleCalificaciones);
        }

        // GET: DetalleCalificaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DetalleCalificaciones == null)
            {
                return NotFound();
            }

            var detalleCalificaciones = await _context.DetalleCalificaciones.FindAsync(id);
            if (detalleCalificaciones == null)
            {
                return NotFound();
            }
            ViewData["IdCalificacion"] = new SelectList(_context.Calificaciones, "Id", "Codigo", detalleCalificaciones.IdCalificacion);
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Nombre", detalleCalificaciones.IdEstudiante);
            return View(detalleCalificaciones);
        }

        // POST: DetalleCalificaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCalificacion,IdEstudiante,Calificacion,CalificacionLiteral,FechaCreacion")] DetalleCalificaciones detalleCalificaciones)
        {
            if (id != detalleCalificaciones.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleCalificaciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleCalificacionesExists(detalleCalificaciones.Id))
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
            ViewData["IdCalificacion"] = new SelectList(_context.Calificaciones, "Id", "Codigo", detalleCalificaciones.IdCalificacion);
            ViewData["IdEstudiante"] = new SelectList(_context.Estudiantes, "Id", "Apellido1", detalleCalificaciones.IdEstudiante);
            return View(detalleCalificaciones);
        }

        // GET: DetalleCalificaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DetalleCalificaciones == null)
            {
                return NotFound();
            }

            var detalleCalificaciones = await _context.DetalleCalificaciones
                .Include(d => d.IdCalificacionNavigation)
                .Include(d => d.IdEstudianteNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleCalificaciones == null)
            {
                return NotFound();
            }

            return View(detalleCalificaciones);
        }

        // POST: DetalleCalificaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DetalleCalificaciones == null)
            {
                return Problem("Entity set 'GestionDeEstudiantesContext.DetalleCalificaciones'  is null.");
            }
            var detalleCalificaciones = await _context.DetalleCalificaciones.FindAsync(id);
            if (detalleCalificaciones != null)
            {
                _context.DetalleCalificaciones.Remove(detalleCalificaciones);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleCalificacionesExists(int id)
        {
          return _context.DetalleCalificaciones.Any(e => e.Id == id);
        }
    }
}
