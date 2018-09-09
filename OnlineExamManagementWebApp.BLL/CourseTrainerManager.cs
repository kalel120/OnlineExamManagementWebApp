using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class CourseTrainerManager {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public List<CourseTrainer> GetCourseTrainersByCourseId(int id) {
            return _unitOfWork.CourseTrainerRepository.GetCourseTrainersByCourseId(id);
        }
    }
}
