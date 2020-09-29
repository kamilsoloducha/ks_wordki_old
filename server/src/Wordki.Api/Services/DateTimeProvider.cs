﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wordki.Api.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now();
    }

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now() => new DateTime();
    }
}