using System;

namespace mvcwebapp.Dtos;

public class UserDto
{

     public int id { get; set; }
    public string Username { get; set; }=null!;

    public string Email { get; set; }=null!;

    public string  Password { get; set; }=null!;

}
