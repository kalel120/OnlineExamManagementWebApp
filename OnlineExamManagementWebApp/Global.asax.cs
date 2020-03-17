using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Models.Identity;
using OnlineExamManagementWebApp.ViewModels;
using OnlineExamManagementWebApp.ViewModels.Account;

namespace OnlineExamManagementWebApp {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Mapper.Initialize(cfg => {
                cfg.CreateMap<Course, CourseEditViewModel>();
                cfg.CreateMap<CourseEditViewModel, Course>();
                cfg.CreateMap<CourseEntryViewModel, Course>();

                cfg.CreateMap<CourseTrainer, CourseTrainerDto>();
                cfg.CreateMap<CourseTrainerDto, CourseTrainer>();

                cfg.CreateMap<Exam, ExamDto>();
                cfg.CreateMap<ExamDto, Exam>();

                cfg.CreateMap<SearchCourseViewModel, SearchCourseDto>();

                cfg.CreateMap<CourseWithOrgNameDto, CourseListViewModel>();

                cfg.CreateMap<OrgEntryViewModel, Organization>();

                cfg.CreateMap<RegisterViewModel, AppUser>();

                cfg.CreateMap<AppUser, UserProfileVm>();
                cfg.CreateMap<UserProfileVm, AppUser>();

                cfg.CreateMap<ExamDetailsDto, ExamIndexViewModel>();
            });
        }
    }
}
