using System.Collections.Generic;
using System.Linq;
using OnlineExamManagementWebApp.Models;
using OnlineExamManagementWebApp.Repository;

namespace OnlineExamManagementWebApp.BLL {
    public class ExamManager {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public bool SaveExams(List<Exam> examsFromView) {
            var courseId = examsFromView[0].CourseId;
            var existingExams = _unitOfWork.ExamRepository.GetActiveExamsByCourseId(courseId);

            var examsToBeSaved = new List<Exam>();
            foreach (var exam in examsFromView) {
                if (!existingExams.Any(e => e.Code == exam.Code)) {
                    examsToBeSaved.Add(exam);
                }
            }
            
            if (examsToBeSaved.Count != 0) {
                _unitOfWork.ExamRepository.SaveAll(examsToBeSaved);
            }

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

        public List<Exam> GetActiveExamsByCourseId(int courseId) {
            return _unitOfWork.ExamRepository.GetActiveExamsByCourseId(courseId);
        }

        public bool RemoveExamByCode(string examCode, int courseId) {            
            var removable = _unitOfWork.ExamRepository.GetCourseSpeceficActiveExamByCode(courseId, examCode);
            
            removable.IsDeleted = true;
            _unitOfWork.ExamRepository.Update(removable);
            return _unitOfWork.Complete();
        }

        public bool UpdateExam(Exam exam) {
            var updatable = _unitOfWork.ExamRepository.GetCourseSpeceficActiveExamByCode(exam.CourseId, exam.Code);
            if (updatable == null) {
                return false;
            }

            updatable.Code = exam.Code;
            updatable.Type = exam.Type;
            updatable.Topic = exam.Topic;
            updatable.FullMarks = exam.FullMarks;
            updatable.Duration = exam.Duration;
            updatable.SerialNo = exam.SerialNo;

             _unitOfWork.ExamRepository.Update(updatable);
            return _unitOfWork.Complete();
        }
    }
}
