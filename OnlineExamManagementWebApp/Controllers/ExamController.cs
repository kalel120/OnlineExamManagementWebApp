using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using AutoMapper;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.ViewModels;

namespace OnlineExamManagementWebApp.Controllers {
    public class ExamController : Controller {

        public ActionResult Index() {
            ICollection<ExamIndexViewModel> examIndexVm;

            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri("http://localhost:52119/api/");

                var response = client.GetAsync("exam");
                response.Wait();

                var result = response.Result;
                if (result.IsSuccessStatusCode) {
                    var readTask = result.Content.ReadAsAsync<ICollection<ExamIndexPageDto>>();
                    readTask.Wait();
                    examIndexVm = Mapper.Map<ICollection<ExamIndexPageDto>, ICollection<ExamIndexViewModel>>(readTask.Result);
                }
                else {
                    return View("Error");
                }
            }

            return View(examIndexVm);
        }
    }
}