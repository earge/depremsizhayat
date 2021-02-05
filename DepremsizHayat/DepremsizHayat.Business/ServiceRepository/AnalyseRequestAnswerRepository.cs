﻿using DepremsizHayat.Business.BaseRepository;
using DepremsizHayat.Business.Factory;
using DepremsizHayat.Business.IServiceRepository;
using DepremsizHayat.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Business.ServiceRepository
{
    public class AnalyseRequestAnswerRepository : Repository<ANALYSE_REQUEST_ANSWER>, IAnalyseRequestAnswerRepository
    {
        public AnalyseRequestAnswerRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}