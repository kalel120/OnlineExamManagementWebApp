using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class ExamManager {
        private readonly UnitOfWork _unitOfWork;

        public ExamManager() {
            _unitOfWork = new UnitOfWork();
        }

        public bool SaveExams(List<Exam> examsFromView) {
            var courseId = examsFromView[0].CourseId;
            var existingExams = _unitOfWork.Exams.GetActiveExamsByCourseId(courseId);

            var examsToBeSaved = new List<Exam>();
            foreach (var exam in examsFromView) {
                if (!existingExams.Any(e => e.Code == exam.Code)) {
                    examsToBeSaved.Add(exam);
                }
            }
 
            if (examsToBeSaved.Count == 0) {
                return false;
            }

            _unitOfWork.Exams.SaveAll(examsToBeSaved);

            if (IsNeedResequancing(examsFromView, existingExams)) {
                foreach (var exam in existingExams) {
                    var index = examsFromView.FindIndex(e => e.Code == exam.Code);
                    exam.SerialNo = examsFromView[index].SerialNo;
                }
            }
            return _unitOfWork.Complete();
        }

        private bool IsNeedResequancing(List<Exam> examsFromView, List<Exam> existingExams) {
            bool isNeedResequancing = false;
            for (var index = 0; index < existingExams.Count; index++) {
                if (!(existingExams[index].Code == examsFromView[index].Code
                      && existingExams[index].SerialNo == examsFromView[index].SerialNo)) {
                    isNeedResequancing = true;
                    break;
                }
            }

            return isNeedResequancing;
        }

        public bool RemoveExamByCode(string examCode, int courseId) {
            var removable = _unitOfWork.Exams.GetActiveExamByCode(courseId, examCode);
            if (removable == null) {
                return false;
            }

            removable.IsDeleted = true;
            _unitOfWork.Exams.Update(removable);
            return _unitOfWork.Complete();
        }

        public bool UpdateExamByCode(string existingCode, Exam exam) {
            var updatable = _unitOfWork.Exams.GetActiveExamByCode(exam.CourseId, existingCode);

            updatable.Code = exam.Code;
            updatable.Type = exam.Type;
            updatable.Topic = exam.Topic;
            updatable.FullMarks = exam.FullMarks;
            updatable.Duration = exam.Duration;

            _unitOfWork.Exams.Update(updatable);
            return _unitOfWork.Complete();
        }

        public bool IsExamExists(int courseId, string examCode) {
            var result = _unitOfWork.Exams.GetActiveExamByCode(courseId, examCode);
            return result == null ? false : true;
        }

        public bool ReSequanceSerial(List<Exam> examsFromView) {
            var courseId = examsFromView[0].CourseId;
            var existingExams = _unitOfWork.Exams.GetActiveExamsByCourseId(courseId);

            foreach (var exam in existingExams) {
                var index = examsFromView.FindIndex(e => e.Code == exam.Code);
                exam.SerialNo = examsFromView[index].SerialNo;
            }
            return _unitOfWork.Complete();
        }
    }
}
