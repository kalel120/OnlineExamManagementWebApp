using System;
using System.Collections.Generic;

namespace OnlineExamManagementWebApp.Models {
    public class Option {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        //Navigation
        public ICollection<QuestionOption> QuestionOptions { get; set; }
    }
}
