// IIFE to wrap all jQuery code

(($) => {

    $(() => {
        $("#dataTable-examList").DataTable();
    });

    const deleteExamById = (examId) => {
        //$.post("/Exam/Delete", { examId: examId })
        //    .done((result) => {
        //        return result;
        //    })
        //    .fail((jqXhr, textStatus) => {
        //        console.log(`ERROR >> ${textStatus}`);
        //    });
        return new Promise((resolve, reject) => {
            setTimeout(() => {
                
                resolve(true);
            },1000);
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
                    // start the spinner
                    deleteExamById(examId)
                        .then(value => {
                            if (value) {
                                //end spinner
                                console.log('promise fulfilled');
                                bootbox.alert(`Exam is deleted`, () => location.reload(true));
                            }
                    });
                }
            }
        });

    });
})(jQuery);