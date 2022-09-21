
using System.ComponentModel;  // for line 20
using System.ComponentModel.DataAnnotations; // for line 15 and so on

namespace robot_controller_api;


public class RobotCommand
{

    public int ID { get; set; }
    //public Guid Id { get; set; } // give a random one for now

    [Required]
    public string Name { get; set; }
    public string? Description { get; set; } // ? means it can be null

    [DefaultValue(true)]
    public bool IsMoveCommand { get; set; }

    // those will be set by datetime now
    public DateTime CreatedDate { get; set; } // D&T of command creation
    public DateTime ModifiedDate { get; set; } // D&T of command modifications

    // constructor
    

    /*
    public RobotCommand( int id,string name, bool isMoveCommand, DateTime createdDate, DateTime modifiedDate , string? description = null)
    {
        ID = id;
        Name = name;
        IsMoveCommand = isMoveCommand;
        CreatedDate = createdDate;
        ModifiedDate = modifiedDate;
        Description = description;
    }
    */
}
