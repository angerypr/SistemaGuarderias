using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaGuarderias.Application.DTOs;
using SistemaGuarderias.Web.Models;
using System.Text;

namespace SistemaGuarderias.Web.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly HttpClient _httpClient;

        public EmpleadosController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7271/api");
        }

        // Listar 
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Empleados");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var empleados = JsonConvert.DeserializeObject<IEnumerable<EmpleadosViewModel>>(content);
                return View("Index", empleados);
            }

            return View(new List<EmpleadosViewModel>());
        }

        // Crear 
        public async Task<IActionResult> Create()
        {
            var responseGuarderias = await _httpClient.GetAsync("/api/Guarderias");

            if (responseGuarderias.IsSuccessStatusCode)
            {
                var content = await responseGuarderias.Content.ReadAsStringAsync();
                ViewBag.GuarderiaList = JsonConvert.DeserializeObject<IEnumerable<GuarderiasViewModel>>(content);
            }
            else
            {
                ViewBag.GuarderiaList = new List<GuarderiasViewModel>(); 
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmpleadoDTO empleado)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(empleado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Empleados", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al crear empelado");
                }
            }

            var responseGuarderias = await _httpClient.GetAsync("/api/Guarderias");
            if (responseGuarderias.IsSuccessStatusCode)
            {
                var content = await responseGuarderias.Content.ReadAsStringAsync();
                ViewBag.GuarderiaList = JsonConvert.DeserializeObject<IEnumerable<GuarderiasViewModel>>(content);
            }
            else
            {
                ViewBag.GuarderiaList = new List<GuarderiasViewModel>();
            }

            return View(empleado);
        }

        // Editar 
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Empleados/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var empleado = JsonConvert.DeserializeObject<EmpleadosViewModel>(content);

                var responseGuarderias = await _httpClient.GetAsync("/api/Guarderias");
                if (responseGuarderias.IsSuccessStatusCode)
                {
                    var contentGuarderias = await responseGuarderias.Content.ReadAsStringAsync();
                    ViewBag.GuarderiaList = JsonConvert.DeserializeObject<IEnumerable<GuarderiasViewModel>>(contentGuarderias);
                }
                else
                {
                    ViewBag.GuarderiaList = new List<GuarderiasViewModel>();
                }

                return View(empleado);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EmpleadoDTO empleado)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(empleado);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"/api/Empleados/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar empleado.");
                }
            }

            var responseGuarderias = await _httpClient.GetAsync("/api/Guarderias");
            if (responseGuarderias.IsSuccessStatusCode)
            {
                var content = await responseGuarderias.Content.ReadAsStringAsync();
                ViewBag.GuarderiaList = JsonConvert.DeserializeObject<IEnumerable<GuarderiasViewModel>>(content);
            }
            else
            {
                ViewBag.GuarderiaList = new List<GuarderiasViewModel>();
            }

            return View(empleado);
        }

        // Mostrar detalles de un empleado
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Empleados/{id}");

            if(!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();
            var empleado = JsonConvert.DeserializeObject<EmpleadosViewModel>(content);

            if (empleado == null)
            {
                return NotFound();
            }

            
            if(empleado.GuarderiaId > 0)  
    {
                var responseGuarderia = await _httpClient.GetAsync($"/api/Guarderias/{empleado.GuarderiaId}");

                if (responseGuarderia.IsSuccessStatusCode)
                {
                    var guarderiaContent = await responseGuarderia.Content.ReadAsStringAsync();
                    var guarderia = JsonConvert.DeserializeObject<GuarderiasViewModel>(guarderiaContent);

                    if (guarderia != null)
                    {
                        empleado.Guarderia = guarderia;
                    }
                }
            }

            return View(empleado);
        }

        // Eliminar 
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Empleados/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar el empleado.";
                return RedirectToAction("Index");
            }
        }
    }
}
