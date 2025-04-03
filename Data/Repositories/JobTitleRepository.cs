﻿using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;
public class JobTitleRepository(DataContext context) : BaseRepository<MemberEntity>(context), IJobTitleRepository
{
}