﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Wasserwacht.DigitalGuardBook.Common.Data
{
    public class Person : IdentityUser<Guid>
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string MidName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public Gender Gender { get; set; }

    }
}
