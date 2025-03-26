using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGuarderias.Application.DTOs;
using SistemaGuarderias.Web.Models;
using System.Text;

namespace SistemaGuarderias.Web.Controllers
{
    public class GuarderiasController : Controller
    {
        private readonly HttpClient _httpClient;

        public GuarderiasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7271/api");
        }

        // Listar
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Guarderias");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var guarderias = JsonConvert.DeserializeObject<IEnumerable<GuarderiasViewModel>>(content);
                return View("Index", guarderias);
            }

            return View(new List<GuarderiasViewModel>());
        }

        // Crear
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GuarderiaDTO guarderias)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(guarderias);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Guarderias", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al añadir guardería");
                }
            }
            return View(guarderias);
        }

        // Edit
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Guarderias/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var guarderia = JsonConvert.DeserializeObject<GuarderiasViewModel>(content);

                return View(guarderia);
            }
            else
            {
                return RedirectToAction("Details");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, GuarderiaDTO guarderia)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(guarderia);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var repsonse = await _httpClient.PutAsync($"/api/Guarderias/{id}", content);

                if (repsonse.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", new { id });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar guardería.");
                }
            }

            return View(guarderia);
        }

        //Mostrar especificamente
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Guarderias/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var guarderias = JsonConvert.DeserializeObject<GuarderiasViewModel>(content);

                return View(guarderias);
            }
            else
            {
                return RedirectToAction("Details");
            }
        }

        //Delete
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Guarderias/{id}");

            if (response.IsSuccessStatusCode)
            { 
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar la guardería.";
                return RedirectToAction("Index");
            }
        }


    }
}