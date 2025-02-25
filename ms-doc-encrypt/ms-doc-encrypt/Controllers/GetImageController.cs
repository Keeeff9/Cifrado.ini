using Microsoft.AspNetCore.Mvc;
using Path = System.IO.Path;

namespace ms_doc_encrypt.Controllers
{
    [Route("api/doc-encrypt/[controller]")]
    [ApiController]
    public class GetImageController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFile(string id) 
        {
            string path = Path.Combine(AppContext.BaseDirectory, "downloads", id);
            if (!System.IO.File.Exists(path)) 
            {
                return NotFound();
            }

            byte[] data = await System.IO.File.ReadAllBytesAsync(path);
            return File(data, "application/octet-stream", id);
        }
    }
}
