using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.ViewModels;

namespace OnlineExamManagementWebApp {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
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
            });
        }
    }
}
