﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyCarBook.Application.Features.Mediator.Results.AboutResults
{
    public class GetAboutQueryResult
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }


        public string ImageUrl { get; set; }
    }
}
