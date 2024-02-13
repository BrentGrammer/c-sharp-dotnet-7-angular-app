﻿using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    public AccountController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("register")] // POST: api/account
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto) // params is an object since the body to a POST is an obj
    {
        // note: the json prop names are lowercase, but thenames in the DTO are pascal case - this is convention and will be converted appropriately
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken.");

        // using keyword means to clean up memory after the class is done being in use
        // using can be applied to any class that implements the IDisposable interface
        using var hmac = new HMACSHA512(); // hashing algo from library

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)), // need to get the bytes to conform to byte array in user entity
            PasswordSalt = hmac.Key // random key that is made by the HMAC512 instance
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
    // Helper method - make async because we need to go to our database to check users
    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}