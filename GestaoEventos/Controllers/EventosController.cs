using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestaoEventos.Data;
using GestaoEventos.Models;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization;

namespace GestaoEventos.Controllers;

//[Authorize(Roles = "Admin")] // Protege todas as ações de gerenciamento
public class EventosController : Controller
{
    private readonly ApplicationDbContext _context;

    public EventosController(ApplicationDbContext context) => _context = context;

    // GET: Eventos
    //No controller, definir as rotas e suas permissões
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        // ORM com Join duplo: Traz dados da Categoria E do Local
        var eventos = await _context.Eventos
            .Include(e => e.Categoria)
            .Include(e => e.Local)
            .ToListAsync();

        return View(eventos);
    }

    // GET: Eventos/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        // Carrega as duas listas para os DropDowns na View
        ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nome");
        ViewBag.LocalId = new SelectList(_context.Locais, "Id", "Nome");
        return View();
    }

    // POST: Eventos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Eventos evento)
    {
        if (ModelState.IsValid)
        {
            _context.Add(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Repopula as listas em caso de erro de validação
        ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nome", evento.CategoriaId);
        ViewBag.LocalId = new SelectList(_context.Locais, "Id", "Nome", evento.LocalId);
        return View(evento);
    }

    // GET: Eventos/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        // Carrega as listas selecionando os valores atuais do evento
        ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nome", evento.CategoriaId);
        ViewBag.LocalId = new SelectList(_context.Locais, "Id", "Nome", evento.LocalId);

        return View(evento);
    }

    // POST: Eventos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    //Colocar a role que tu permite ver essa página
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, Eventos evento)
    {
        if (id != evento.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(evento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.CategoriaId = new SelectList(_context.Categorias, "Id", "Nome", evento.CategoriaId);
        ViewBag.LocalId = new SelectList(_context.Locais, "Id", "Nome", evento.LocalId);
        return View(evento);
    }

    // GET: Eventos/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var evento = await _context.Eventos
            .Include(e => e.Categoria)
            .Include(e => e.Local)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (evento == null) return NotFound();

        return View(evento);
    }

    // POST: Eventos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento != null)
        {
            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}