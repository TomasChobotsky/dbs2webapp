using dbs2webapp.Application.DTOs;
using dbs2webapp.Application.DTOs.Tests;
using dbs2webapp.Application.DTOs.Admin;
using dbs2webapp.Application.DTOs.Auth;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<Course, CourseDto>();
            CreateMap<Course, CreateCourseDto>().ReverseMap();

            CreateMap<Chapter, ChapterDto>();
            CreateMap<Chapter, CreateChapterDto>().ReverseMap();

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
