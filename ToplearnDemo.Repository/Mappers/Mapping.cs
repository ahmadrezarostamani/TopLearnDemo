﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ToplearnDemo.DomainClassess.User;
using ToplearnDemo.ViewModels.Account;

namespace ToplearnDemo.Repository.Mappers
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<User, RegisterViewModel>().ReverseMap();
        }
    }
}
