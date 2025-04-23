using dbs2webapp.Application.DTOs;
using dbs2webapp.Application.DTOs.Tests;
using dbs2webapp.Application.DTOs.Admin;
using dbs2webapp.Application.DTOs.Auth;
using AutoMapper;
using dbs2webapp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using dbs2webapp.Application.DTOs.Tests.Result;

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
            CreateMap<Question, QuestionResultDto>();

            CreateMap<Option, OptionDto>().ReverseMap();
            CreateMap<Option, OptionResultDto>()
                .ForMember(d => d.WasChosen, c => c.Ignore())
                .AfterMap((src, dest, ctx) =>
                 {
                     var chosen = (HashSet<int>)ctx.Items["ChosenOptionIds"];
                     dest.WasChosen = chosen.Contains(src.Id);
                 });

            CreateMap<TestResult, TestResultDto>().ReverseMap();
            CreateMap<TestResult, TestResultDetailsDto>()
                .ForMember(d => d.TestTitle,
                           c => c.MapFrom(s => s.Test.Title))
                .ForMember(d => d.Questions,        
                           c => c.MapFrom(s => s.Test.Questions));

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
