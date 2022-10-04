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

        const isQuestionRemovedOnServer = function (questionId) {
            let reqData = {
                QuestionId: questionId,
                ExamId: $("#js_examId").val(),
                IsDeleted: true
            };

            return $.ajax({
                type: "PUT",
                url: "/QuestionAnswer/IsAssignedQuestionRemoved",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(reqData)
            });
        };

        const reDrawQuestionDataTable = function () {
            $QuestionsDt.ajax.reload();
            console.log("reloaded");
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

                    try {
                        // marked as removed on server
                        let questionId = closestRow.find("td:eq(5) > a").data("question-id");
                        let removedRes = await isQuestionRemovedOnServer(questionId);

                        if (!removedRes) {
                            return;
                        }

                        // refresh questions table (from server)
                        await reDrawQuestionDataTable();

                        if (typeof localStorage !== "undefined") {
                            localStorage.setItem("success", "Question Removed");
                        }
                        //window.location.reload();
                    } catch (error) {
                        if (error.status === 500) {
                            //window.location = "/Error/Error500";
                            return;
                        }

                        if (error.status === 404) {
                            //window.location = "/Error/Error404";
                            return;
                        }

                        bootbox.alert("Something is wrong. Check log for more info");
                        console.log({ error });
                        return;
                    }
                }
            });
        });
    });
})(jQuery);