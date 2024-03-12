using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class BuggyController : BaseApiController
{
    private readonly StoreContext _context;

    public BuggyController(StoreContext context)
    {
        _context = context;
    }

    [HttpGet("not-found")]
    public ActionResult GetNotFoundRequest()
    {
        var thing = _context.Products.Find(42);
        if (thing == null)
        {
            return NotFound(new ApiResponse(404));
        }
        return Ok();
    }

    [HttpGet("server-error")]
    public ActionResult GetServerError()
    {
        var thing = _context.Products.Find(42);
        var thingToReturn = thing.ToString();
        return Ok(thingToReturn);
    }

    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }


    [HttpGet("bad-request/{id}")]
    public ActionResult GetBadRequest(int id)
    {
        return BadRequest();
    }
}
