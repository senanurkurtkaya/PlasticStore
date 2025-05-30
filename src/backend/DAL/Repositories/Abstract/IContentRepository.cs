﻿using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Abstract
{
    public interface IContentRepository
    {
        Task<List<Content>> GetAllContentsAsync();       
    }
}
