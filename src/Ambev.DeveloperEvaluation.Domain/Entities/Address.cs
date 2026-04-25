using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Address : BaseEntity
{
    protected Address() { }

    public Address(int userId, string city, string street, int number, string zipCode, string geolocation_lat, string geolocation_long)
    {
        UserId = userId;
        City = city;
        Street = street;
        Number = number;
        ZipCode = zipCode;
        Geolocation_lat = geolocation_lat;
        Geolocation_long = geolocation_long;
    }

    public int UserId { get; private set; }
    public string City { get; private set; }
    public string Street { get; private set; }
    public int Number { get; private set; }
    public string ZipCode { get; private set; }
    public string Geolocation_lat { get; private set; }
    public string Geolocation_long { get; private set; }
    public virtual User? User { get; set; }

}

