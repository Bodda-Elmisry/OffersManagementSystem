using Mapster;
using Microsoft.AspNetCore.Mvc;
using OffersManagementSystem.Application.IServices;
using OffersManagementSystem.Domain.Entities;
using OffersManagementSystem.Web.DTOs;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OffersManagementSystem.Web.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            this._offerService = offerService;
        }

        public async Task<IActionResult> Offers()
        {
            var offers = await _offerService.GetAllOffersAsync(null, null, null, null, true, null, null);
            var result = offers.Adapt<List<OfferResultDTO>>();

            return View(result);
        }

        public async Task<IActionResult> ViewOfferDetail(long id)
        {
            var offer = await _offerService.GetOfferByIdAsync(id);
            var result = offer.Adapt<OfferResultDTO>();

            return View(result);
        }

        public IActionResult Create()
        {
            var offer = new CreateOfferDTO();
            offer.OfferDate = DateTime.Now;
            offer.OfferDayes = 30;
            return View(offer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOfferDTO model, IFormFile UploadedFile)
        {
            if (!ModelState.IsValid)
                return View(model);

            if(UploadedFile == null || UploadedFile.Length == 0)
            {
                return View(model);
            }

            var offer = model.Adapt<Offer>();

            offer.FilePath = await UploadFile(UploadedFile);

            var created = await _offerService.CreateOfferAsync(offer);
            
            return RedirectToAction("Offers");
        }

        private async Task<string> UploadFile(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public IActionResult DownloadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return NotFound("File path is missing.");


            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", filePath);

            if (!System.IO.File.Exists(fullPath))
                return NotFound("File not found.");

            var contentType = "application/octet-stream";
            var fileName = Path.GetFileName(fullPath);

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, contentType, fileName);
        }
    }
}
