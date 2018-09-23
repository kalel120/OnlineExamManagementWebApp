using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class ExamManager {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public bool SaveExams(List<Exam> examsFromView) {
            var courseId = examsFromView[0].CourseId;
            var existingExams = _unitOfWork.ExamRepository.GetActiveExamsByCourseId(courseId);

            var examsToBeSaved = new List<Exam>();
            foreach (var exam in examsFromView) {
                if (!existingExams.Any(e => e.Code == exam.Code)) {
                    examsToBeSaved.Add(exam);
                }
            }

            bool isSavedAll = _unitOfWork.ExamRepository.SaveAll(examsToBeSaved);

            var newExistingExams = _unitOfWork.ExamRepository.GetActiveExamsByCourseId(courseId);


            bool isNeedResequancing = false;
            for (var index = 0; index < newExistingExams.Count; index++) {
                if (!(newExistingExams[index].Code == examsFromView[index].Code
                      && newExistingExams[index].SerialNo == examsFromView[index].SerialNo)) {
                    isNeedResequancing = true;
                    break;
                }
            }

            if (isNeedResequancing) {
                foreach (var exam in newExistingExams) {
                    var index = examsFromView.FindIndex(e => e.Code == exam.Code);
                    exam.SerialNo = examsFromView[index].SerialNo;
                }
                return _unitOfWork.ExamRepository.UpdateSerialNo();
            }

            return isSavedAll;
        }

        public List<Exam> GetActiveExamsByCourseId(int courseId) {
            return _unitOfWork.ExamRepository.GetActiveExamsByCourseId(courseId);
        }
    }
}
