using CloudCustomers.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCustomers.UnitTests.Fixtures
{
    public static class UsersFixture
    {
        public static List<User> GetTestUsers()
        {

            return new()
            {
                new User { Name = "Jeff", Address = new Address()
                {
                    Street = "123 Road",
                    City = "Leeds",
                    PostCode = "12345F"
                },
                Email = "Jeff@email.com"
            },
                new User { Name = "Tom", Address = new Address()
                {
                    Street = "ABC Street",
                    City = "Burnley",
                    PostCode = "AAAAAA"
                },
                Email = "Tom@email.com"
            },
                new User { Name = "Steve", Address = new Address()
                {
                    Street = "FFF Lane",
                    City = "Oxford",
                    PostCode = "HFHFHFH"
                },
                Email = "Steve@email.com"
            }
            };


        }
    }
}
