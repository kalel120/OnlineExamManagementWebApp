// IIFE to wrap jQuery code

(($) => {
    $(() => {
        // Initialization
        const $QuestionsDt = $('#tbl-questions').DataTable();

        // For every call of this function, questions serial no. will reset using loop index
        const reOrderQuestionSerialOnClient = function () {
            $("#tbl-questions").find("tbody tr td:nth-child(1)").each(function (index, element) {
                $(element).text(index + 1);
            });
        };

        const removeQuestionOnServer = function(questionId) {

        };

        $(document).on("click", ".js-removeQuestion", function (event) {
            let eventTarget = $(event.target);

            let closestRow = eventTarget.closest("tr");

            bootbox.confirm({
                size: "large",
                message: "Are you sure? This will delete this particular question",
                callback: async function (confirmed) {
                    if (!confirmed) {
                        return;
                    }

                    // remove from questions table (client)
                    $QuestionsDt.rows(closestRow).remove().draw();

                    // re-order serial no. on client
                    reOrderQuestionSerialOnClient();

                    // marked as removed on server
                    console.log(closestRow.find("td:eq(5) > a").data("question-id"));
                    // re-sequence on server
                    // refresh questions table (from serve)
                    //if (typeof localStorage !== "undefined") {
                    //    localStorage.setItem("success", "Question Removed");
                    //}
                    //window.location.reload();
                }
            });
        });
    });
})(jQuery);