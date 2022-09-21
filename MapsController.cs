using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{
    
    //private static readonly List<Map> _maps = new List<Map>
    //{
    //    // maps here
    //    new(15, "Germany", 17, 18 ,DateTime.Now, DateTime.UtcNow, "a map of Germany"),
    //    new(16, "Poland", 15, 15 ,DateTime.Now, DateTime.UtcNow, "a map of Poland"),
    //    new(17, "Austraila", 67, 88 ,DateTime.Now, DateTime.UtcNow),
    //    new(18, "Canada", 107, 107 ,DateTime.Now, DateTime.UtcNow),
    //    new(19, "Hungary", 13, 15 ,DateTime.Now, DateTime.UtcNow, "a map of Hungary"),
    //};

    // Endpoints here

    private static readonly List<Map> _maps2 = new List<Map>();

    
    //                              GET ALL MAPS

    [HttpGet()]
    public IEnumerable<Map> GetAllMaps()
    {
        return MapDataAccess.GetMapCommands();
    }
    // same as
    /* public IEnumerable<RobotCommand> GetAllRobotCommands() => _commands; */


    //                              GET SQUARE MAPS

/*    [HttpGet("square")]
    public IEnumerable<Map> GetSquareMapsOnly()
    {

        foreach (var val in _maps)
        {
            if (val.Columns == val.Rows)
                _maps2.Add(val);
        }

        // return a new array where we placed the matches for our condition above
        return _maps2;
    }
*/
    
    //                              GET BY ID

   [HttpGet("{id}")]
    public IEnumerable<Map> GetMapByID(int id)
    {
        return MapDataAccess.Get(id);
    }
 

    //                              ADD

    [HttpPost()]
    public Task AddNewMap(Map newMap)
    {
        return MapDataAccess.Add(newMap);

    }


    //                              UPDATE
    [HttpPut("{id}")]
    public Task UpdateMap(int id, Map updatedMap)
    {
        return MapDataAccess.Update(id, updatedMap);
    }


    //                              DELETE 

    [HttpDelete("{id}")]
    public Task DeleteMapCommand(int id)
    {
        return MapDataAccess.Delete(id);
    }

    // x, y belong?
 /*  [HttpGet("{id}/{x}-{y}")]
    public IActionResult CheckCoordinate(int id, int x, int y)
    {
        bool isOnMap = false;

        // negative coordinates
        if (Math.Sign(x) == -1 || Math.Sign(y) == -1)
            return BadRequest();

        // found?
        var findMap = _maps.FirstOrDefault(c => c.ID == id);
        // if not found
        if (findMap is null)
            return NotFound();

        // so the way I understand is that on a coordinate map we start at 0,0 being x,y
        // and then we specify how many x 'blocks' to the right, and y 'blocks' down
        // so I use LinQ to check if the provided x and y can be found in ranges starting 0,0 
        // and ending at the Columns/Rows values of the selected by ID map 

        else if(Enumerable.Range(0,findMap.Columns).Contains(x) && Enumerable.Range(0,findMap.Rows).Contains(y))
            isOnMap = true;  // point belongs to map

        return Ok(isOnMap); // returns true or false
    }
 */
}
