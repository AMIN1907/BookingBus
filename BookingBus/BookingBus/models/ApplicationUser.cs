
/*
 {
  A@dmin123   7655ea93-2fc0-4136-a056-6419f4a1aa02   "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJBQGRtaW4xMjMiLCJqdGkiOiI1YzhkODY0MS0wNTFjLTQ2ZjUtYThkZi1iYjIyMTVmNmU2YjMiLCJlbWFpbCI6IkFAZG1pbjEyMyIsInVpZCI6Ijc2NTVlYTkzLTJmYzAtNDEzNi1hMDU2LTY0MTlmNGExYWEwMiIsInJvbGVzIjpbIlVzZXIiLCJBZG1pbiJdLCJleHAiOjE3MDkzODAyODYsImlzcyI6IlNlY3VyZUFwaSIsImF1ZCI6IlNlY3VyZUFwaVVzZXIifQ.2UFDPxm5SiSNypcaS8pC2CPU6AUdVLzkiLT96m-T5lY"
  "email": "aminAMIN12@",  5d563b9d-a537-4ada-aa63-656083e3ba95
  "password": "aminAMIN12@", 
  }
 
 */

using Microsoft.AspNetCore.Identity;

namespace BookingBus.models
{
    public class ApplicationUser : IdentityUser

    {
        public string fname { get; set; }
        public string lname { get; set; }
        public DateTime Birthdate { get; set; }
        public string? ginder { get; set; }




    }
}
