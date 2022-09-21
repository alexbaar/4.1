using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/robot-commands")]
[Produces("application/json")] // attribute that declares that the controller's actions support a response content type of application/json
public class RobotCommandsController : ControllerBase
{


    // Robot commands endpoints here :

    //                                 GET ALL
    /// <summary>
    /// Shows a list with all comands</summary>
    ///<response code = "200" >Request successful</response>

    [ProducesResponseType(StatusCodes.Status200OK)]

    [HttpGet()]
    public ActionResult<IEnumerable<RobotCommand>> GetAllRobotCommands()
    {
        return Ok(RobotCommandDataAccess.GetRobotCommands());
    }



    //                                 ADD NEW
    /// <summary>
    /// Adds a specific command.
    /// </summary>
    /// <param name="newCommand"></param>
    ///<response code = "200" > Returns the newly created robotcommand</response>
    /// <response code="400">RobotCommand's name/ID cannot be null</response>
    /// <response code="409">If a robot command with the same name/ID already exists.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]

    [HttpPost()]
    public ActionResult AddRobotCommand(RobotCommand newCommand)
    {

        if (newCommand.Name == null) return BadRequest("Name should not be null.");
        else if (RobotCommandDataAccess.DoesIdAlreadyExist(newCommand) == true) return Conflict("This ID is taken already");
        else if (RobotCommandDataAccess.DoesNameAlreadyExist(newCommand) == true) return Conflict("This name is taken already");
        else return Ok(RobotCommandDataAccess.Add(newCommand));
    }

 




    //                                  GET BY ID
    /// <summary>
    /// Gets a specific command by ID.
    /// </summary>
    /// <param name="id"></param>
    ///<response code = "200" > Returns a command, if found</response>
    ///<response code = "400" > No command found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    [HttpGet("{id}")]
    public ActionResult<IEnumerable<RobotCommand>> GetRobotCommandById(int id)
    {
        if (RobotCommandDataAccess.DoesCommandIdAlreadyExist(id) == false) return BadRequest("note: No command with this ID was found");  // cmd with that id does not exist in our DB
        else return Ok(RobotCommandDataAccess.Get(id));
    }




    //                                  UPDATE
    /// <summary>
    /// Updates a specific command by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updatedCommand"></param>
    ///<response code = "200" > Returns the newly updated robotcommand</response>
    /// <response code="409">A robot command with the same name already exists.</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]

    [HttpPut("{id}")]
    public ActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        if (RobotCommandDataAccess.DoesNameAlreadyExist(updatedCommand) != false) return Conflict(); // name needs to be unique
        else return Ok(RobotCommandDataAccess.Update(id, updatedCommand));

    }


    //                                  DELETE 
    /// <summary>
    /// Deletes a specific command by ID.
    /// </summary>
    /// <param name="id"></param>
    ///<response code = "200" > Deletes a command, if found</response>
    ///<response code = "400" > No command found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    [HttpDelete("{id}")]
    public ActionResult DeleteRobotCommand(int id)
    {

        if (RobotCommandDataAccess.DoesCommandIdAlreadyExist(id) == false) return BadRequest("There is no command with the provided ID");  // cmd with that id does not exist in our DB
        else return Ok(RobotCommandDataAccess.Delete(id));

    }



}
