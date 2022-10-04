// IIFE to wrap jQuery code

(($) => {
    $(() => {
        // Initialization
        const $QuestionsDt = $('#tbl-questions').DataTable();

        // bs alert
        const showAndDismissSuccessAlert = function (message) {
            let html = `<div class="alert alert-success" role="alert" id="js-qoEntry-alert-success">`;
            html += `<button type="button" class="close" data-dismiss="alert" aria-label="Close" data-form-type=""><span aria-hidden="true">×</span></button>`;
            html += `<strong>Success!</strong> ${message} </div>`;

            $("#js-qoEntry-bs-alert").append(html);
            $("#js-qoEntry-alert-success").addClass("animated tada");

            setTimeout(function () {
                $("#js-qoEntry-alert-success").addClass("animated fadeOut").remove();
            }, 10000);
        };

        const getReOrderedQuestionFromClient = function () {
            let rows = new Array();

            $("#tbl-questions").find("tbody tr").each(function (index, element) {
                let content = {
                    ExamId: $("#js_examId").val(),
                    QuestionId: $(element).find("td:eq(5)").children(".js-qoEditModalPopup").attr("data-question-id"),
                    Serial: $.trim($(element).find("td:eq(0)").text()),
                    Marks: $.trim($(element).find("td:eq(1)").text()),
                    Description: $.trim($(element).find("td:eq(2)").text()),
                    OptionType: $.trim($(element).find("td:eq(3)").text())
                };
                rows.push(content);    
            });

            return rows;
        };

        // Re-Order Questions serial on server
        const reOrderQuestionsSerialOnServer = function(questions) {
            return $.ajax({
                type: "PUT",
                url: "/QuestionAnswer/IsQuestionsReOrdered",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(questions)
            });
        };

        // For every call of this function, questions serial no. will reset using loop index
        const reOrderQuestionsSerialOnClient = function () {
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

                    // re-order serial on client
                    reOrderQuestionsSerialOnClient();

                    try {
                        // marked as removed on server
                        let removedRes = await isQuestionRemovedOnServer(closestRow.find("td:eq(5) > a").data("question-id"));

                        if (!removedRes) {
                            return;
                        }

                        // get current tableRow and re-order question on server
                        let reOrderedRows = getReOrderedQuestionFromClient();
                        if (!Array.isArray(reOrderedRows) || !reOrderedRows.length) {
                            return;
                        }
                        await reOrderQuestionsSerialOnServer(reOrderedRows);
                        await $QuestionsDt.ajax.reload();

                        showAndDismissSuccessAlert("This question no longer exists");
                    } catch (error) {
                        if (error.status === 500) {
                            window.location = "/Error/Error500";
                            return;
                        }

                        if (error.status === 404) {
                            window.location = "/Error/Error404";
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