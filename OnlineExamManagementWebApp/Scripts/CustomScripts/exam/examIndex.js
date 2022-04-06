// IIFE to wrap all jQuery code

(($) => {
   
    $(() => {
        // start
        let loadingSpinner = $('#loading');
        loadingSpinner.hide();
        $("#dataTable-examList").DataTable({
            //"processing": true,
            //"language": {
            //    "loadingRecords": "&nbsp;",
            //    "processing": '<div id="loading"></div>'
            //}
        });

        const deleteExamById = (examId) => {
            return new Promise((resolve, reject) => {
                $.post("/Exam/DeleteExam", { examId: examId })
                    .done((result) => {
                        resolve(result);
                    })
                    .fail((jqXhr, textStatus) => {
                        reject(textStatus);
                    });
            });
        }

        $(document).on("click", ".js-deleteExam", (event) => {
            const examId = $(event.target).data("exam-id");

            bootbox.confirm({
                title: 'Delete Exam',
                size: 'lg',
                message: "This exam will be permanently deleted and cannot be recovered. Are you sure?",
                buttons: {
                    confirm: {
                        label: "DELETE?",
                        className: 'btn-danger'
                    },
                    cancel: {
                        label: "CANCEL",
                        className: 'btn-default'
                    }
                },
                callback: (result) => {
                    if (result) {
                        loadingSpinner.show();
                        deleteExamById(examId)
                            .then(value => {
                                if (value) {
                                    loadingSpinner.hide();
                                    bootbox.alert(`Exam is deleted`, () => location.reload(true));
                                }
                            })
                            .catch(error => {
                                console.log(error);
                            });
                    }
                }
            });

        });

        // End
    });
})(jQuery);