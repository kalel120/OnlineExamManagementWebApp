using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using System.Configuration;
using AutoMapper;
using OnlineExamManagementWebApp.DTOs;
using OnlineExamManagementWebApp.ViewModels;

namespace OnlineExamManagementWebApp.Controllers {
    [Authorize]
    public class ExamController : Controller {
        private readonly Uri _webApiUri;

        public ExamController() {
            _webApiUri = new Uri(ConfigurationManager.AppSettings["webApiUri"]);
        }

        public ActionResult Index() {
            ICollection<ExamIndexViewModel> examIndexVm = null;

            try {
                using (var client = new HttpClient()) {
                    client.BaseAddress = _webApiUri;

                    var response = client.GetAsync("exam");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode) {
                        var readTask = result.Content.ReadAsAsync<ICollection<ExamIndexPageDto>>();
                        readTask.Wait();
                        examIndexVm = Mapper.Map<ICollection<ExamIndexPageDto>, ICollection<ExamIndexViewModel>>(readTask.Result);
                    }
                }
            }
            catch (Exception e) {
                return View("Error");
            }

            return View(examIndexVm);
        }

        public JsonResult DeleteExam(int examId) {
            bool isDeleted = false;
            try {
                using (var client = new HttpClient()) {
                    client.BaseAddress = _webApiUri;

                    var response = client.DeleteAsync("exam/" + examId);
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode) {
                        isDeleted = true;
                    }
                }
            }
            catch (Exception e) {
                return Json(isDeleted, JsonRequestBehavior.AllowGet);
            }

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }
    }
}