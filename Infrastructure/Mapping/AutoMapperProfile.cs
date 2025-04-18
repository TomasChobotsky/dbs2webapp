using Application.DTOs;
using Application.DTOs.Tests;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Course, CourseDto>();
            CreateMap<Course, CreateCourseDto>().ReverseMap();

            // TEST DTOs
            CreateMap<Test, TestDto>();
            CreateMap<Test, TestResultDto>();
            CreateMap<Test, CreateTestDto>().ReverseMap();

            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<Option, OptionDto>().ReverseMap();

            CreateMap<TestResult, TestResultDto>().ReverseMap();

            // TEST submission model mapping
            CreateMap<TestResult, TestResultDto>()
                .ForMember(dest => dest.TestTitle,
                    opt => opt.MapFrom(src => src.Test!.Title ?? "(Unnamed Test)"));

            CreateMap<AnswerSubmissionDto, Option>();
            CreateMap<TestSubmissionDto, TestResult>();

            // USER + IDENTITY
            CreateMap<AdminCreateUserDto, IdentityUser>();
            CreateMap<RegisterDto, IdentityUser>();
            CreateMap<LoginDto, IdentityUser>();
        }
    }
}
