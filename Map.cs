using System.ComponentModel.DataAnnotations;

namespace robot_controller_api;

public class Map
{


    public int ID { get; set; }
    //public Guid Id { get; set; } // give a random one for now

    [Required]
    public string Name { get; set; }
    [Required]
    public int Columns { get; set; }
    [Required]
    public int Rows { get; set; }
    public string? Description { get; set; } // ? means it can be null

    // those will be set by datetime now
    public DateTime CreatedDate { get; set; } 
    public DateTime ModifiedDate { get; set; } 

    // constructor

    //public Map(int id, string name,  int columns , int rows,  DateTime createdDate,
    //                    DateTime modifiedDate, string? description = null)
    //{
    //    ID = id;
    //    Name = name;
    //    Description = description;
    //    Columns = columns;
    //    Rows = rows;
    //    CreatedDate = createdDate;
    //    ModifiedDate = modifiedDate;
    //}
}
