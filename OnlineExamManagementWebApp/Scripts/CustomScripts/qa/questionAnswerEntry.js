
// IIFE to wrap jQuery code

(($) => {
    $(() => {
        //start
        const examId = $('#js_examId').val();
        const middleRowPanel = $('#qa_entry_middleRow');
        const btnToggleQaSection = $('#btn-toggle-qaSection');
        const tblQuestions = $('#tbl-questions');

        const toggleDom = (domElement, status) => {
            if (status === "hide") {
                if (domElement.is(":visible")) {
                    domElement.hide();
                }
                return;
            }
            if (status === "show") {
                if (domElement.is(":hidden")) {
                    domElement.show();
                }
                return;
            }
        }

        toggleDom(middleRowPanel, "hide");

        $('#hide-section').on('click', function () {
            toggleDom(middleRowPanel, "hide");
        });

        btnToggleQaSection.on('click', function () {
            toggleDom(middleRowPanel, "show");
        });

        // fetch list of questions
        const getQuestionsByExamId = (examId) => {
            return new Promise((resolve) => {
                $.get(`/QuestionAnswer/GetQuestionsByExamId?id=${examId}`)
                    .done((data) => {
                        resolve(data);
                    });
            });
        }

        $('#btn_testCallback').on('click', async () => {
            let result = await getQuestionsByExamId(examId);
            console.log(result);
        });


        tblQuestions.DataTable();

        //end
    });
})(jQuery);

