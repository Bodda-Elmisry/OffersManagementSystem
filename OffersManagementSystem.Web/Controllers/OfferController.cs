using Mapster;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            this._offerService = offerService;
        }

        /// <summary>
        /// Getting offers list with filter and navigate to Offers view
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IActionResult> Offers(OffersFilterDTO filter)
        {
            var offers = await _offerService.GetAllOffersAsync(
                                            filter.Serial, 
                                            filter.OfferAddress,
                                            filter.FromDate, 
                                            filter.ToDate, 
                                            filter.Active,
                                            filter.TotalFrom, 
                                            filter.TotalTo);
            var result = offers.Adapt<List<OfferResultDTO>>();
            ViewData["Filter"] = filter;
            return View(result);
        }

        /// <summary>
        /// Clear the filter and redirect to Offers action with default values
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearFilter()
        {
            
            return RedirectToAction("Offers", "Offer", new OffersFilterDTO());
        }

        /// <summary>
        /// Getting offer detail by id and navigate to OfferDetail view to show offer details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> ViewOfferDetail(long id)
        {
            var offer = await _offerService.GetOfferByIdAsync(id);
            var result = offer.Adapt<OfferResultDTO>();

            return View(result);
        }

        /// <summary>
        /// Navigate to Create Offer view with default values
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var offer = new CreateOfferDTO();
            offer.OfferDate = DateTime.Now;
            offer.OfferDayes = 30;
            return View(offer);
        }

        /// <summary>
        /// Create a new offer with uploaded file and redirect to Offers view
        /// </summary>
        /// <param name="model"></param>
        /// <param name="UploadedFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateOfferDTO model, IFormFile UploadedFile)
        {
            // Check if file is missing or empty
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "File is required");
            }

            if (!ModelState.IsValid)
                return View(model);

            var offer = model.Adapt<Offer>();

            offer.FilePath = await UploadFile(UploadedFile);

            var created = await _offerService.CreateOfferAsync(offer);
            
            return RedirectToAction("Offers");
        }

        /// <summary>
        /// Upload file to the server and return the file path
        /// </summary>
        /// <param name="file"></param>
        /// <returns>
        /// File path as string
        /// </returns>
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

        /// <summary>
        /// Download file from the server based on the file path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>
        /// File result with the file content and content type
        /// </returns>
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
